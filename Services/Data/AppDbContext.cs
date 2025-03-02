using Microsoft.EntityFrameworkCore;
using myuzbekistan.Shared;
using ActualLab.Fusion.EntityFramework;
using ActualLab.Fusion.EntityFramework.Operations;
using EF.Audit.Core;
using EF.Audit.Core.Extensions;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Claims;
using ActualLab.Fusion;
using ActualLab.Fusion.Authentication.Services;
using ActualLab.Fusion.Extensions.Services;
using ActualLab.Fusion.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Newtonsoft.Json;

namespace myuzbekistan.Services;

public class JsonbValueConverter<T> : ValueConverter<T, string>
{
    public JsonbValueConverter()
        : base(
            v => JsonConvert.SerializeObject(v),   // �������������� �� ������� � ������ (����������)
            v => JsonConvert.DeserializeObject<T>(v) ?? default!)  // �������������� �� ������ � ������ (������)
    { }
}
public partial class AppDbContext : DbContextBase
{
    private readonly AuditDbContext _context;
    public IServiceScopeFactory _serviceScopeFactory;

    [ActivatorUtilitiesConstructor]
    public AppDbContext(DbContextOptions<AppDbContext> options, IDbContextFactory<AuditDbContext> context,
      IServiceScopeFactory serviceScopeFactory) : base(options)
    {
        _serviceScopeFactory = serviceScopeFactory;
        _context = context.CreateDbContext();
    }


    // ActualLab.Fusion.EntityFramework tables
    public DbSet<DbUser<string>> Users { get; protected set; } = null!;
    public DbSet<DbUserIdentity<string>> UserIdentities { get; protected set; } = null!;
    public DbSet<DbSessionInfo<string>> Sessions { get; protected set; } = null!;
    public DbSet<DbKeyValue> KeyValues { get; protected set; } = null!;
    public DbSet<DbOperation> Operations { get; protected set; } = null!;
    public DbSet<DbEvent> Events { get; protected set; } = null!;

    public override int SaveChanges()
    {
        AddTimestamps();
        return base.SaveChanges();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ContentEntity>(entity =>
        {
            entity.Property(e => e.PhoneNumbers)
                  .HasConversion(new JsonbValueConverter<List<CallInformation>>());
        });


        modelBuilder.Entity<ContentEntity>()
            .HasIndex(c => c.PhotoId) // ������ ��� ������������
            .IsUnique(false);

    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        AddTimestamps();

        using var scope = _serviceScopeFactory.CreateScope();
        var userContext = scope.ServiceProvider.GetService<UserContext>();
        if (userContext!.UserClaims.Count() > 1)
        {
            var identity = userContext.UserClaims.First(x => x.Type == ClaimTypes.NameIdentifier).Value;
            await _context.SaveChangesAndAuditAsync(this.ChangeTracker.Entries(), identity, cancellationToken: cancellationToken);
            return await base.SaveChangesAsync(cancellationToken);
        }
        return await base.SaveChangesAsync(cancellationToken);
    }

    private void AddTimestamps()
    {
        var entities = ChangeTracker.Entries()
            .Where(x => x.Entity is BaseEntity && (x.State == EntityState.Added || x.State == EntityState.Modified));

        foreach (var entity in entities)
        {
            var now = DateTime.UtcNow; // current datetime

            if (entity.State == EntityState.Added)
            {
                ((BaseEntity)entity.Entity).CreatedAt = now;
            }
            ((BaseEntity)entity.Entity).UpdatedAt = now;
        }
    }
}
