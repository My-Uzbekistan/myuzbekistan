using ActualLab.Fusion.EntityFramework;
using Microsoft.EntityFrameworkCore;
using myuzbekistan.Shared;

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
    public DbSet<MerchantCategoryEntity> MerchantCategories { get; protected set; } = null!;
    public DbSet<MerchantEntity> Merchants { get; protected set; } = null!;
    public DbSet<ServiceTypeEntity> ServiceTypes { get; protected set; } = null!;
    public DbSet<InvoiceEntity> Invoices { get; protected set; } = null!;
    public DbSet<SimCountryEntity> SimCountries { get; protected set; } = null!;
    public DbSet<CardPrefixEntity> CardPrefixes { get; protected set; } = null!;
    public DbSet<CardColorEntity> CardColors { get; protected set; } = null!;
    public DbSet<ESimPackageEntity> ESimPackages { get; protected set; } = null!;
    public DbSet<ESimOrderEntity> ESimOrders { get; protected set; } = null!;
    public DbSet<PackageDiscountEntity> PackageDiscounts { get; protected set; } = null!;
    public DbSet<ESimSlugEntity> ESimSlugs { get; protected set; } = null!;
    public DbSet<DeviceEntity> Devices { get; protected set; } = null!;
    public DbSet<SmsTemplateEntity> SmsTemplates { get; protected set; } = null!;
}
