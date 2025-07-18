using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using myuzbekistan.Shared;
using System.Reflection.Emit;

namespace myuzbekistan.Services;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser, IdentityRole<long>, long>(options)
{

    protected override void OnModelCreating(ModelBuilder builder)
    {
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
        base.OnModelCreating(builder);
    }
}
