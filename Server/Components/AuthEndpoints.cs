using ActualLab.Fusion.Authentication;
using ActualLab.Fusion;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using ActualLab.CommandR;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Identity;
using Client.Pages.Auth;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Primitives;
using myuzbekistan.Shared;
using System.Security.Claims;
using System.Text.Json;

namespace Server.Components;

internal static class AuthEndpoints
{
    public static IEndpointConventionBuilder MapAdditionalIdentityEndpoints(this IEndpointRouteBuilder endpoints)
    {
        ArgumentNullException.ThrowIfNull(endpoints);

        var accountGroup = endpoints.MapGroup("/");

        
        

        var manageGroup = accountGroup.MapGroup("/Manage").RequireAuthorization();

        

        var loggerFactory = endpoints.ServiceProvider.GetRequiredService<ILoggerFactory>();
        var downloadLogger = loggerFactory.CreateLogger("DownloadPersonalData");

        manageGroup.MapPost("/DownloadPersonalData", async (
            HttpContext context,
            [FromServices] UserManager<ApplicationUser> userManager,
            [FromServices] AuthenticationStateProvider authenticationStateProvider) =>
        {
            var user = await userManager.GetUserAsync(context.User);
            if (user is null)
            {
                return Results.NotFound($"Unable to load user with ID '{userManager.GetUserId(context.User)}'.");
            }

            var userId = await userManager.GetUserIdAsync(user);
            downloadLogger.LogInformation("User with ID '{UserId}' asked for their personal data.", userId);

            // Only include personal data for download
            var personalData = new Dictionary<string, string>();
            var personalDataProps = typeof(ApplicationUser).GetProperties().Where(
                prop => Attribute.IsDefined(prop, typeof(PersonalDataAttribute)));
            foreach (var p in personalDataProps)
            {
                personalData.Add(p.Name, p.GetValue(user)?.ToString() ?? "null");
            }

            var logins = await userManager.GetLoginsAsync(user);
            foreach (var l in logins)
            {
                personalData.Add($"{l.LoginProvider} external login provider key", l.ProviderKey);
            }

            personalData.Add("Authenticator Key", (await userManager.GetAuthenticatorKeyAsync(user))!);
            var fileBytes =  System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(personalData);

            context.Response.Headers.TryAdd("Content-Disposition", "attachment; filename=PersonalData.json");
            return TypedResults.File(fileBytes, contentType: "application/json", fileDownloadName: "PersonalData.json");
        });

        accountGroup.MapGet("/Logout", async (
            ICommander command,
            Session session,
            HttpContext httpContext,
            [FromServices] SignInManager<ApplicationUser> signInManager
            ) =>
        {
            var authProps = new AuthenticationProperties
            {
                RedirectUri = "/"
            };
            await signInManager.SignOutAsync();
            await command.Call(new Auth_SignOut(session));

            await httpContext.SignOutAsync();
            await httpContext.SignOutAsync(IdentityConstants.ApplicationScheme, authProps);
        });

        accountGroup.MapGet("/Login", (
            HttpContext httpContext,
            [FromQuery] string? redirectUri
           ) =>
        {
            if (string.IsNullOrWhiteSpace(redirectUri))
            {
                redirectUri = "/";
            }
            // If user is already logged in, we can redirect directly...
            if (httpContext!.User!.Identity!.IsAuthenticated)
            {
                httpContext.Response.Redirect(redirectUri);
                return Results.Redirect(redirectUri);
            }
            return Results.Challenge(
                new AuthenticationProperties
                {
                    RedirectUri = redirectUri
                },
                [IdentityConstants.ApplicationScheme]);
        });





        return accountGroup;
    }
}
