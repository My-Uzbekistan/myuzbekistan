using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using myuzbekistan.Shared;
using Microsoft.AspNetCore.Http.HttpResults;

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
        if (!IsValidPin(request.Pin)) return BadRequest(new { Message = "PIN must be 4 digits" });

        var user = await _userManager.FindByIdAsync(User.Id().ToString());
        if (user == null) return Unauthorized();
        if (!string.IsNullOrEmpty(user.Code)) return Conflict(new { Message = "PIN already exists" });

        user.Code = _passwordHasher.HashPassword(user, request.Pin);
        var res = await _userManager.UpdateAsync(user);
        if (!res.Succeeded) return StatusCode(500, new { Message = "Failed to set PIN" });
        return Created();
    }

    // POST api/pin/verify
    [HttpPost("verify")]
    public async Task<IActionResult> VerifyPin([FromBody] VerifyPinRequest request)
    {
        var user = await _userManager.FindByIdAsync(User.Id().ToString());
        if (user == null) return Unauthorized();
        if (string.IsNullOrEmpty(user.Code)) return NotFound(new { Message = "PIN not set" });

        var result = _passwordHasher.VerifyHashedPassword(user, user.Code, request.Pin);
        return result == PasswordVerificationResult.Success
            ? Ok(new { Valid = true })
            : BadRequest(new { Valid = false, Message = "Invalid PIN" });
    }

    // POST api/pin/change
    [HttpPost("change")]
    public async Task<IActionResult> ChangePin([FromBody] ChangePinRequest request)
    {
        if (!IsValidPin(request.NewPin)) return BadRequest(new { Message = "New PIN must be 4 digits" });

        var user = await _userManager.FindByIdAsync(User.Id().ToString());
        if (user == null) return Unauthorized();
        if (string.IsNullOrEmpty(user.Code)) return NotFound(new { Message = "PIN not set" });

        var verifyOld = _passwordHasher.VerifyHashedPassword(user, user.Code, request.OldPin);
        if (verifyOld != PasswordVerificationResult.Success)
            return BadRequest(new { Message = "Old PIN invalid" });

        user.Code = _passwordHasher.HashPassword(user, request.NewPin);
        var res = await _userManager.UpdateAsync(user);
        if (!res.Succeeded) return StatusCode(500, new { Message = "Failed to change PIN" });
        return NoContent();
    }

    // DELETE api/pin/remove
    [HttpDelete("remove")]
    public async Task<IActionResult> RemovePin([FromBody] RemovePinRequest request)
    {
        var user = await _userManager.FindByIdAsync(User.Id().ToString());
        if (user == null) return Unauthorized();
        if (string.IsNullOrEmpty(user.Code)) return NotFound(new { Message = "PIN not set" });

        // Optional verification before removal
        if (!string.IsNullOrWhiteSpace(request.Pin))
        {
            var verify = _passwordHasher.VerifyHashedPassword(user, user.Code, request.Pin);
            if (verify != PasswordVerificationResult.Success)
                return BadRequest(new { Message = "PIN invalid" });
        }

        user.Code = null;
        var res = await _userManager.UpdateAsync(user);
        if (!res.Succeeded) return StatusCode(500, new { Message = "Failed to remove PIN" });
        return NoContent();
    }

    private static bool IsValidPin(string? pin) => !string.IsNullOrWhiteSpace(pin) && pin.Length is >= 4 and <= 4 && pin.All(char.IsDigit);
}

public record CreatePinRequest(string Pin);
public record VerifyPinRequest(string Pin);
public record ChangePinRequest(string OldPin, string NewPin);
public record RemovePinRequest(string? Pin);
