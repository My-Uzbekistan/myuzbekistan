using Microsoft.AspNetCore.Http;

namespace myuzbekistan.Services;

public class GPTokenResponse
{
    [JsonPropertyName("access_token")]
    public string? Token { get; set; }

    [JsonPropertyName("expires_in")]
    public int ExpiresIn { get; set; }

}


public class GPBindCardRequest
{
    [JsonPropertyName("cardNumber")]
    [JsonProperty("cardNumber")]
    [Required]
    public string CardNumber { get; set; } = null!;
    [JsonPropertyName("expiryDate")]
    [JsonProperty("expiryDate")]
    [RegularExpression(@"^\d{2}((0[1-9])|(1[0-2]))$", ErrorMessage = "Expiry должен быть в формате YYMM (например, 2603)")]
    public string? ExpiryDate { get; set; }
    [JsonPropertyName("smsNotificationNumber")]
    [JsonProperty("smsNotificationNumber")]
    [Required]
    public string? SmsNotificationNumber { get; set; } = null!;
    [JsonPropertyName("cardHolderName")]
    [JsonProperty("cardHolderName")]
    [Required]
    public string? CardHolderName { get; set; } = null!;

}

public class GPBindCardResponse
{
    [JsonPropertyName("cardToken")]
    public string CardToken { get; set; } = null!;

    [JsonPropertyName("smsNotificationNumber")]
    public string SmsNotificationNumber { get; set; } = null!;

    [JsonPropertyName("needsVerification")]
    public bool NeedsVerification { get; set; }

    [JsonPropertyName("bankName")]
    public string BankName { get; set; } = null!;

    [JsonPropertyName("processingType")]
    public string ProcessingType { get; set; } = null!;
}

public class GPBindCardConfirmResponse
{
    [JsonPropertyName("token")]
    public string Token { get; set; } = null!;

    [JsonPropertyName("cardNumber")]
    public string CardNumber { get; set; } = null!;

    [JsonPropertyName("balance")]
    public long Balance { get; set; }

    [JsonPropertyName("expiryDate")]
    public string ExpiryDate { get; set; } = null!;

    [JsonPropertyName("processingType")]
    public string ProcessingType { get; set; } = null!;

    [JsonPropertyName("holderFullName")]
    public string HolderFullName { get; set; } = null!;

    [JsonPropertyName("bankName")]
    public string BankName { get; set; } = null!;

    [JsonPropertyName("cardOrigin")]
    public string CardOrigin { get; set; } = null!;
}

public class GPCreatePaymentRequest
{
    [JsonPropertyName("externalId")]
    public string ExternalId { get; set; } = Guid.NewGuid().ToString();

    [JsonPropertyName("serviceId")]
    public int ServiceId { get; set; }

    [JsonPropertyName("amount")]
    public decimal Amount { get; set; }

    [JsonPropertyName("currency")]
    public string Currency { get; set; } = "UZS";

    [JsonPropertyName("account")]
    public string Account { get; set; } = null!;

    [JsonPropertyName("description")]
    public string? Description { get; set; }

    [JsonPropertyName("items")]
    public List<GPCreatePaymentItem?> Items { get; set; } = new();
}

public class GPCreatePaymentItem
{
    [JsonPropertyName("title")]
    public string Title { get; set; } = null!;

    [JsonPropertyName("price")]
    public decimal Price { get; set; }

    [JsonPropertyName("count")]
    public int Count { get; set; }

    [JsonPropertyName("code")]
    public string Code { get; set; } = null!;

    [JsonPropertyName("units")]
    public int Units { get; set; }

    [JsonPropertyName("vatPercent")]
    public int VatPercent { get; set; }

    [JsonPropertyName("packageCode")]
    public string PackageCode { get; set; } = null!;
}

public class GPCreatePaymentResponse
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = null!;

    [JsonPropertyName("externalId")]
    public string ExternalId { get; set; } = null!;

    [JsonPropertyName("status")]
    public string Status { get; set; } = null!;

    [JsonPropertyName("paymentItems")]
    public List<GPCreatePaymentItem> PaymentItems { get; set; } = new();
}



public class GPConfirmPaymentRequest
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = null!;

    [JsonPropertyName("externalId")]
    public string ExternalId { get; set; } = null!;

    [JsonPropertyName("cardToken")]
    public string CardToken { get; set; } = null!;

    [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("cardSecurityCode")]
    public string? CardSecurityCode { get; set; } // Опционально

    [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("clientIpAddress")]
    public string? ClientIpAddress { get; set; } // Опционально
}

public class GPConfirmPaymentResponse
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = null!;

    [JsonPropertyName("externalId")]
    public string ExternalId { get; set; } = null!;

    [JsonPropertyName("processingId")]
    public string ProcessingId { get; set; } = null!;

    [JsonPropertyName("amount")]
    public decimal Amount { get; set; }

    [JsonPropertyName("currency")]
    public string Currency { get; set; } = null!;

    [JsonPropertyName("type")]
    public string Type { get; set; } = null!;

    [JsonPropertyName("status")]
    public string Status { get; set; } = null!;

    [JsonPropertyName("processingType")]
    public string ProcessingType { get; set; } = null!;

    [JsonPropertyName("createdAt")]
    public DateTime CreatedAt { get; set; }

    [JsonPropertyName("approvedAt")]
    public DateTime? ApprovedAt { get; set; }

    [JsonPropertyName("gnkPerformedAt")]
    public DateTime? GnkPerformedAt { get; set; }

    [JsonPropertyName("account")]
    public string Account { get; set; } = null!;

    [JsonPropertyName("card")]
    public GPConfirmPaymentCard Card { get; set; } = null!;

    [JsonPropertyName("gnkFields")]
    public GPConfirmPaymentGnkFields? GnkFields { get; set; }

    [JsonPropertyName("paymentItems")]
    public List<GPCreatePaymentItem> PaymentItems { get; set; } = new();

    [JsonPropertyName("service")]
    public GPConfirmPaymentService Service { get; set; } = null!;
}

public class GPConfirmPaymentCard
{
    [JsonPropertyName("token")]
    public string Token { get; set; } = null!;

    [JsonPropertyName("cardNumber")]
    public string CardNumber { get; set; } = null!;

    [JsonPropertyName("expiryDate")]
    public string ExpiryDate { get; set; } = null!;

    [JsonPropertyName("smsNotificationNumber")]
    public string SmsNotificationNumber { get; set; } = null!;

    [JsonPropertyName("processingType")]
    public string ProcessingType { get; set; } = null!;

    [JsonPropertyName("cardOrigin")]
    public string CardOrigin { get; set; } = null!;
}

public class GPConfirmPaymentGnkFields
{
    [JsonPropertyName("receiptId")]
    public long ReceiptId { get; set; }

    [JsonPropertyName("terminalId")]
    public string TerminalId { get; set; } = null!;

    [JsonPropertyName("fiscalSign")]
    public string FiscalSign { get; set; } = null!;

    [JsonPropertyName("success")]
    public bool Success { get; set; }

    [JsonPropertyName("message")]
    public string Message { get; set; } = null!;

    [JsonPropertyName("qrcodeUrl")]
    public string QrCodeUrl { get; set; } = null!;
}

public class GPConfirmPaymentService
{
    [JsonPropertyName("id")]
    public int Id { get; set; }
}

public class GlobalPayService(
    IHttpClientFactory httpClientFactory,
    IHttpContextAccessor contextAccessor,
    IPaymentService paymentService,
    ICardService cardService,
    ICommander commander,
    ISessionResolver sessionResolver,
    IConfiguration configuration)
{
    private static DateTime? _expiredAt;
    private static string? _token;
    private static readonly SemaphoreSlim _tokenSemaphore = new(1, 1);

    public async Task<GPBindCardResponse> BindCard(GPBindCardRequest bindCardRequest)
    {
        var resultString = await SendRequestAsync("cards/v1/card", bindCardRequest, HttpMethod.Post);
        var content = JsonSerializer.Deserialize<GPBindCardResponse>(resultString)!;
        return content;
    }

    public async Task<CardView> ConfirmBindCard(string cardToken, string otp)
    {
        var resultString = await SendRequestAsync($"cards/v1/card/confirm/{cardToken}", new { code = otp }, HttpMethod.Post);
        var content = JsonSerializer.Deserialize<GPBindCardConfirmResponse>(resultString)!;

        return new CardView
        {
            CardToken = content.Token,
            CardPan = content.CardNumber,
            ExpirationDate = content.ExpiryDate,
            HolderName = content.HolderFullName,
            Phone = content.CardNumber,
            Ps = content.ProcessingType,
            Status = "Active",
            Name = content.BankName,
            Balance = content.Balance / 100m, // Предполагается, что баланс возвращается в копейках
        };
    }

    private MultiOfd GetOfd(decimal amount) => new MultiOfd
    {
        Vat = 0,
        Price = amount,
        Qty = 1,
        Name = "Sayyohlik agentliklari va turoperatorlarning xizmatlari",
        Mxik = "10703999001000000",
        PackageCode = "1495086",
        Total = amount,
    };

    private int serviceId = 110;

    public async Task<string> CreateInvoice(decimal amount)
    {
        var paymentRequest = new MultiPaymentRequest
        {
            Amount = amount,
            StoreId = serviceId,
            InvoiceId = Guid.NewGuid().ToString(),
            Ofd = [GetOfd(amount)]
        };

        var resultString = await SendRequestAsync("/payments/v1/merchant/invoice", paymentRequest, HttpMethod.Post);
        var content = JsonSerializer.Deserialize<MultiWrapper<MultiPaymentResponse>>(resultString);
        return content?.Data!.Uuid ?? throw new Exception("Failed to create invoice");
    }

    public async Task<GPCreatePaymentResponse> CreatePayment(long userId, decimal amount, CardView card)
    {
        var externalId = Guid.NewGuid().ToString();
        var paymentRequest = new GPCreatePaymentRequest
        {
            ExternalId = externalId,
            ServiceId = serviceId,
            Amount = amount,
            Account = userId.ToString(),
            Description = "Payment",
            Items =
            [
                new GPCreatePaymentItem
                {
                    Title = "Sayyohlik agentliklari va turoperatorlarning xizmatlari",
                    Price = amount,
                    Count = 1,
                    Code = "10703999001000000",
                    Units = 1,
                    VatPercent = 0,
                    PackageCode = "1495086"
                }
            ]
        };

        var resultString = await SendRequestAsync("payments/v2/payment/init", paymentRequest, HttpMethod.Post);
        var content = JsonSerializer.Deserialize<GPCreatePaymentResponse>(resultString)!;

        var session = await sessionResolver.GetSession();
        await commander.Call(new CreatePaymentCommand(session, new PaymentView
        {
            UserId = userId,
            Amount = amount,
            CardId = card.Id,
            PaymentStatus = PaymentStatus.Pending,
            TransactionId = content.Id,
            ExternalId = externalId,
            CallbackData = resultString
        }));

        return content;
    }

    public async Task<GPConfirmPaymentResponse> ConfirmPayment(string paymentId, string? cardSecurityCode = null, string? clientIpAddress = null)
    {
        var payment = await paymentService.GetByExternalId(paymentId);
        var card = await cardService.Get(payment.CardId, payment.UserId);
        var paymentRequest = new GPConfirmPaymentRequest
        {
            Id = payment.TransactionId!,
            ExternalId = payment.ExternalId!,
            CardToken = card.CardToken,
            //CardSecurityCode = cardSecurityCode,
            //ClientIpAddress = clientIpAddress
        };

        var resultString = await SendRequestAsync("payments/v2/payment/perform", paymentRequest, HttpMethod.Post);
        var content = JsonSerializer.Deserialize<GPConfirmPaymentResponse>(resultString)!;

        // Обновление статуса платежа в базе данных
        var session = await sessionResolver.GetSession();

        payment.PaymentStatus = content.Status == "APPROVED" ? PaymentStatus.Completed : PaymentStatus.Failed;
        payment.CallbackData = resultString;
        await commander.Call(new UpdatePaymentCommand(session, payment));

        if (payment.PaymentStatus == PaymentStatus.Completed)
        {
            await commander.Call(new IncrementUserBalanceCommand(session, payment.UserId, content.Amount));
        }

        return content;
    }


    public async Task GetToken()
    {
        // Проверяем с запасом в 30 секунд
        if (_expiredAt != null && _expiredAt > DateTime.UtcNow.AddSeconds(30))
            return;

        await _tokenSemaphore.WaitAsync();
        try
        {
            // Повторная проверка после получения блокировки
            if (_expiredAt != null && _expiredAt > DateTime.UtcNow.AddSeconds(30))
                return;

            var body = new StringContent($$"""
                {
                    "username": "{{configuration["GlobalPay:Username"]}}",
                    "password": "{{configuration["GlobalPay:Password"]}}"
                }
                """, Encoding.UTF8, "application/json");

            var client = httpClientFactory.CreateClient("globalpay");
            var response = await client.SendAsync(new HttpRequestMessage(HttpMethod.Post, "/payments/v1/merchant/auth")
            {
                Content = body
            });

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new Exception($"Failed to get token from GlobalPay API. Status: {response.StatusCode}, Content: {errorContent}");
            }

            var content = await response.Content.ReadAsStringAsync();
            var tokenResponse = JsonSerializer.Deserialize<GPTokenResponse>(content);

            _token = tokenResponse?.Token;
            _expiredAt = DateTime.UtcNow.AddSeconds(tokenResponse?.ExpiresIn ?? 0);
        }
        finally
        {
            _tokenSemaphore.Release();
        }
    }

    private async Task<string> SendRequestAsync(string url, object requestBody, HttpMethod? method)
    {
        ArgumentNullException.ThrowIfNull(method);
        await GetToken();

        var client = httpClientFactory.CreateClient("globalpay");
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);

        var json = JsonSerializer.Serialize(requestBody);
        var body = new StringContent(json, Encoding.UTF8, "application/json");

        var httpRequest = new HttpRequestMessage
        {
            Method = method ?? HttpMethod.Post,
            RequestUri = new Uri(url, UriKind.Relative),
            Content = body
        };

        var response = await client.SendAsync(httpRequest);
        var responseString = await response.Content.ReadAsStringAsync();

        if ((int)response.StatusCode >= 400)
        {
            var errorWrapper = JsonSerializer.Deserialize<MultiErrorWrapper<MultiError>>(responseString);
            throw new MultiException(errorWrapper!);
        }

        response.EnsureSuccessStatusCode();
        return responseString;
    }
}