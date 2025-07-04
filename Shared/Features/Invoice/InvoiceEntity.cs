﻿namespace myuzbekistan.Shared;
[SkipGeneration]
public class InvoiceEntity : BaseEntity
{
    public decimal Amount { get; set; } = 0.0m;
    public string? Currency { get; set; } = null!;
    public string? Description { get; set; } = null!;
    public long UserId { get; set; }

    [NotMapped]
    public ApplicationUser? User { get; set; } = null!;
    public MerchantEntity Merchant { get; set; } = null!;
}
