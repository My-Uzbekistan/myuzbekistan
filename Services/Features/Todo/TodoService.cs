using Microsoft.EntityFrameworkCore;
using ActualLab.Fusion;
using myuzbekistan.Shared;
using ActualLab.Fusion.EntityFramework;
using System.ComponentModel.DataAnnotations;
using ActualLab.Async;
using System.Reactive;
namespace myuzbekistan.Services;

public class TodoService(IServiceProvider services) : DbServiceBase<AppDbContext>(services), ITodoService 
{
    #region Queries
    [ComputeMethod]
    public async virtual Task<TableResponse<TodoView>> GetAll(TableOptions options, CancellationToken cancellationToken = default)
    {
        await Invalidate();
        await using var dbContext = await DbHub.CreateDbContext(cancellationToken);
        var todo = from s in dbContext.Todos select s;

        if (!String.IsNullOrEmpty(options.Search))
        {
            todo = todo.Where(s => 
                     s.Name.Contains(options.Search)
            );
        }

        Sorting(ref todo, options);
        
        var count = await todo.AsNoTracking().CountAsync(cancellationToken: cancellationToken);
        var items = await todo.AsNoTracking().Paginate(options).ToListAsync(cancellationToken: cancellationToken);
        return new TableResponse<TodoView>(){ Items = items.MapToViewList(), TotalItems = count };
    }

    [ComputeMethod]
    public async virtual Task<TodoView> Get(long Id, CancellationToken cancellationToken = default)
    {
        await Invalidate();
        await using var dbContext = await DbHub.CreateDbContext(cancellationToken);
        var todo = await dbContext.Todos
        .FirstOrDefaultAsync(x => x.Id == Id);
        
        return todo == null ? throw new ValidationException("TodoEntity Not Found") : todo.MapToView();
    }

    #endregion
    #region Mutations
    public async virtual Task Create(CreateTodoCommand command, CancellationToken cancellationToken = default)
    {
        if (Invalidation.IsActive)
        {
            _ = await Invalidate();
            return;
        }

        await using var dbContext = await DbHub.CreateOperationDbContext(cancellationToken);
        TodoEntity todo=new TodoEntity();
        Reattach(todo, command.Entity, dbContext); 
        
        dbContext.Update(todo);
        await dbContext.SaveChangesAsync(cancellationToken);

    }


    public async virtual Task Delete(DeleteTodoCommand command, CancellationToken cancellationToken = default)
    {
        if (Invalidation.IsActive)
        {
            _ = await Invalidate();
            return;
        }
        await using var dbContext = await DbHub.CreateOperationDbContext(cancellationToken);
        var todo = await dbContext.Todos
        .FirstOrDefaultAsync(x => x.Id == command.Id);
        if (todo == null) throw  new ValidationException("TodoEntity Not Found");
        dbContext.Remove(todo);
        await dbContext.SaveChangesAsync(cancellationToken);
    }


    public async virtual Task Update(UpdateTodoCommand command, CancellationToken cancellationToken = default)
    {
        if (Invalidation.IsActive)
        {
            _ = await Invalidate();
            return;
        }
        await using var dbContext = await DbHub.CreateOperationDbContext(cancellationToken);
        var todo = await dbContext.Todos
        .FirstOrDefaultAsync(x => x.Id == command.Entity!.Id);

        if (todo == null) throw  new ValidationException("TodoEntity Not Found"); 

        Reattach(todo, command.Entity, dbContext);
        
        await dbContext.SaveChangesAsync(cancellationToken);
    }
    #endregion

    

    #region Helpers

    [ComputeMethod]
    public virtual Task<Unit> Invalidate() => TaskExt.UnitTask;
    private void Reattach(TodoEntity todo, TodoView todoView, AppDbContext dbContext)
    {
        TodoMapper.From(todoView, todo);



    }

    private void Sorting(ref IQueryable<TodoEntity> todo, TableOptions options) => todo = options.SortLabel switch
    {
        "Name" => todo.Ordering(options, o => o.Name),
        "ImageId" => todo.Ordering(options, o => o.ImageId),
        "Id" => todo.Ordering(options, o => o.Id),
        _ => todo.OrderBy(o => o.Id),
        
    };
    #endregion
}
