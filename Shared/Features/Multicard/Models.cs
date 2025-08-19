using Newtonsoft.Json;
using Shared.Localization;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace myuzbekistan.Shared;



public class PaymentVendorCardRequest
{
    [JsonPropertyName("pan")]
    [JsonProperty("pan")]
    [Required]
    public string Token { get; set; } = null!;
    [JsonPropertyName("expiry")]
    [JsonProperty("expiry")]
    [RegularExpression(@"^\d{2}((0[1-9])|(1[0-2]))$", ErrorMessage = "Expiry должен быть в формате YYMM (например, 2603)")]
    public string? Expiry { get; set; }

    [JsonPropertyName("smsNotificationNumber")]
    [JsonProperty("smsNotificationNumber")]
    [RegularExpression(@"^998\d{9}$",ErrorMessageResourceType = typeof(SharedResource), ErrorMessageResourceName= "NumberFormatForGlobalPay")]
    public string? SmsNotificationNumber { get; set; } = null!;
    [JsonPropertyName("cardHolderName")]
    [JsonProperty("cardHolderName")]
    public string? CardHolderName { get; set; }

    [JsonPropertyName("cvv")]
    [JsonProperty("cvv")]
    public int? Cvv { get; set; } = null!;

    [JsonPropertyName("image")]
    [JsonProperty("image")]
    public string? Image { get; set; } = null!;

}

public class MultiTokenResponse
{
    [JsonPropertyName("token")]
    [JsonProperty("token")]
    public string? Token { get; set; }
    [JsonPropertyName("expiry")]
    [JsonProperty("expiry")]
    public DateTime? ExpiredAt { get; set; }
}


public class MultiWrapper<T>
{
    [JsonPropertyName("data")]
    public T? Data { get; set; }
    [JsonPropertyName("message")]
    public string? Message { get; set; }
    [JsonPropertyName("success")]
    public bool Success { get; set; }
}


public class MultiError
{
    [JsonPropertyName("code")]
    public string Code { get; set; }

    [JsonPropertyName("details")]
    public string Details { get; set; }
}
public class MultiErrorWrapper<T>
{
    [JsonPropertyName("error")]
    public MultiError? Error { get; set; }
    
    [JsonPropertyName("success")]
    public bool Success { get; set; }
}
public class MultiBindCardResponse
{
    [JsonPropertyName("card_token")]
    public string CardToken { get; set; } = null!;
    [JsonPropertyName("phone")]
    public string Phone { get; set; } = null!;
}
public class MultiConfirmCardRequest
{
    [JsonPropertyName("otp")]
    public string Otp { get; set; } = null!;
}

public class MultiCard
{
    [JsonPropertyName("token")]
    [JsonProperty("token")]
    public string Token { get; set; } = null!;
}

public class DeviceDetails
{
    [JsonPropertyName("Ip")]
    [JsonProperty("Ip")]
    public string? Ip { get; set; }
    [JsonPropertyName("UserAgent")]
    [JsonProperty("UserAgent")]
    public string? UserAgent { get; set; }
}

public class MultiPaymentRequest
{
    [JsonPropertyName("card")]
    [JsonProperty("card")]
    public MultiCard Card { get; set; } = null!;
    [JsonPropertyName("callback_url")]
    [JsonProperty("callback_url")]
    public string CallbackUrl { get; set; } = null!;
    [JsonPropertyName("amount")]
    [JsonProperty("amount")]
    public decimal Amount { get; set; }
    [JsonPropertyName("store_id")]
    [JsonProperty("store_id")]
    public int StoreId { get; set; }
    [JsonPropertyName("invoice_id")]
    [JsonProperty("invoice_id")]
    public string InvoiceId { get; set; } = null!;
    [JsonPropertyName("billing_id")]
    [JsonProperty("billing_id")]
    public long? BillingId { get; set; }
    [JsonPropertyName("device_details")]
    [JsonProperty("device_details")]
    public DeviceDetails DeviceDetails { get; set; } = null!;
    [JsonPropertyName("ofd")]
    [JsonProperty("ofd")]
    public List<MultiOfd>? Ofd { get; set; }
}
public class MultiOfd
{
    [JsonProperty("vat")]
    [JsonPropertyName("vat")]
    public int Vat { get; set; }

    [JsonProperty("price")]
    [JsonPropertyName("price")]
    public decimal Price { get; set; }

    [JsonProperty("qty")]
    [JsonPropertyName("qty")]
    public int Qty { get; set; }

    [JsonProperty("name")]
    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonProperty("package_code")]
    [JsonPropertyName("package_code")]
    public string PackageCode { get; set; }

    [JsonProperty("mxik")]
    [JsonPropertyName("mxik")]
    public string Mxik { get; set; }

    [JsonProperty("total")]
    [JsonPropertyName("total")]
    public decimal Total { get; set; }
}

public class MultiPaymentResponse
{
    [JsonPropertyName("status")]
    [JsonProperty("status")]
    public string Status { get; set; } = null!;
    [JsonPropertyName("uuid")]
    [JsonProperty("uuid")]
    public string Uuid { get; set; } = null!;
}