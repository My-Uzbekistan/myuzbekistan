using Microsoft.Extensions.Localization;
using myuzbekistan.Shared;
using OpenTelemetry.Trace;
using Shared.Localization;
using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Serialization;


namespace Server.Infrastructure;

public class ErrorHandlerMiddleware(RequestDelegate next, ILogger<ErrorHandlerMiddleware> logger,
    IAlertaGram alertaGram, IWebHostEnvironment env, IStringLocalizer<SharedResource> @L)
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
        if (exception is myuzbekistan.Shared.MyUzException customEx)
        {
            httpResponse = new ErrorResponse(400, $"{customEx.Service}---{customEx.Method}---{customEx.Message}", @L[customEx.Message]);
        }
        else
        {
            httpResponse = exception switch
            {
                MultiException e when e.MultiErrorWrapper?.Error != null =>
                    new ErrorResponse(400, e.MultiErrorWrapper.Error.Code, e.MultiErrorWrapper.Error.Details),

                MultiException =>
                    new ErrorResponse(500, "500", "MultiException without details"),

                _ => exception.Message switch
                {
                    "004" => new ErrorResponse(400, "004", "Identification face spoofing"),
                    "005" => new ErrorResponse(400, "005", "Identification face"),
                    "006" => new ErrorResponse(400, "006", "Identification data"),
                    "401" => new ErrorResponse(400, "401", "Service Returned Invalid Response"),
                    "5000" => new ErrorResponse(400, "5000", "Limit sim card"),
                    "5009" => new ErrorResponse(400, "5009", "Financial obligations"),
                    _ => new ErrorResponse(400, "400", exception.Message ?? "Unknown error")
                }
            };

        }

        logger.LogError(exception, httpResponse.Code);

        // ✅ Добавляем проверку, чтобы избежать ошибки
        if (!context.Response.HasStarted)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = httpResponse.Status;
            await context.Response.WriteAsync(System.Text.Json.JsonSerializer.Serialize(httpResponse));
        }
        else
        {
            logger.LogWarning("Ответ уже начался, ошибка не может быть отправлена клиенту.");
        }


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

