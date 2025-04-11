using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Server.Infrastructure.ServiceCollection
{
    public static class AuthenticationService
    {
        public static IServiceCollection AddAuth(this IServiceCollection services, IWebHostEnvironment env, ConfigurationManager configuration)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultScheme = "oidc";
                options.DefaultChallengeScheme = "oidc";
            })
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = configuration["Jwt:Issuer"],
                        ValidAudience = configuration["Jwt:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!)
                        ),
                        RoleClaimType = ClaimTypes.Role
                    };
                })
                .AddGoogle(options =>
                {
                    options.ClientId = configuration.GetValue<string>("Google:ClientId")!;
                    options.ClientSecret = configuration.GetValue<string>("Google:ClientSecret")!;
                    options.CallbackPath = "/signin-google";
                    options.SignInScheme = IdentityConstants.ExternalScheme;
                    options.ClaimActions.MapJsonKey("urn:google:profile", "link");
                    options.ClaimActions.MapJsonKey("urn:google:image", "picture");
                })
                .AddApple(options =>
                {
                    options.ClientId = configuration.GetValue<string>("Apple:ClientId")!;
                    options.KeyId = configuration.GetValue<string>("Apple:KeyId")!;
                    options.TeamId = configuration.GetValue<string>("Apple:TeamId")!;
                    options.UsePrivateKey((keyId) => env.ContentRootFileProvider.GetFileInfo(configuration.GetValue<string>("Apple:PrivateKey")!));
                    options.CallbackPath = "/signin-apple";
                    options.SignInScheme = IdentityConstants.ExternalScheme;
                    options.Scope.Add("email");
                    options.Scope.Add("name");

                    options.SaveTokens = true;
                })
                .AddPolicyScheme("oidc", "oidc", options =>
                    options.ForwardDefaultSelector = context =>
                    {
                        string? authorization = context.Request.Headers?[HeaderNames.Authorization];
                        if (string.IsNullOrEmpty(authorization) || !authorization.StartsWith("Bearer "))
                            return "Identity.Application";
                        var token = authorization["Bearer ".Length..].Trim();
                        var jwtHandler = new JwtSecurityTokenHandler();

                        return jwtHandler.CanReadToken(token) && jwtHandler.ReadJwtToken(token).Issuer
                            .Equals(configuration.GetValue<string>("Identity:Url"))
                            ? "Bearer"
                            : "Bearer";
                    }
                )

            .AddIdentityCookies();
            return services;
        }
    }
}
