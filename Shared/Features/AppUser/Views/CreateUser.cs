using Shared.Localization;
using System.Text.RegularExpressions;

namespace myuzbekistan.Shared;

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