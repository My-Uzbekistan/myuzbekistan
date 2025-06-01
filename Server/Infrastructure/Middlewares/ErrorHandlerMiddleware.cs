using myuzbekistan.Shared;
using OpenTelemetry.Trace;
using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Serialization;


namespace Server.Infrastructure;

public class ErrorHandlerMiddleware(RequestDelegate next, ILogger<ErrorHandlerMiddleware> logger,
    IAlertaGram alertaGram, IWebHostEnvironment env)
{
    public async Task Invoke(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        //context.Response.ContentType = "application/json";

        var activity = Activity.Current;
        if (activity != null)
        {
            activity.RecordException(exception);
            activity.SetStatus(ActivityStatusCode.Error);
        }

        ErrorResponse httpResponse;
        if (exception is ServiceException customEx)
        {
            httpResponse = new ErrorResponse(400, customEx.Code, $"{customEx.Service}---{customEx.Method}---{customEx.Message}");
        }
        else
        {
             httpResponse = exception.Message switch
            {
                "004" => new ErrorResponse(400, "004", "Identification face spoofing"),
                "005" => new ErrorResponse(400, "005", "Identification face"),
                "006" => new ErrorResponse(400, "006", "Identification data"),
                "401" => new ErrorResponse(400, "401", "Service Returned Invalid Response"),
                "5000" => new ErrorResponse(400, "5000", "Limit sim card"),
                "5009" => new ErrorResponse(400, "5009", "Financial obligations"),
                _ => new ErrorResponse(500, "500", exception.Message),
            };
        }

        logger.LogError(exception, exception.Message);

        context.Response.StatusCode = httpResponse.Status;
        await context.Response.WriteAsync(JsonSerializer.Serialize(httpResponse));

        if (env.IsProduction())
        {
            string message = httpResponse.Message;
            int maxLength = 4096;

            if (message.Length > maxLength)
            {
                message = message.Substring(0, maxLength);
            }

            await alertaGram.NotifyErrorAsync($"{message}",
                $"{httpResponse.Status}", "Simkomat");
        }
    }
}

public class HttpResponse(int status, object internalError, string message, string code, string name)
{
    [JsonPropertyName("status")]
    public int Status { get; } = status;

    [JsonPropertyName("internalError")]
    public object InternalError { get; } = internalError;

    [JsonPropertyName("message")]
    public string Message { get; } = message;

    [JsonPropertyName("code")]
    public string Code { get; } = code;

    [JsonPropertyName("name")]
    public string Name { get; } = name;
}

