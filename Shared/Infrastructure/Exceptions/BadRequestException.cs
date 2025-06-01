using System.Text.Json.Serialization;

namespace myuzbekistan.Shared;

public class BadRequestException : SystemException
{

}
public class MultiException(MultiErrorWrapper<MultiError> multiErrorWrapper) : SystemException
{
    public MultiErrorWrapper<MultiError> MultiErrorWrapper { get; } = multiErrorWrapper;
}

public class ErrorResponse(int status, string code, string message)
{
    //[JsonPropertyName("status")]
    [Newtonsoft.Json.JsonIgnore]
    public int Status { get; } = status;

    [JsonPropertyName("code")]
    public string Code { get; } = code;

    [JsonPropertyName("message")]
    public string Message { get; } = message;
}


public class ServiceException(string service, string method, string message, string code = "400") : Exception(message)
{
    public string Service { get; set; } = service;
    public string Method { get; set; } = method;
    public string Code { get; set; } = code;
    public new string Message { get; set; } = message;
}
