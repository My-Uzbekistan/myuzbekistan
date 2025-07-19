using System.Text.Json.Serialization;
using System.Runtime.CompilerServices;

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


public class MyUzException : Exception
{
    public string Service { get; set; }
    public string Method { get; set; }
    public string Code { get; set; }
    public new string Message { get; set; }

    public MyUzException(
        string message,
        string code = "400",
        [CallerFilePath] string service = "",
        [CallerMemberName] string method = "") : base(message)
    {
        Service = System.IO.Path.GetFileNameWithoutExtension(service); // Извлекаем имя файла без расширения
        Method = method;
        Code = code;
        Message = message;
    }
}
