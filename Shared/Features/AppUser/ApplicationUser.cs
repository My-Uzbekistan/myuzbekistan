using ActualLab.Fusion.Blazor;
using MemoryPack;
using Microsoft.AspNetCore.Identity;
using System.Runtime.Serialization;

namespace myuzbekistan.Shared;

// Add profile data for application users by adding properties to the ApplicationUser class

public class ApplicationUser : IdentityUser<long>
{
    public string? RefreshToken { get; set; }
    public DateTime? RefreshTokenExpiryTime { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public string? ProfilePictureUrl { get; set; }

    public List<IdentityRole<long>> Roles { get; set; } = new List<IdentityRole<long>>();
}

[DataContract, MemoryPackable]
[ParameterComparer(typeof(ByValueParameterComparer))]
public partial class ApplicationUserView
{
    [property: DataMember] public long Id { get; set; }
    [property: DataMember] public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    [property: DataMember] public string UserName { get; set; } = null!;
    [property: DataMember] public string Email { get; set; } = null!;

}

[DataContract, MemoryPackable]
[ParameterComparer(typeof(ByValueParameterComparer))]

public partial class CreateUser
{
    [property: DataMember] public string Login { get; set; } = null!;
    [property: DataMember] public string Password { get; set; } = null!;
    [property: DataMember] public string Role { get; set; } = "User";
}