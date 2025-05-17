using ActualLab.Fusion.Blazor;
using MemoryPack;
using Microsoft.AspNetCore.Identity;
using Shared.Localization;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;

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
    [property: DataMember, Required(ErrorMessageResourceType = typeof(SharedResource),
    ErrorMessageResourceName = "PasswordRequired")]
    [RegularExpression(@"^(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).{6,}$",
        ErrorMessageResourceType = typeof(SharedResource),
    ErrorMessageResourceName = "PasswordInvalid")]
    public string Password { get; set; } = null!;
    [property: DataMember] public string Role { get; set; } = "User";

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (Password?.Length < 6)
        {
            yield return new ValidationResult("PasswordTooShort", new[] { nameof(Password) });
        }

        if (!Regex.IsMatch(Password ?? "", @"\d"))
        {
            yield return new ValidationResult("PasswordRequiresDigit", new[] { nameof(Password) });
        }

        if (!Regex.IsMatch(Password ?? "", @"[A-Z]"))
        {
            yield return new ValidationResult("PasswordRequiresUpper", new[] { nameof(Password) });
        }

        if (!Regex.IsMatch(Password ?? "", @"[\W_]"))
        {
            yield return new ValidationResult("PasswordRequiresNonAlphanumeric", new[] { nameof(Password) });
        }
    }
}