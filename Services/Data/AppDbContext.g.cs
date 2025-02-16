using Microsoft.EntityFrameworkCore;
using myuzbekistan.Shared;
using ActualLab.Fusion.EntityFramework;

namespace myuzbekistan.Services;
public partial class AppDbContext : DbContextBase
{
    public DbSet<CategoryEntity> Categories { get; protected set; } = null!;
    public DbSet<ContentEntity> Contents { get; protected set; } = null!;
    public DbSet<FacilityEntity> Facilities { get; protected set; } = null!;
    public DbSet<FavoriteEntity> Favorites { get; protected set; } = null!;
    public DbSet<FileEntity> Files { get; protected set; } = null!;
    public DbSet<LanguageEntity> Languages { get; protected set; } = null!;
    public DbSet<ReviewEntity> Reviews { get; protected set; } = null!;
}
