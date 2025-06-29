using Microsoft.EntityFrameworkCore;
using myuzbekistan.Services;
using RTools_NTS.Util;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

public  sealed class TelegramBotService : BackgroundService
{
    private readonly ILogger<TelegramBotService> _log;
    private readonly IServiceProvider _sp;
    private readonly IConfiguration _cfg;
    private TelegramBotClient? _bot;

    public TelegramBotService(ILogger<TelegramBotService> log,
                              IServiceProvider sp,
                              IConfiguration cfg)
    {
        _log = log;
        _sp = sp;
        _cfg = cfg;
        var token = _cfg["Telegram:BotToken"];
        if(token != null)
        _bot = new TelegramBotClient(token);
    }


    public async Task NotifyAsync(string chatId, string message, CancellationToken ct = default)
    {
        if (_bot is null)
            return;

        await _bot.SendMessage(chatId, message, cancellationToken: ct);
    }

    /* ------------------ запуск ------------------ */
    protected override async Task ExecuteAsync(CancellationToken ct)
    {
        if (_bot == null) return;
        

        // Обработчики polling'а
        _bot.StartReceiving(
            updateHandler: HandleUpdate,
            errorHandler: HandleError,
            receiverOptions: new ReceiverOptions
            {
                AllowedUpdates = new[] { UpdateType.Message }
            },
            cancellationToken: ct);

        var me = await _bot.GetMe(ct);
        _log.LogInformation("Telegram‑бот запущен: @{User}", me.Username);
    }

    /* -------- обработка входящих обновлений -------- */
    private async Task HandleUpdate(ITelegramBotClient bot,
                                    Update update,
                                    CancellationToken ct)
    {
        // Нас интересуют только текстовые сообщения
        if (update.Type != UpdateType.Message || update.Message!.Type != MessageType.Text)
            return;

        var chatId = update.Message.Chat.Id;
        var chatIdStr = chatId.ToString();
        var text = update.Message.Text!.Trim();

        // /start → приветствие
        if (text.Equals("/start", StringComparison.OrdinalIgnoreCase))
        {
            await bot.SendMessage(chatId,
                "Добро пожаловать в MyUzbekistan Finance!\n" +
                "Отправьте свой токен для привязки.");
            return;
        }

        /* -------- DI‑scope -------- */
        await using var scope = _sp.CreateAsyncScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var commander = scope.ServiceProvider.GetRequiredService<ICommander>();

        /* === 1) MerchantCategory === */
        var category = await db.MerchantCategories
                               .AsNoTracking()
                               .FirstOrDefaultAsync(x => x.Token == text, ct);

        var session = new Session("~");

        if (category is not null)
        {
            await commander.Call(
                new MerchantCategoryAddChatIdCommand(session , text,chatIdStr ), ct);

            await bot.SendMessage(chatId,
                $"✅ Токен принят! Вы привязаны к мерчанту‑категории «{category.BrandName}».");
            return;
        }

        /* === 2) Merchant === */
        var merchant = await db.Merchants
                               .AsNoTracking()
                               .FirstOrDefaultAsync(x => x.Token == text, ct);

        if (merchant is not null)
        {
            await commander.Call(
                new MerchantAddChatIdCommand(session , text,chatIdStr ), ct);

            await bot.SendMessage(chatId,
                $"✅ Токен принят! Вы привязаны к мерчанту «{merchant.Name}».");
            return;
        }

        /* === 3) Токен не найден === */
        await bot.SendMessage(chatId,
            "❌ Токен не найден. Проверьте правильность и попробуйте снова.");
    }

    /* -------- обработка ошибок -------- */
    private Task HandleError(ITelegramBotClient bot,
                         Exception exception,
                         CancellationToken ct)
    {
        _log.LogError(exception, "Telegram‑бот: ошибка обработки");
        return Task.CompletedTask;
    }
}
