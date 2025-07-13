using ActualLab.CommandR;
using ActualLab.Fusion;
using Microsoft.AspNetCore.Http;
using myuzbekistan.Shared;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace myuzbekistan.Services;


public class MultiCardService(IHttpClientFactory httpClientFactory, IHttpContextAccessor contextAccessor, IPaymentService paymentService, ICommander commander, ISessionResolver sessionResolver, IConfiguration configuration)
{
    public static DateTime? ExpiredAt { get; set; }
    public static string? Token { get; set; } = null!;
    

    public async Task GetToken()
    {
        if (ExpiredAt != null && ExpiredAt > DateTime.UtcNow)
            return;
        
        var body = new StringContent($$"""
            {
                "application_id": "{{configuration["Multicard:ApplicationId"]}}",
                "secret":"{{configuration["Multicard:Secret"]}}"

            }
            """, Encoding.UTF8, "application/json");

        var client = httpClientFactory.CreateClient("multicard");
        client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Token);
        var response = await client.SendAsync(new HttpRequestMessage(HttpMethod.Post, client.BaseAddress + "auth")
        {
            Content = body
        });

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception("Failed to get token from Multicard API");
        }
        var content = await response.Content.ReadAsStringAsync();
        var tokenResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<MultiTokenResponse>(content);
        Token = tokenResponse?.Token;
        ExpiredAt = tokenResponse?.ExpiredAt;

    }


    private async Task<string> SendPostRequestAsync(string url, object requestBody, HttpMethod? method )
    {
        ArgumentNullException.ThrowIfNull(method);
        await GetToken();
        var client = httpClientFactory.CreateClient("multicard");
        client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Token);
        var json = JsonSerializer.Serialize(requestBody);
        var body = new StringContent(json, Encoding.UTF8, "application/json");

        var httpRequest = new HttpRequestMessage
        {
            Method = method ?? HttpMethod.Post,
            RequestUri = new Uri(url),
            Content = body
        };
        var response = await client.SendAsync(httpRequest);
        var responseString = await response.Content.ReadAsStringAsync();

        if (response.StatusCode == HttpStatusCode.BadRequest)
        {
            var errorWrapper = JsonSerializer.Deserialize<MultiErrorWrapper<MultiError>>(responseString);
            throw new MultiException(errorWrapper!);
        }

        response.EnsureSuccessStatusCode();
        return responseString;
    }

    public async Task<MultiBindCardResponse> BindCard(PaymentVendorCardRequest multiBindCard)
    {
        var resultString = await SendPostRequestAsync(configuration["Multicard:Url"] + "payment/card", multiBindCard, HttpMethod.Post);

        var content = JsonSerializer.Deserialize<MultiWrapper<MultiBindCardResponse>>(resultString)!;
        return content.Data!;
    }


    public async Task<CardView> ConfirmBindCard(string cardToken, string otp)
    {
        var resultString = await SendPostRequestAsync(configuration["Multicard:Url"] + $"payment/card/{cardToken}", new { otp }, HttpMethod.Put);

        var content = JsonSerializer.Deserialize<MultiWrapper<CardView>>(resultString)!;
        return content.Data!;
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

    private int storeId = 1720;
    //private int storeId = 6;

    public async Task<string> CreateInvoice(decimal amount)
    {
        
        var httpContext = contextAccessor.HttpContext!;

        var paymentRequest = new MultiPaymentRequest
        {
            Amount = amount,
            StoreId = storeId,
            InvoiceId = Guid.NewGuid().ToString(),
            Ofd = [GetOfd(amount)]


        };
        var resultString = await SendPostRequestAsync(configuration["Multicard:Url"] + "payment/invoice", paymentRequest, HttpMethod.Post);
        var content = JsonSerializer.Deserialize<MultiWrapper<MultiPaymentResponse>>(resultString);

        return content?.Data!.Uuid ?? throw new Exception("Failed to create payments");
    }

    public async Task<string> CreatePayment(long userId, decimal amount, string token)
    {
        var invoiceId = await CreateInvoice(amount);
        var httpContext = contextAccessor.HttpContext!;

        var paymentRequest = new MultiPaymentRequest
        {
            Amount = amount,
            StoreId = storeId,
            InvoiceId = invoiceId,
            BillingId = userId,
            DeviceDetails = new DeviceDetails
            {
                Ip = httpContext.Connection.RemoteIpAddress?.ToString(),
                UserAgent = httpContext.Request.Headers.UserAgent.ToString()
            },
            Card = new MultiCard { Token = token },
            Ofd = [GetOfd(amount)]
        };

        var resultString = await SendPostRequestAsync(configuration["Multicard:Url"] + "payment", paymentRequest, HttpMethod.Post);
        var content = JsonSerializer.Deserialize<MultiWrapper<MultiPaymentResponse>>(resultString);
        var session = await sessionResolver.GetSession();
        await commander.Call(new CreatePaymentCommand(session, new PaymentView
        {
            Amount = amount,
            PaymentStatus = PaymentStatus.Pending,
            TransactionId = content?.Data!.Uuid,
            CallbackData = resultString,
            UserId = userId,
        }));

        return content?.Data!.Uuid ?? throw new Exception("Failed to create payments");
    }

    public async Task ConfirmPayment(string paymentId, string otp)
    {
        await GetToken();
        var client = httpClientFactory.CreateClient("multicard");
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
        var body = new StringContent($$"""
            {
                "otp": "{{otp}}"
            }
            """, Encoding.UTF8, "application/json");

        var resultString = await SendPostRequestAsync(configuration["Multicard:Url"] + $"payment/{paymentId}", body, HttpMethod.Put);
        var paymentResponse = JsonSerializer.Deserialize<MultiWrapper<MultiPaymentResponse>>(resultString)?.Data; 
        var session = await sessionResolver.GetSession();
        var payment = await paymentService.GetByTransactionId(paymentId);
        payment.PaymentStatus = paymentResponse?.Status == "success" ? PaymentStatus.Completed : PaymentStatus.Failed;
        await commander.Call(new UpdatePaymentCommand(session, payment));

        await commander.Call(new IncrementUserBalanceCommand(session, payment.UserId, payment?.Amount ?? 0));


    }
}
