using Microsoft.AspNetCore.Identity;

namespace myuzbekistan.Shared;

// Add profile data for application users by adding properties to the ApplicationUser class
public class ApplicationUser : IdentityUser<long>
{
    public string? RefreshToken { get; set; }
    public DateTime? RefreshTokenExpiryTime { get; set; }
}

