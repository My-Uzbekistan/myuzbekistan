using Microsoft.EntityFrameworkCore;
using ActualLab.Fusion;
using myuzbekistan.Shared;
using ActualLab.Fusion.EntityFramework;
using System.ComponentModel.DataAnnotations;
using ActualLab.Async;
using System.Reactive;
namespace myuzbekistan.Services;

public class FileService(IServiceProvider services) : DbServiceBase<AppDbContext>(services), IFileService 
{
    #region Queries
    [ComputeMethod]
    public async virtual Task<TableResponse<FileView>> GetAll(TableOptions options, CancellationToken cancellationToken = default)
    {
        await Invalidate();
        await using var dbContext = await DbHub.CreateDbContext(cancellationToken);
        var file = from s in dbContext.Files select s;

        if (!String.IsNullOrEmpty(options.Search))
        {
            file = file.Where(s => 
                     s.Name.Contains(options.Search)
                    || s.Extension !=null && s.Extension.Contains(options.Search)
                    || s.Path !=null && s.Path.Contains(options.Search)
            );
        }

        Sorting(ref file, options);
        
        var count = await file.AsNoTracking().CountAsync(cancellationToken: cancellationToken);
        var items = await file.AsNoTracking().Paginate(options).ToListAsync(cancellationToken: cancellationToken);
        return new TableResponse<FileView>(){ Items = items.MapToViewList(), TotalItems = count };
    }

    [ComputeMethod]
    public async virtual Task<FileView> Get(long Id, CancellationToken cancellationToken = default)
    {
        await Invalidate();
        await using var dbContext = await DbHub.CreateDbContext(cancellationToken);
        var file = await dbContext.Files
        .FirstOrDefaultAsync(x => x.Id == Id);
        
        return file == null ? throw new NotFoundException("FileEntity Not Found") : file.MapToView();
    }

    #endregion
    #region Mutations
    public async virtual Task Create(CreateFileCommand command, CancellationToken cancellationToken = default)
    {
        if (Invalidation.IsActive)
        {
            _ = await Invalidate();
            return;
        }

        await using var dbContext = await DbHub.CreateOperationDbContext(cancellationToken);
        FileEntity file=new FileEntity();
        Reattach(file, command.Entity, dbContext); 
        
        dbContext.Update(file);
        await dbContext.SaveChangesAsync(cancellationToken);

    }


    public async virtual Task Delete(DeleteFileCommand command, CancellationToken cancellationToken = default)
    {
        if (Invalidation.IsActive)
        {
            _ = await Invalidate();
            return;
        }
        await using var dbContext = await DbHub.CreateOperationDbContext(cancellationToken);
        var file = await dbContext.Files
        .FirstOrDefaultAsync(x => x.Id == command.Id);
        if (file == null) throw  new ValidationException("FileEntity Not Found");
        dbContext.Remove(file);
        await dbContext.SaveChangesAsync(cancellationToken);
    }


    public async virtual Task Update(UpdateFileCommand command, CancellationToken cancellationToken = default)
    {
        if (Invalidation.IsActive)
        {
            _ = await Invalidate();
            return;
        }
        await using var dbContext = await DbHub.CreateOperationDbContext(cancellationToken);
        var file = await dbContext.Files
        .FirstOrDefaultAsync(x => x.Id == command.Entity!.Id);

        if (file == null) throw  new ValidationException("FileEntity Not Found"); 

        Reattach(file, command.Entity, dbContext);
        
        await dbContext.SaveChangesAsync(cancellationToken);
    }
    #endregion

    

    #region Helpers

    [ComputeMethod]
    public virtual Task<Unit> Invalidate() => TaskExt.UnitTask;
    private void Reattach(FileEntity file, FileView fileView, AppDbContext dbContext)
    {
        FileMapper.From(fileView, file);



    }

    private void Sorting(ref IQueryable<FileEntity> file, TableOptions options) => file = options.SortLabel switch
    {
        "Name" => file.Ordering(options, o => o.Name),
        "FileId" => file.Ordering(options, o => o.FileId),
        "Extension" => file.Ordering(options, o => o.Extension),
        "Path" => file.Ordering(options, o => o.Path),
        "Size" => file.Ordering(options, o => o.Size),
        "Type" => file.Ordering(options, o => o.Type),
        "Id" => file.Ordering(options, o => o.Id),
        _ => file.OrderBy(o => o.Id),
        
    };
    #endregion
}
