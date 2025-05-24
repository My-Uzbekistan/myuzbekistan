using Microsoft.EntityFrameworkCore;
using myuzbekistan.Shared;
using ActualLab.Fusion.EntityFramework;

namespace myuzbekistan.Services;
public partial class AppDbContext : DbContextBase
{
    public DbSet<CardEntity> Cards { get; protected set; } = null!;
    public DbSet<CategoryEntity> Categories { get; protected set; } = null!;
    public DbSet<ContentRequestEntity> ContentRequests { get; protected set; } = null!;
    public DbSet<ContentEntity> Contents { get; protected set; } = null!;
    public DbSet<FacilityEntity> Facilities { get; protected set; } = null!;
    public DbSet<FavoriteEntity> Favorites { get; protected set; } = null!;
    public DbSet<FileEntity> Files { get; protected set; } = null!;
    public DbSet<LanguageEntity> Languages { get; protected set; } = null!;
    public DbSet<PaymentEntity> Payments { get; protected set; } = null!;
    public DbSet<RegionEntity> Regions { get; protected set; } = null!;
    public DbSet<ReviewEntity> Reviews { get; protected set; } = null!;
    public DbSet<MerchantEntity> Merchants { get; protected set; } = null!;
}
