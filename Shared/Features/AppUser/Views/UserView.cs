namespace myuzbekistan.Shared;

[DataContract, MemoryPackable]
[ParameterComparer(typeof(ByValueParameterComparer))]
public partial class UserView
{
    [property: DataMember] public long Id { get; set; }
    [property: DataMember] public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    [property: DataMember] public string UserName { get; set; } = null!;
    [property: DataMember] public string Email { get; set; } = null!;
    [property: DataMember] public string? ProfilePictureUrl { get; set; }
    [property: DataMember] public string? FullName { get; set; }
    [property: DataMember] public decimal Balance { get; set; }
    [property: DataMember] public List<EsimView> Orders { get; set; } = [];

    public override bool Equals(object? o)
    {
        var other = o as UserView;
        return other?.Id == Id;
    }

    public override int GetHashCode() => Id.GetHashCode();

    public static implicit operator UserView(ApplicationUser user)
    {
        return new UserView
        {
            Id = user.Id,
            CreatedAt = user.CreatedAt,
            UserName = user.UserName ?? string.Empty,
            Email = user.Email ?? string.Empty,
            ProfilePictureUrl = user.ProfilePictureUrl,
            FullName = user.FullName,
            Balance = user.Balance,
        };
    }
}