using EF.Audit;
using EF.Audit.Core;
using Microsoft.EntityFrameworkCore;
using myuzbekistan.Shared;
using Newtonsoft.Json;

namespace myuzbekistan.Services;

public class AuditLogService : IAuditLogsService
{
    private readonly IDbContextFactory<AuditDbContext> _context;
    private readonly IDbContextFactory<AppDbContext> _dbcontext;

    public AuditLogService(IDbContextFactory<AuditDbContext> context, IDbContextFactory<AppDbContext> dbcontext)
    {
        _context = context;
        _dbcontext = dbcontext;
    }
    public virtual async Task<TableResponse<AuditLogView>> GetAll(TableOptions options, CancellationToken cancellationToken = default)
    {
        var dbContext = _context.CreateDbContext();
        await using var _ = dbContext.ConfigureAwait(false);
        var logs = from s in dbContext.AuditLogs select s;

        if (!string.IsNullOrEmpty(options.Search))
        {
            logs = logs.Where(s =>
                     s.TableName.ToLower().Contains(options.Search.ToLower())
                    || s.Operation.ToLower().Contains(options.Search.ToLower())
            );
        }
        if (options.From != null && options.To != null)
        {
            logs = logs.Where(s => DateOnly.FromDateTime(s.Created) > options.From && DateOnly.FromDateTime(s.Created) < options.To);
        }
        var count = await logs.CountAsync();
        var auditLogs = await logs.OrderBy(x => x.Created).Paginate(options).ToListAsync();
        var audits = await MapToViewAsync(auditLogs);
        return new TableResponse<AuditLogView>() { Items = audits, TotalItems = count };
    }

    private async Task<List<AuditLogView>> MapToViewAsync(List<AuditLog> auditLogs)
    {
        List<AuditLogView> logs = new List<AuditLogView>();

        foreach (var log in auditLogs)
        {
            logs.Add(new AuditLogView
            {
                Operation = log.Operation,
                NewValues = log.NewValues,
                OldValues = log.OldValues,
                TableName = log.TableName,
                User = await GetUserAsync(log.Identity),
                Created = log.Created
            });
        }
        return logs.ToList();
    }
    private async Task<string> GetUserAsync(string identity)
    {
        var dbContext = _dbcontext.CreateDbContext();
        await using var _ = dbContext.ConfigureAwait(false);

        var users = from s in dbContext.Users select s;
        var user = users.FirstOrDefault(u => u.ClaimsJson.Contains(identity));
        if (user == null)
        {
            return "Super Admin";
        }
        ClaimsJson userClaims = JsonConvert.DeserializeObject<ClaimsJson>(user.ClaimsJson)!;
        return userClaims.Name! ?? userClaims.Username!;
    }
    
}
