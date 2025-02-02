using Microsoft.EntityFrameworkCore;
using myuzbekistan.Shared;
using ActualLab.Fusion.EntityFramework;

namespace myuzbekistan.Services;
public partial class AppDbContext : DbContextBase
{
    public DbSet<TodoEntity> Todos { get; protected set; } = null!;
}
