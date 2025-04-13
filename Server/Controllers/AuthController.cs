using AspNet.Security.OAuth.Apple;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using myuzbekistan.Shared;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Server.Controllers;

[Route("api/auth")]
[AllowAnonymous]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IConfiguration _configuration;
    private readonly ILogger<AuthController> _logger;

    public AuthController(UserManager<ApplicationUser> userManager, IConfiguration configuration, ILogger<AuthController> logger)
    {
        _userManager = userManager;
        _configuration = configuration;
        _logger = logger;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginModel model)
    {
        var user = await _userManager.FindByNameAsync(model.Username);

        if (user != null)
        {
            bool isPasswordValid = await _userManager.CheckPasswordAsync(user, model.Password);

            if (isPasswordValid)
            {
                var roles = await _userManager.GetRolesAsync(user);

                var accessToken = GenerateJwtToken(user, roles);

                var refreshToken = GenerateRefreshToken();
                // Сохраняем refresh-токен в БД
                user.RefreshToken = refreshToken;
                user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);

                try
                {
                    return Ok(new
                    {
                        access_token = accessToken,
                        refresh_token = refreshToken,
                        expires = DateTime.Now.AddMinutes(30).Millisecond
                    });
                }
                finally
                {
                    await _userManager.UpdateAsync(user);
                }

            }
        }

        _logger.LogWarning("Login failed for user: {Username}", model.Username);
        return Unauthorized();
    }

    // Старт авторизации через Google
    [HttpGet("login-google")]
    public IActionResult LoginGoogle()
    {
        var redirectUrl = Url.Action(nameof(ExternalLoginCallback));
        var properties = new AuthenticationProperties { RedirectUri = redirectUrl };
        return Challenge(properties, GoogleDefaults.AuthenticationScheme);
    }

    [HttpGet("login-apple")]
    public IActionResult LoginApple()
    {
        var redirectUrl = Url.Action(nameof(ExternalLoginCallback));
        var properties = new AuthenticationProperties { RedirectUri = redirectUrl };
        return Challenge(properties, AppleAuthenticationDefaults.AuthenticationScheme);
    }

    private async Task<GoogleJsonWebSignature.Payload?> ValidateGoogleToken(string idToken, string platform)
    {
        try
        {
            var settings = new GoogleJsonWebSignature.ValidationSettings()
            {
                //Audience = [
                //    platform == "ios" ? _configuration["Google:IosClientId"]! : 
                //    platform == "android" ? _configuration["Google:AndroidClientId"]! : 
                //    _configuration["Google:ClientId"]!
                //]
            };

            var payload = await GoogleJsonWebSignature.ValidateAsync(idToken, settings);
            return payload;
        }
        catch (InvalidJwtException)
        {
            return null;
        }
    }

    public class GoogleLoginRequest
    {
        public string IdToken { get; set; }
    }
    [HttpPost("google-login")]
    public async Task<IActionResult> GoogleLogin([FromBody] GoogleLoginRequest request, [FromQuery] string platform)
    {
        var validPayload = await ValidateGoogleToken(request.IdToken, platform);
        if (validPayload == null)
        {
            return BadRequest("Invalid Google token.");
        }

        var email = validPayload.Email;
        var user = await _userManager.FindByEmailAsync(email);

        if (user == null)
        {
            user = new ApplicationUser
            {
                UserName = email,
                Email = email,
                ProfilePictureUrl = validPayload.Picture
            };

            var result = await _userManager.CreateAsync(user);
            if (!result.Succeeded)
                return BadRequest(result.Errors);

            await _userManager.AddToRoleAsync(user, "User");
        }

        var roles = await _userManager.GetRolesAsync(user);
        var accessToken = GenerateJwtToken(user, roles);

        var refreshToken = GenerateRefreshToken();
        // Сохраняем refresh-токен в БД
        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);

        try
        {
            return Ok(new
            {
                access_token = accessToken,
                refresh_token = refreshToken,
                expires = DateTime.Now.AddMinutes(30).Millisecond
            });
        }
        finally
        {
            await _userManager.UpdateAsync(user);
        }

    }


   

    public async Task<ClaimsPrincipal?> ValidateAppleTokenAsync(string idToken)
    {
        var handler = new JwtSecurityTokenHandler();
        var jwt = handler.ReadJwtToken(idToken);

        var key = new AppleKey
        {
            Kty = "RSA",
            Kid = "dMlERBaFdK",
            Use = "sig",
            Alg = "RS256",
            N = "ryLWkB74N6SJNRVBjKF6xKMfP-QW3AAsJotv0LjVtf7m4NZNg_gTL78e7O8wmvngF8FuzBrvqf1mGW17Ct8BgNK6YXxnoGL0YLmlwXbmCZvTXki0VlEW1PDXeViWy7qXaCp2caF5v4OOdPsgroxNO_DgJRTuA_izJ4DFZYHCHXwojfdWJiDYG67j5PlD5pXKGx7zaqyryjovZTEII_Z1_bhFCRUZRjfJ3TVoK0fZj2z7iAZWjn33i-V3zExUhwzEyeuGph0118NfmOLCUEy_Jd4xvLf_X4laPpe9nq8UeORfs72yz2qH7cHDKL85W6oG08Gu05JWuAs5Ay49WxJrmw",
            E = "AQAB"
        };


        var rsa = new RsaSecurityKey(new RSAParameters
        {
            Modulus = Base64UrlEncoder.DecodeBytes(key.N),
            Exponent = Base64UrlEncoder.DecodeBytes(key.E)
        });

        var parameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = "https://appleid.apple.com",

            ValidateAudience = false,
            //ValidAudience = "uz.travel.my.uzbid",

            ValidateIssuerSigningKey = true,
            IssuerSigningKey = rsa,

            ValidateLifetime = true
        };

        try
        {
            var principal = handler.ValidateToken(idToken, parameters, out var _);
            return principal;
        }
        catch
        {
            return null;
        }
    }

    [HttpPost("apple-login")]
    public async Task<IActionResult> AppleLogin([FromBody] GoogleLoginRequest request, [FromQuery] string platform)
    {

        var validPayload = await ValidateAppleTokenAsync(request.IdToken);
            
        if (validPayload == null)
        {
            return BadRequest("Invalid apple token.");
        }

        var email = validPayload.Claims.First(x=>x.Type == ClaimTypes.Email).Value;
        var user = await _userManager.FindByEmailAsync(email);

        if (user == null)
        {
            user = new ApplicationUser
            {
                UserName = email,
                Email = email,
                //ProfilePictureUrl = validPayload.Picture
            };

            var result = await _userManager.CreateAsync(user);
            if (!result.Succeeded)
                return BadRequest(result.Errors);

            await _userManager.AddToRoleAsync(user, "User");
        }

        var roles = await _userManager.GetRolesAsync(user);
        var accessToken = GenerateJwtToken(user, roles);

        var refreshToken = GenerateRefreshToken();
        // Сохраняем refresh-токен в БД
        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);

        try
        {
            return Ok(new
            {
                access_token = accessToken,
                refresh_token = refreshToken,
                expires = DateTime.Now.AddMinutes(30).Millisecond
            });
        }
        finally
        {
            await _userManager.UpdateAsync(user);
        }

    }

    // Callback после авторизации через Google
    [HttpGet("external-login-callback")]
    public async Task<IActionResult> ExternalLoginCallback()
    {
        var authenticateResult = await HttpContext.AuthenticateAsync(IdentityConstants.ExternalScheme);

        if (!authenticateResult.Succeeded)
            return BadRequest("Ошибка аутентификации через Google");

        var email = authenticateResult.Principal.FindFirstValue(ClaimTypes.Email);
        var idToken = authenticateResult.Properties?.GetTokenValue("id_token");
        var user = await _userManager.FindByEmailAsync(email);

        if (user == null)
        {
            user = new ApplicationUser
            {
                UserName = email,
                Email = email,
                ProfilePictureUrl = authenticateResult.Principal.FindFirstValue("urn:google:image")
            };

            var result = await _userManager.CreateAsync(user);
            if (!result.Succeeded)
                return BadRequest(result.Errors);

            // Привязываем внешний логин (Google) к пользователю
            var loginInfo = new UserLoginInfo(
                authenticateResult.Properties.Items[".AuthScheme"]!,
                authenticateResult.Principal.FindFirstValue(ClaimTypes.NameIdentifier),
                "Google"
            );

            var addLoginResult = await _userManager.AddLoginAsync(user, loginInfo);
            if (!addLoginResult.Succeeded)
                return BadRequest(addLoginResult.Errors);

            // ✅ Добавляем роль "User" после создания
            var roleResult = await _userManager.AddToRoleAsync(user, "User");
            if (!roleResult.Succeeded)
                return BadRequest(roleResult.Errors);
        }

        // Генерация JWT с ролями
        var accessToken =  GenerateJwtToken(user);

        var refreshToken = GenerateRefreshToken();
        // Сохраняем refresh-токен в БД
        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);

        try
        {
            return Ok(new
            {
                access_token = accessToken,
                refresh_token = refreshToken,
                expires = DateTime.Now.AddMinutes(30).Millisecond
            });
        }
        finally
        {
            await _userManager.UpdateAsync(user);
        }
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
    {
        var user = await _userManager.Users.FirstOrDefaultAsync(u => u.RefreshToken == request.RefreshToken);

        if (user == null || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
        {
            _logger.LogWarning("Invalid refresh token for user.");
            return Unauthorized(new { message = "Invalid refresh token" });
        }

        var roles = await _userManager.GetRolesAsync(user);

        var newAccessToken = GenerateJwtToken(user, roles);

        var newRefreshToken = GenerateRefreshToken();
        user.RefreshToken = newRefreshToken;
        user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);

        await _userManager.UpdateAsync(user);

        return Ok(new
        {
            access_token = newAccessToken,
            refresh_token = newRefreshToken,
            expires = DateTime.Now.AddMinutes(30).Millisecond
        });
    }

    private string GenerateJwtToken(ApplicationUser user, IList<string>? roles = null)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Name, user.UserName!),
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new("userName", user.UserName!),
            new("photoUrl", user.ProfilePictureUrl ?? string.Empty)
        };
        if(roles != null)
            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));
        else
        {
            claims.Add(new Claim(ClaimTypes.Role, "User"));
        }

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(30),
            signingCredentials: credentials
        );


        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private string GenerateRefreshToken()
    {
        var refreshToken = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));

        return refreshToken;
    }
}

public class LoginModel
{
    public string Username { get; set; }
    public string Password { get; set; }
}

public class RefreshTokenRequest
{
    public string RefreshToken { get; set; }
}
