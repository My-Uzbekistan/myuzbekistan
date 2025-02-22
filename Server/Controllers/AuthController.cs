using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using myuzbekistan.Shared;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Google.Apis.Auth;

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

    private async Task<GoogleJsonWebSignature.Payload?> ValidateGoogleToken(string idToken)
    {
        try
        {
            var settings = new GoogleJsonWebSignature.ValidationSettings()
            {
                Audience = new List<string>() { _configuration["Google:ClientId"]! }
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
    public async Task<IActionResult> GoogleLogin([FromBody] GoogleLoginRequest request)
    {
        var validPayload = await ValidateGoogleToken(request.IdToken);
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

    // Callback после авторизации через Google
    [HttpGet("external-login-callback")]
    public async Task<IActionResult> ExternalLoginCallback()
    {
        var authenticateResult = await HttpContext.AuthenticateAsync(IdentityConstants.ExternalScheme);

        if (!authenticateResult.Succeeded)
            return BadRequest("Ошибка аутентификации через Google");

        var email = authenticateResult.Principal.FindFirstValue(ClaimTypes.Email);
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
        var token =  GenerateJwtToken(user);

        return Ok(new { token });
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
            new(ClaimTypes.NameIdentifier, user.Id.ToString())
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
