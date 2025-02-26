using System.Data;
using Microsoft.EntityFrameworkCore;
using ActualLab;
using ActualLab.Fusion.EntityFramework;
using ActualLab.Fusion.EntityFramework.Npgsql;
using ActualLab.Fusion.EntityFramework.Operations;
using ActualLab.Fusion.EntityFramework.Operations.LogProcessing;
using myuzbekistan.Services;

namespace Server.Infrastructure.ServiceCollection;

public static class AddDB
{
    public static IServiceCollection AddDataBase<TContext>(this IServiceCollection services, IWebHostEnvironment env,
    ConfigurationManager cfg, DataBaseType dataBaseType) where TContext : DbContext
    {
        
        services.AddTransient(_ => new DbOperationScope<TContext>.Options
        {
            IsolationLevel = IsolationLevel.RepeatableRead
        });

        services.AddDbContextServices<TContext>(ctx =>
        {
            ctx.AddOperations(operations =>
             {
                 operations.ConfigureOperationLogReader(_ => new DbOperationLogReader<TContext>.Options
                 {
                     CheckPeriod = TimeSpan.FromSeconds(env.IsDevelopment() ? 60 : 5),
                 });
                 if (dataBaseType == DataBaseType.PostgreSQL)
                 {
                     operations.AddNpgsqlOperationLogWatcher();
                 }
                 else
                 {
                     operations.AddFileSystemOperationLogWatcher();
                 }
             });

            ctx.Services.AddDbContextFactory<TContext>((c, db) =>
            {
                switch (dataBaseType)
                {
                    case DataBaseType.SQLite:
                        var dbPath = "/App.db";
                        db.UseSqlite($"Data Source={Directory.GetCurrentDirectory() + dbPath}");
                        
                        break;
                    case DataBaseType.PostgreSQL:
                        db.UseNpgsql(cfg!.GetConnectionString("Default")!, x =>
                        {
                            x.EnableRetryOnFailure(0);
                            x.UseNetTopologySuite();
                        });

                        db.UseNpgsqlHintFormatter();
                        break;
                }

                if (env.IsDevelopment()) db.EnableSensitiveDataLogging();
            });

            

        });

        services.AddDbContextServices<ApplicationDbContext>(ctx =>
        {
            ctx.Services.AddDbContextFactory<ApplicationDbContext>(options => options
         .UseNpgsql(cfg.GetValue<string>("Identity:Connection")));
        });

         

        return services;
    }
}

public enum DataBaseType
{
    PostgreSQL,
    SQLite
}