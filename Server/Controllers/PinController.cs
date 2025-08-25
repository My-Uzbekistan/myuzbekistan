using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using myuzbekistan.Shared;

namespace Server.Controllers;

[Route("api/pin")]
[ApiController]
[Authorize]
public class PinController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IPasswordHasher<ApplicationUser> _passwordHasher;

    public PinController(UserManager<ApplicationUser> userManager, IPasswordHasher<ApplicationUser> passwordHasher)
    {
        _userManager = userManager;
        _passwordHasher = passwordHasher;
    }

    // POST api/pin/create
    [HttpPost("create")]
    public async Task<IActionResult> CreatePin([FromBody] CreatePinRequest request)
    {
        if (!IsValidPin(request.Pin))
            throw new MyUzException("New PIN must be 4 digits");

        var user = await _userManager.FindByIdAsync(User.Id().ToString())
                   ?? throw new MyUzException("Unauthorized");

        if (!string.IsNullOrEmpty(user.Code))
            throw new MyUzException("PIN already exists");

        user.Code = _passwordHasher.HashPassword(user, request.Pin);
        var res = await _userManager.UpdateAsync(user);
        if (!res.Succeeded)
            throw new MyUzException("Failed to set PIN");

        return Created(string.Empty, null);
    }

    // POST api/pin/verify
    [HttpPost("verify")]
    public async Task<IActionResult> VerifyPin([FromBody] VerifyPinRequest request)
    {
        var user = await _userManager.FindByIdAsync(User.Id().ToString())
                   ?? throw new MyUzException("Unauthorized");

        if (string.IsNullOrEmpty(user.Code))
            throw new MyUzException("PIN not set");

        var result = _passwordHasher.VerifyHashedPassword(user, user.Code, request.Pin);
        if (result != PasswordVerificationResult.Success)
            throw new MyUzException("Invalid PIN");

        return Ok(new { Valid = true });
    }

    // POST api/pin/change
    [HttpPost("change")]
    public async Task<IActionResult> ChangePin([FromBody] ChangePinRequest request)
    {
        if (!IsValidPin(request.NewPin))
            throw new MyUzException("New PIN must be 4 digits");

        var user = await _userManager.FindByIdAsync(User.Id().ToString())
                   ?? throw new MyUzException("Unauthorized");

        if (string.IsNullOrEmpty(user.Code))
            throw new MyUzException("PIN not set");

        var verifyOld = _passwordHasher.VerifyHashedPassword(user, user.Code, request.OldPin);
        if (verifyOld != PasswordVerificationResult.Success)
            throw new MyUzException("Old PIN invalid");

        user.Code = _passwordHasher.HashPassword(user, request.NewPin);
        var res = await _userManager.UpdateAsync(user);
        if (!res.Succeeded)
            throw new MyUzException("Failed to change PIN");

        return NoContent();
    }

    // DELETE api/pin/remove
    [HttpDelete("remove")]
    public async Task<IActionResult> RemovePin([FromBody] RemovePinRequest request)
    {
        var user = await _userManager.FindByIdAsync(User.Id().ToString())
                   ?? throw new MyUzException("Unauthorized");

        if (string.IsNullOrEmpty(user.Code))
            throw new MyUzException("PIN not set");

        if (!string.IsNullOrWhiteSpace(request.Pin))
        {
            var verify = _passwordHasher.VerifyHashedPassword(user, user.Code, request.Pin);
            if (verify != PasswordVerificationResult.Success)
                throw new MyUzException("PIN invalid");
        }

        user.Code = null;
        var res = await _userManager.UpdateAsync(user);
        if (!res.Succeeded)
            throw new MyUzException("Failed to remove PIN");

        return NoContent();
    }

    private static bool IsValidPin(string? pin) => !string.IsNullOrWhiteSpace(pin) && pin.Length == 4 && pin.All(char.IsDigit);
}

public record CreatePinRequest(string Pin);
public record VerifyPinRequest(string Pin);
public record ChangePinRequest(string OldPin, string NewPin);
public record RemovePinRequest(string? Pin);
