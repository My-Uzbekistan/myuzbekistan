using Microsoft.EntityFrameworkCore;
using ActualLab.Fusion;
using myuzbekistan.Shared;
using ActualLab.Fusion.EntityFramework;
using System.ComponentModel.DataAnnotations;
using ActualLab.Async;
using System.Reactive;
using System.Globalization;
using System.Drawing;
using ClosedXML.Excel;

namespace myuzbekistan.Services;

public class FavoriteService(IServiceProvider services, IContentService contentService, IDbContextFactory<ApplicationDbContext> dbContextFactory) : DbServiceBase<AppDbContext>(services), IFavoriteService
{
    #region Queries


    public async virtual Task<TableResponse<MainPageContent>> GetFavorites(long userId, TableOptions options, CancellationToken cancellationToken = default)
    {
        await Invalidate();
        await using var dbContext = await DbHub.CreateDbContext(cancellationToken);
        var favorite = from s in dbContext.Favorites
                       where s.UserId == userId && s.ContentLocale == LangHelper.currentLocale
                       select s;

        var count = await favorite.AsNoTracking().CountAsync(cancellationToken: cancellationToken);
        var contentIds = favorite.Where(f => f.UserId == userId).Select(x => x.ContentId).Paginate(options).ToList();
        return new TableResponse<MainPageContent> { Items = await contentService.GetContentsByIds(contentIds, userId, options, cancellationToken), TotalItems = count };
    }
    [ComputeMethod]
    public async virtual Task<TableResponse<FavoriteView>> GetAll(TableOptions options, CancellationToken cancellationToken = default)
    {
        await Invalidate();
        await using var dbContext = await DbHub.CreateDbContext(cancellationToken);
        var favorite = from s in dbContext.Favorites select s;

        if (!String.IsNullOrEmpty(options.Search))
        {
            favorite = favorite.Where(s =>
                s.Id.ToString().Contains(options.Search)
            );
        }

        if (!String.IsNullOrEmpty(options.Lang))
            favorite = favorite.Where(x => x.ContentLocale.Equals(options.Lang));

        Sorting(ref favorite, options);

        favorite = favorite.Include(x => x.Content);

        var count = await favorite.AsNoTracking().CountAsync(cancellationToken: cancellationToken);
        var items = await favorite.AsNoTracking().Paginate(options).ToListAsync(cancellationToken: cancellationToken);
        var users = dbContextFactory.CreateDbContext().Users.Where(x => items.Select(x => x.UserId).Contains(x.Id)).ToList();

        items.ForEach(x => x.User = users.FirstOrDefault(u => u.Id == x.UserId));
        return new TableResponse<FavoriteView>() { Items = items.MapToViewList(), TotalItems = count };
    }

    [ComputeMethod]
    public async virtual Task<FavoriteView> Get(long Id, CancellationToken cancellationToken = default)
    {
        await Invalidate();
        await using var dbContext = await DbHub.CreateDbContext(cancellationToken);
        var favorite = await dbContext.Favorites
        .Include(x => x.Content)
        .FirstOrDefaultAsync(x => x.Id == Id);

        return favorite == null ? throw new ValidationException("FavoriteEntity Not Found") : favorite.MapToView();
    }

    #endregion
    #region Mutations

    public async virtual Task<string> FavoriteToExcel(FavoriteToExcelCommand command,
        CancellationToken cancellationToken = default)
    {
        if (Invalidation.IsActive)
        {
            _ = await Invalidate();
            return default!;
        }

        command.Options.PageSize = 1000000;
        var favorites = await GetAll(command.Options, cancellationToken);


        using var workbook = new XLWorkbook();
        var worksheet = workbook.Worksheets.Add("favorites");
        var currentRow = 1;
        var headers = new[]
    {
         "Id","Content", "Email",
    };

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

        foreach (var favorite in favorites.Items.ToList())
        {
            currentRow++;
            worksheet.Cell(currentRow, 2).Value = favorite.Id;
            worksheet.Cell(currentRow, 2).Style.Fill.BackgroundColor = XLColor.LightBlue;
            worksheet.Cell(currentRow, 2).Style.Font.FontColor = XLColor.Black;
            worksheet.Cell(currentRow, 2).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            worksheet.Cell(currentRow, 2).Style.Border.OutsideBorderColor = XLColor.Black;

            worksheet.Cell(currentRow, 3).Value = favorite.ContentView.Title;
            worksheet.Cell(currentRow, 4).Value = favorite?.User?.Email;
            
        }

        worksheet.Columns().AdjustToContents();

        using var stream = new MemoryStream();
        workbook.SaveAs(stream);
        return Convert.ToBase64String(stream.ToArray());
    }

    public async virtual Task Create(CreateFavoriteCommand command, CancellationToken cancellationToken = default)
    {
        if (Invalidation.IsActive)
        {
            _ = await Invalidate();
            return;
        }
        await using var dbContext = await DbHub.CreateOperationDbContext(cancellationToken);
        var contents = dbContext.Contents.Where(x => x.Id == command.ContentId).ToList();
        if (contents.Count > 0)
        {
            foreach (var content in contents)
            {
                FavoriteEntity favorite = new()
                {
                    Content = content,
                    UserId = command.UserId

                };

                dbContext.Add(favorite);
            }


            await dbContext.SaveChangesAsync(cancellationToken);
        }
        else
        {
            throw new NotFoundException("Content not found");
        }

    }


    public async virtual Task Delete(DeleteFavoriteCommand command, CancellationToken cancellationToken = default)
    {
        if (Invalidation.IsActive)
        {
            _ = await Invalidate();
            return;
        }
        await using var dbContext = await DbHub.CreateOperationDbContext(cancellationToken);
        var favorites = dbContext.Favorites.Where(x => x.ContentId == command.ContentId && x.UserId == command.UserId).ToList();
        if (favorites.Count == 0)
        {
            throw new ValidationException("FavoriteEntity Not Found");
        }
        dbContext.RemoveRange(favorites);
        await dbContext.SaveChangesAsync(cancellationToken);
    }


    public async virtual Task Update(UpdateFavoriteCommand command, CancellationToken cancellationToken = default)
    {
        if (Invalidation.IsActive)
        {
            _ = await Invalidate();
            return;
        }
        await using var dbContext = await DbHub.CreateOperationDbContext(cancellationToken);
        var favorite = await dbContext.Favorites
        .Include(x => x.Content)
        .FirstOrDefaultAsync(x => x.Id == command.Entity!.Id);

        if (favorite == null) throw new ValidationException("FavoriteEntity Not Found");

        Reattach(favorite, command.Entity, dbContext);

        await dbContext.SaveChangesAsync(cancellationToken);
    }
    #endregion



    #region Helpers

    [ComputeMethod]
    public virtual Task<Unit> Invalidate() => TaskExt.UnitTask;
    private void Reattach(FavoriteEntity favorite, FavoriteView favoriteView, AppDbContext dbContext)
    {
        FavoriteMapper.From(favoriteView, favorite);


        if (favorite.Content != null)
            favorite.Content = dbContext.Contents
            .First(x => x.Id == favorite.Content.Id);

    }

    private void Sorting(ref IQueryable<FavoriteEntity> favorite, TableOptions options) => favorite = options.SortLabel switch
    {
        "Content" => favorite.Ordering(options, o => o.Content),
        "Id" => favorite.Ordering(options, o => o.Id),
        _ => favorite.OrderBy(o => o.Id),

    };


    #endregion
}
