using System.Net.Http.Json;
using System.Text.Json.Serialization;
using ActualLab.Fusion;
using myuzbekistan.Shared;

namespace myuzbekistan.Services;

public class SmsSendService(
    IServiceProvider services,
    ISmsTemplateService templateService,
    IHttpClientFactory httpClientFactory,
    IConfiguration cfg) : IComputeService, ISmsSendService
{
    private const string ClientName = "smsxabar";

    private sealed class SmsRequest
    {
        [JsonPropertyName("messages")] public IEnumerable<SmsMessage> Messages { get; set; } = null!;
    }
    private sealed class SmsMessage
    {
        [JsonPropertyName("recipient")] public string Recipient { get; set; } = null!;
        [JsonPropertyName("message-id")] public string MessageId { get; set; } = null!;
        [JsonPropertyName("sms")] public SmsPayload Sms { get; set; } = null!;
    }
    private sealed class SmsPayload
    {
        [JsonPropertyName("originator")] public string Originator { get; set; } = null!;
        [JsonPropertyName("content")] public SmsContent Content { get; set; } = null!;
    }
    private sealed class SmsContent
    {
        [JsonPropertyName("text")] public string Text { get; set; } = null!;
    }

    public async Task Send(SendSmsCommand command, CancellationToken cancellationToken = default)
    {
        if (Invalidation.IsActive)
        {
            _ = await Invalidate();
            return;
        }

        // fetch template variants by Id
        var variants = await templateService.Get(command.TemplateId, cancellationToken);
        var template = variants.FirstOrDefault(t => t.Locale == command.Locale)
            ?? variants.FirstOrDefault(t => t.Locale == "en")
            ?? variants.FirstOrDefault()
            ?? throw new NotFoundException("Sms template not found");

        var text = SmsTemplatePlaceholders.Apply(template.Template, command.Parameters);

        var originator = cfg.GetValue<string>("SmsXabar:Originator") ?? "3700";
        var request = new SmsRequest
        {
            Messages = new[]
            {
                new SmsMessage
                {
                    Recipient = NormalizePhone(command.Phone),
                    MessageId = Guid.NewGuid().ToString("N")[..12],
                    Sms = new SmsPayload
                    {
                        Originator = originator,
                        Content = new SmsContent { Text = text }
                    }
                }
            }
        };

        var client = httpClientFactory.CreateClient(ClientName);
        var resp = await client.PostAsJsonAsync("send", request, cancellationToken);
        if (!resp.IsSuccessStatusCode)
        {
            var err = await resp.Content.ReadAsStringAsync(cancellationToken);
            throw new BadRequestException($"SMS send failed: {resp.StatusCode} {err}");
        }
    }

    private static string NormalizePhone(string phone)
    {
        if (string.IsNullOrWhiteSpace(phone)) return phone;
        phone = new string(phone.Where(char.IsDigit).ToArray());
        if (phone.StartsWith("998")) return phone; // assume already international
        if (phone.Length == 9) return "998" + phone; // local without code
        return phone;
    }

    public Task<Unit> Invalidate() => TaskExt.UnitTask;
}
