using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using myuzbekistan.Shared;

namespace myuzbekistan.Services;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser, IdentityRole<long>, long>(options)
{

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.HasDefaultSchema("aspnet");
        builder.Entity<ApplicationUser>()
            .HasMany(x=> x.Roles) // ApplicationUser ⇄ IdentityRole<long>
            .WithMany()
            .UsingEntity<IdentityUserRole<long>>(
                join => join
                    .HasOne<IdentityRole<long>>()
                    .WithMany()
                    .HasForeignKey(r => r.RoleId),
                join => join
                    .HasOne<ApplicationUser>()
                    .WithMany()
                    .HasForeignKey(ur => ur.UserId),
                join =>
                {
                    join.ToTable("AspNetUserRoles", "aspnet"); // 👈 имя таблицы
                    join.HasKey(x => new { x.UserId, x.RoleId });
                }
            );

        var jsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true
        };

        builder.Entity<ESimPackageEntity>()
            .Property(x => x.Coverage)
            .HasColumnType("jsonb")
            .HasConversion(
                v => JsonSerializer.Serialize(v, jsonOptions),
                v => JsonSerializer.Deserialize<List<PackageResponseCoverage>>(v, jsonOptions) ?? new List<PackageResponseCoverage>()
            );
    }
}
