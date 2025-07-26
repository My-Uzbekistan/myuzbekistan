using Microsoft.AspNetCore.Identity;

namespace myuzbekistan.Shared;

// Add profile data for application users by adding properties to the ApplicationUser class

public class ApplicationUser : IdentityUser<long>
{
    public string? RefreshToken { get; set; }
    public DateTime? RefreshTokenExpiryTime { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public string? ProfilePictureUrl { get; set; }
    public string? FullName { get; set; } 

    public decimal Balance { get; set; }

    public List<IdentityRole<long>> Roles { get; set; } = new List<IdentityRole<long>>();
}