using Microsoft.AspNetCore.Identity;

public class CustomPasswordHasher<TUser> : IPasswordHasher<TUser> where TUser : class
{
    private readonly ILogger<CustomPasswordHasher<TUser>> _logger;

    public CustomPasswordHasher(ILogger<CustomPasswordHasher<TUser>> logger)
    {
        _logger = logger;
    }

    public string HashPassword(TUser user, string password)
    {
        return GetMD5Hash(password);
    }

    public PasswordVerificationResult VerifyHashedPassword(TUser user, string hashedPassword, string providedPassword)
    {
        if (hashedPassword == HashPassword(user, providedPassword))
            return PasswordVerificationResult.Success;
        else
            return PasswordVerificationResult.Failed;
    }
    private string GetMD5Hash(string input)
    {
        using (var md5 = System.Security.Cryptography.MD5.Create())
        {
            var inputBytes = Encoding.UTF8.GetBytes(input);
            var hashBytes = md5.ComputeHash(inputBytes);
            return Convert.ToHexString(hashBytes);
        }
    }


}