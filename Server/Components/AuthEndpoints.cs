using ActualLab.Fusion.Authentication;
using ActualLab.Fusion;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using ActualLab.CommandR;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Identity;

namespace Server.Components;

internal static class AuthEndpoints
{
    public static IEndpointConventionBuilder MapAdditionalIdentityEndpoints(this IEndpointRouteBuilder endpoints)
    {
        ArgumentNullException.ThrowIfNull(endpoints);

        var accountGroup = endpoints.MapGroup("/");

        accountGroup.MapGet("/Logout", async (
            ICommander command,
            Session session,
            HttpContext httpContext) =>
        {
            var authProps = new AuthenticationProperties
            {
                RedirectUri = "/"
            };
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
