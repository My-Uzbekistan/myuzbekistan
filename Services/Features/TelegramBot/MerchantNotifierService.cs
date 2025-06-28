using myuzbekistan.Services;

public class MerchantNotifierService
{
    private readonly TelegramBotService _bot;
    private readonly IServiceScopeFactory _scopeFactory;

    public MerchantNotifierService(TelegramBotService bot,
                                   IServiceScopeFactory scopeFactory)
    {
        _bot = bot;
        _scopeFactory = scopeFactory;
    }

    public async Task Notify(long categoryId, string message, CancellationToken ct = default)
    {
        if (_bot is null)
            throw new InvalidOperationException("Telegram бот не инициализирован");

        await using var scope = _scopeFactory.CreateAsyncScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        var categoryGroup = await db.MerchantCategories
            .Include(x => x.Merchants)
            .Where(x => x.Id == categoryId)
            .ToListAsync(ct);

        if (categoryGroup.Count == 0)
            return;

        var mainCategory = categoryGroup.First();

        IEnumerable<string?> chatIds = mainCategory.ChatIds?.Where(x => !string.IsNullOrWhiteSpace(x))
                                   ?? Enumerable.Empty<string>();

        if (!chatIds.Any())
        {
            chatIds = categoryGroup
                .SelectMany(x => x.Merchants)
                .SelectMany(x => x.ChatIds ?? Enumerable.Empty<string?>())
                .Where(x => !string.IsNullOrWhiteSpace(x));
        }

        foreach (var chatId in chatIds.Distinct())
            await _bot.NotifyAsync(chatId!, message, ct);
    }
}

