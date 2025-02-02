using System.Security.Claims;
using ActualLab.Fusion.Extensions;
using ActualLab.Rpc;
using ActualLab.Fusion.Server.Authentication;
using ActualLab.Fusion.Authentication;
using ActualLab.Fusion;
using ActualLab.Fusion.Server;
using ActualLab.Fusion.Blazor;
using ActualLab.Fusion.Blazor.Authentication;
using ActualLab.CommandR;
using myuzbekistan.Shared;
using myuzbekistan.Services;
using myuzbekistan.Server;
using ActualLab.Resilience;
using System.Diagnostics;
using ActualLab.CommandR.Diagnostics;
using OpenTelemetry.Trace;

namespace Server.Infrastructure.ServiceCollection
{
    public static class FusionServices
    {
        public static IServiceCollection AddFusionServices(this IServiceCollection services)
        {
            // Fusion services
            var fusion = services.AddFusion(RpcServiceMode.Server, true);

            var fusionServer = fusion.AddWebServer();

            fusionServer.ConfigureAuthEndpoint(_ => new()
            {
                DefaultSignInScheme = "oidc",
                DefaultSignOutScheme = "oidc",
                SignInPropertiesBuilder = (_, properties) =>
                {
                    properties.IsPersistent = true;
                }
            });
            fusionServer.ConfigureServerAuthHelper(_ => new()
            {
                NameClaimKeys = Array.Empty<string>(),
            });

            fusion.AddSandboxedKeyValueStore<AppDbContext>();
            fusion.AddOperationReprocessor();

            fusion.AddBlazor().AddAuthentication().AddPresenceReporter();

            fusion.AddDbAuthService<AppDbContext, string>();
            fusion.AddDbKeyValueStore<AppDbContext>();
            fusion.AddUtcServices();
            fusion.Commander.AddHandlers<CommandTracer>();

            var oldCore = TransiencyResolvers.CoreOnly;
            var nameList = new HashSet<string>();
            Debouncer debouncer = new Debouncer(1000); // интервал дебаунса в миллисекундах
            TransiencyResolvers.CoreOnly = e =>
            {
                string? name = e.StackTrace?.ToString().TrimStart().Substring(3, e.StackTrace.ToString().IndexOf('(') - 6);

                if (name != null)
                {
                    debouncer.Debounce(name, () =>
                    {
                        if (Activity.Current?.DisplayName == "Microsoft.AspNetCore.Hosting.HttpRequestIn")
                        {
                            Activity.Current = null;
                        }

                        using var activity = CommanderInstruments.ActivitySource.StartActivity(name);

                        activity?.SetParentId(ActivityTraceId.CreateRandom().ToString());
                        activity?.SetStatus(Status.Error);
                        activity?.RecordException(e);

                    });
                }


                return oldCore.Invoke(e);
            };

            return services;
        }
    }
    public class FusionAuthMiddleWare
    {
        private readonly RequestDelegate _next;

        public FusionAuthMiddleWare(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, ServerAuthHelper serverAuthHelper, IAuth auth, ICommander commander, UserContext userContext)
        {
            userContext.UserClaims = context.User.Claims;
            userContext.Session = serverAuthHelper.Session;
            if (context is { Request.Path.Value: { } } &&
            (context.Request.Path.Value.Contains("api") || context.Request.Path.Value.Contains("rpc")) &&
            context.User?.Identity != null &&
            context.User.Identity.IsAuthenticated)
            {
                var user = await auth.GetUser(serverAuthHelper.Session);
                if (user != null && user.Claims.First(x => x.Key.Equals(ClaimTypes.NameIdentifier)).Value != context.User.Claims.First(x => x.Type.Equals(ClaimTypes.NameIdentifier)).Value)
                {
                    await commander.Call(new Auth_SignOut(serverAuthHelper.Session));
                }
                var dups = context.User.Claims.ToList();

                foreach (var dupGroup in dups.GroupBy(x => x.Type).Where(x => x.Count() > 1).ToList())
                {
                    int i = 0;
                    foreach (var item in dupGroup)
                    {
                        if (i == 0)
                        {
                            i++;
                            continue;
                        }
                        (context.User.Identity as ClaimsIdentity)?.RemoveClaim(item);
                        i++;
                    }
                }
                await serverAuthHelper.UpdateAuthState(context);
            }

            await _next.Invoke(context!);
        }
    }
}
