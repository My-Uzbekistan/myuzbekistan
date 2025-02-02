using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics;
using Microsoft.AspNetCore.Builder;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Microsoft.Extensions.Logging;
using System.Text;

namespace Server.Infrastructure.ServiceCollection;

public static class OpenTelemetryCollection
{
    public static IServiceCollection AddOpenTelemetryFeature(this IServiceCollection services, WebApplicationBuilder builder)
    {
        string ServiceName = builder.Configuration.GetValue<string>("OpenTelemetry:ServiceName")!;
        string ServiceUrl = builder.Configuration.GetValue<string>("OpenTelemetry:Url")!;
        string customActivitySourceName = $"{ServiceName}_Source";
        var activitySource = new ActivitySource(customActivitySourceName);
        if (!string.IsNullOrEmpty(ServiceName) && !string.IsNullOrEmpty(ServiceUrl))
        {
            builder.Services.AddOpenTelemetry()
            .WithTracing(x =>
            {
                x.SetErrorStatusOnException(true);
                x.SetResourceBuilder(ResourceBuilder.CreateDefault().AddService(serviceName:ServiceName,ServiceName).AddEnvironmentVariableDetector());
                x.AddSource(customActivitySourceName);
                x.AddAspNetCoreInstrumentation(asp =>
                {
                    asp.Filter = (httpContext) =>
                    {
                        // only collect telemetry about HTTP GET requests
                        if (httpContext.Request.Path != null && httpContext.Request.Path.Value != null)
                        {
                            var path = httpContext.Request.Path.Value;
                            List<string> paths = ["/_", "favicon.ico", "Images"];

                            return !paths.Any(x => path.Contains(x));

                        }
                        return false;

                    };
                    asp.RecordException = true;
                });

                x.AddHttpClientInstrumentation(http =>
                {
                    http.EnrichWithHttpRequestMessage = (activity, request) =>
                    {
                        if (request.Content is null) return;

                        // Get raw string from request body with masking
                        var output = request.Content.ReadAsByteArrayAsync()
                            .GetAwaiter()
                            .GetResult()
                            .ToArray();
                        activity.SetTag("requestHeaderAuthorization",
                            request.Headers.Authorization?.ToString());
                        var body = Encoding.UTF8.GetString(output);
                        activity.SetTag("requestBody", body);
                        activity.SetTag("requestBody.length", body.Length.ToString());
                    };

                    http.EnrichWithHttpResponseMessage = (activity, response) =>
                    {
                        if (!response.IsSuccessStatusCode) return;

                        var output = response.Content.ReadAsByteArrayAsync()
                            .GetAwaiter()
                            .GetResult()
                            .ToArray();
                        var body = Encoding.UTF8.GetString(output);
                        activity.SetTag("responseBody", body);
                        activity.SetTag("responseBody.length", body.Length.ToString());
                    };
                    http.RecordException = true;
                });
                x.SetSampler(_ => new AlwaysOnSampler());
                x.AddSource("ActualLab.Core");
                x.AddSource("ActualLab.Rpc");
                x.AddSource("ActualLab.CommandR");
                x.AddSource("ActualLab.Fusion");
                x.AddSource("des.Services");
                x.AddSource("des.Client");
                x.AddSource("des.Server");


                x.AddOtlpExporter(options =>
                {
                    options.Endpoint = new Uri(ServiceUrl);
                });
            }).WithMetrics(x =>
            {
                x.SetResourceBuilder(ResourceBuilder.CreateDefault().AddService(serviceName:ServiceName,ServiceName));

                x.AddRuntimeInstrumentation();
                x.AddAspNetCoreInstrumentation();


                x.AddEventCountersInstrumentation(counters => counters.AddEventSources(
                   "Microsoft.AspNetCore.Hosting",
                   "Microsoft.AspNetCore.Http.Connections",
                   "System.Net.Http",
                   "System.Net.NameResolution",
                   "System.Net.Security",
                   "System.Net.Sockets")
                );
                x.AddMeter("ActualLab.Core");
                x.AddMeter("ActualLab.Rpc");
                x.AddMeter("ActualLab.CommandR");
                x.AddMeter("ActualLab.Fusion");
                x.AddMeter("ActualLab.Fusion.EntityFramework");
                x.AddOtlpExporter(options =>
                {
                    options.Endpoint = new Uri(ServiceUrl);
                });
            });

            builder.Logging.AddOpenTelemetry(x =>
            {
                x.SetResourceBuilder(ResourceBuilder.CreateDefault().AddService(serviceName:ServiceName,ServiceName));
                x.IncludeFormattedMessage = true;
                x.IncludeScopes = true;
                x.ParseStateValues = true;

                x.AddOtlpExporter((options, res) =>
                {
                    options.Endpoint = new Uri(ServiceUrl);
                });
            });
        }

        return services;
    }
}



