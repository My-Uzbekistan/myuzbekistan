using ClosedXML.Excel;
using Microsoft.AspNetCore.Identity;
using myuzbekistan.Services;

namespace myuzbekistan.Services;

#region Queries
public class UserService(
    IServiceProvider services,
    IDbContextFactory<ApplicationDbContext> dbContextFactory,
    IAlertaGram alertaGram,
    IAuth auth)
    : DbServiceBase<ApplicationDbContext>(services), IUserService
{
    public async virtual Task<TableResponse<UserView>> GetAllUsers(TableOptions options, CancellationToken cancellationToken)
    {
        await Invalidate();
        await using var dbContext = await DbHub.CreateDbContext(cancellationToken);
        var count = dbContext.Users.Count();
        var items = dbContext.Users
            .Include(x => x.Roles)
            .Where(x => x.Roles.Count > 0 && x.Roles.Any(x => x.Name == "User"))
            .Paginate(options)
            .Select(x => (UserView)x)
            .ToList();
        return new TableResponse<UserView>() { Items = items, TotalItems = count };
    }

    public async virtual Task<TableResponse<ApplicationUser>> GetAll(TableOptions options, CancellationToken cancellationToken)
    {
        await Invalidate();
        await using var dbContext = await DbHub.CreateDbContext(cancellationToken);
        var count = dbContext.Users.Count();
        var items = dbContext.Users
            .Include(x => x.Roles)
            .Paginate(options).ToList();
        return new TableResponse<ApplicationUser>() { Items = items, TotalItems = count };
    }

    public virtual async Task<ApplicationUser> GetUserAsync(Session session, CancellationToken cancellationToken = default)
    {
        await Invalidate();
        await using var dbContext = await DbHub.CreateDbContext(cancellationToken);
        var userSession = await auth.GetUser(session, cancellationToken) 
            ?? throw new UnauthorizedAccessException("User session not found");
        long userId = long.Parse(userSession.Claims.First(x => x.Key.Equals(ClaimTypes.NameIdentifier)).Value);
        var user = dbContext.Users.FirstOrDefault(x => x.Id == userId);
        if (user is null)
        {
            
            await alertaGram.NotifyAsync(JsonConvert.SerializeObject(userSession.Claims), "MyUzbekistan");
            throw new NotFoundException("User not found");
        }

        return user;
    }

    public virtual async Task<ApplicationUser> GetAsync(long id, CancellationToken cancellationToken = default)
    {
        await Invalidate();
        await using var dbContext = await DbHub.CreateDbContext(cancellationToken);
        var user = dbContext.Users.FirstOrDefault(x => x.Id == id)
            ?? throw new NotFoundException("User Not Found");

        return user;
    }

    #endregion

    #region Commands
    public async virtual Task<string> UserToExcel(UserToExcelCommand command,
        CancellationToken cancellationToken = default)
    {
        command.Options.PageSize = 1000000;
        var users = await GetAll(command.Options, cancellationToken);


        using var workbook = new XLWorkbook();
        var worksheet = workbook.Worksheets.Add("users");
        var currentRow = 1;
        var headers = new[] { "Id", "Email", "CreatedAt" };

        for (int col = 2; col < headers.Length + 2; col++)
        {
            worksheet.Cell(currentRow, col).Value = headers[col - 2];
            worksheet.Cell(currentRow, col).Style.Font.Bold = true;
            worksheet.Cell(currentRow, col).Style.Fill.BackgroundColor = XLColor.LightBlue;
            worksheet.Cell(currentRow, col).Style.Font.FontColor = XLColor.Black;
            worksheet.Cell(currentRow, col).Style.Border.OutsideBorder = XLBorderStyleValues.Thick;
            worksheet.Cell(currentRow, col).Style.Border.OutsideBorderColor = XLColor.Black;
            worksheet.Cell(currentRow, col).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
        }

        foreach (var user in users.Items.ToList())
        {
            currentRow++;
            worksheet.Cell(currentRow, 2).Value = user.Id;
            worksheet.Cell(currentRow, 2).Style.Fill.BackgroundColor = XLColor.LightBlue;
            worksheet.Cell(currentRow, 2).Style.Font.FontColor = XLColor.Black;
            worksheet.Cell(currentRow, 2).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            worksheet.Cell(currentRow, 2).Style.Border.OutsideBorderColor = XLColor.Black;

            worksheet.Cell(currentRow, 3).Value = user.Email;
            worksheet.Cell(currentRow, 4).Value = user.CreatedAt;

        }

        worksheet.Columns().AdjustToContents();

        using var stream = new MemoryStream();
        workbook.SaveAs(stream);
        return Convert.ToBase64String(stream.ToArray());
    }


    public async virtual Task IncrementBalance(IncrementUserBalanceCommand userBalanceCommand, CancellationToken cancellationToken = default)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);

        
        var user =  dbContext.Users.Where(x => x.Id == userBalanceCommand.UserId).First();
        user.Balance += userBalanceCommand.Balance;

        dbContext.SaveChanges();
        using (Invalidation.Begin())
        {
            _ = await Invalidate();
        }
    }

    public async virtual Task DecrementBalance(DecrementUserBalanceCommand userBalanceCommand, CancellationToken cancellationToken = default)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);


        var user = dbContext.Users.Where(x => x.Id == userBalanceCommand.UserId).First();
        user.Balance -= userBalanceCommand.Balance;

        dbContext.SaveChanges();
        using (Invalidation.Begin())
        {
            _ = await Invalidate();
        }
    }


    public async virtual Task ChangeRole(ChangeRoleCommand roleCommand, CancellationToken cancellationToken = default)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);
        using var scope = Services.CreateScope();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

        var user = dbContext.Users.Include(x => x.Roles).AsNoTracking().First(x => x.Id == roleCommand.UserId);
        var roleId = dbContext.Roles.First(x => x.Name == roleCommand.Role).Id;

        dbContext.UserRoles.Where(x => x.UserId == user.Id).ExecuteDelete();
        var userRole = new IdentityUserRole<long>
        {
            UserId = user.Id,
            RoleId = roleId
        };
        dbContext.UserRoles.Add(userRole);
        dbContext.SaveChanges();
        using (Invalidation.Begin())
        {
            _ = await Invalidate();
        }
    }


    public async virtual Task CreateUser(CreateUserCommand userCommand, CancellationToken cancellationToken = default)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);
        using var scope = Services.CreateScope();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        var user = new ApplicationUser
        {
            UserName = userCommand.User.Login,
            Email = userCommand.User.Login,
            EmailConfirmed = true
           
        };

        var result = await userManager.CreateAsync(user, userCommand.User.Password);

        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(user, userCommand.User.Role);
        }

        using (Invalidation.Begin())
        {
            _ = await Invalidate();
        }

    }

    public async virtual Task ChangePassword(ChangePasswordCommand changePasswordCommand, CancellationToken cancellationToken = default)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);
        using var scope = Services.CreateScope();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

        var user = dbContext.Users.First(x => x.Id == changePasswordCommand.UserId);
        var token = await userManager.GeneratePasswordResetTokenAsync(user);
        await userManager.ResetPasswordAsync(user, token, changePasswordCommand.Password);

        using (Invalidation.Begin())
        {
            _ = await Invalidate();
        }

    }

    [ComputeMethod]
    public virtual Task<Unit> Invalidate() => TaskExt.UnitTask;

    public virtual async Task InvalidateUsers(InvalidateUserCommand invalidateUserCommand, CancellationToken cancellationToken = default)
    {
        using (Invalidation.Begin())
        {
            _ = await Invalidate();
        }
    }

    #endregion
}
