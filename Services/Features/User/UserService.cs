using ActualLab.Fusion.EntityFramework;
using myuzbekistan.Services;
using myuzbekistan.Shared;

namespace Services.Features.User
{
    public class UserService(IServiceProvider services) : DbServiceBase<ApplicationDbContext>(services), IUserService
    {
        

        public async virtual Task<List<ApplicationUser>> Get(long Id, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async virtual Task<TableResponse<ApplicationUser>> GetAll(TableOptions options, CancellationToken cancellationToken)
        {
            await using var dbContext = await DbHub.CreateDbContext(cancellationToken);
            var count = dbContext.Users.Count();
            var items = dbContext.Users.Paginate(options).ToList();
            return new TableResponse<ApplicationUser>() { Items = items, TotalItems = count };
        }

        
    }
}
