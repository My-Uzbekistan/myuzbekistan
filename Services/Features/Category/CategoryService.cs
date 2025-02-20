using Microsoft.EntityFrameworkCore;
using ActualLab.Fusion;
using myuzbekistan.Shared;
using ActualLab.Fusion.EntityFramework;
using System.ComponentModel.DataAnnotations;
using ActualLab.Async;
using System.Reactive;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Collections;
namespace myuzbekistan.Services;

public class ContentEntityConverter : JsonConverter<ContentEntity>
{
    public override ContentEntity? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        // Используем стандартную десериализацию
        return JsonSerializer.Deserialize<ContentEntity>(ref reader, options);
    }

    public override void Write(Utf8JsonWriter writer, ContentEntity value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        var properties = value.GetType().GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);

        foreach (var property in properties)
        {
            var propValue = property.GetValue(value);
            if (propValue == null) continue;  // Пропускаем null

            if (propValue is IEnumerable enumerable && !(propValue is string))
            {
                var hasElements = enumerable.GetEnumerator().MoveNext();
                if (!hasElements) continue;  // Пропускаем пустые коллекции
            }

            // Преобразуем имя свойства в camelCase
            var namingPolicyValue = options.PropertyNamingPolicy?.ConvertName(property.Name) ?? property.Name;

            writer.WritePropertyName(namingPolicyValue);
            JsonSerializer.Serialize(writer, propValue, options);
        }

        writer.WriteEndObject();
    }
}
public class CustomMainPageApiConverter : JsonConverter<MainPageApi>
{
    public override MainPageApi? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        // Используем стандартную десериализацию
        return JsonSerializer.Deserialize<MainPageApi>(ref reader, options);
    }

    public override void Write(Utf8JsonWriter writer, MainPageApi value, JsonSerializerOptions options)
    {
        // Получаем свойства объекта
        var properties = value.GetType().GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);

        writer.WriteStartObject();

        foreach (var property in properties)
        {
            var propValue = property.GetValue(value);
            if (propValue == null) continue;  // Пропускаем null

            if (propValue is IEnumerable enumerable && !(propValue is string))
            {
                // Проверяем, пуст ли массив
                var hasElements = enumerable.GetEnumerator().MoveNext();
                if (!hasElements) continue;  // Пропускаем пустые коллекции
            }

            // Преобразуем имя свойства в camelCase
            var namingPolicyValue = options.PropertyNamingPolicy?.ConvertName(property.Name) ?? property.Name;

            // Сериализуем оставшееся свойство
            writer.WritePropertyName(namingPolicyValue);
            JsonSerializer.Serialize(writer, propValue, options);
        }

        writer.WriteEndObject();
    }
}

public class CategoryService(IServiceProvider services) : DbServiceBase<AppDbContext>(services), ICategoryService
{
    #region Queries
    //[ComputeMethod]
    public async Task<List<MainPageApi>> GetMainPageApi(CancellationToken cancellationToken = default)
    {
        await Invalidate();
        await using var dbContext = await DbHub.CreateDbContext(cancellationToken);
        var category = from s in dbContext.Categories.Include(x => x.Icon)
                       where s.Status == ContentStatus.Active && s.Locale == CultureInfo.CurrentCulture.TwoLetterISOLanguageName
                       join c in dbContext.Contents on new { s.Id, s.Locale } equals new { Id = c.CategoryId, c.Locale }
                       select new MainPageApi(s.Name, s.Id, s.Contents != null ? s.Contents.Where(x=>x.Locale == CultureInfo.CurrentCulture.TwoLetterISOLanguageName).ToList() : new List<ContentEntity>() { });
        return [.. category];
    }
    //[ComputeMethod]
    public async virtual Task<List<CategoryApi>> GetCategories(CancellationToken cancellationToken = default)
    {
        await Invalidate();
        await using var dbContext = await DbHub.CreateDbContext(cancellationToken);
        var category = from s in dbContext.Categories.Include(x => x.Icon)
                       where s.Status == ContentStatus.Active && s.Locale == CultureInfo.CurrentCulture.TwoLetterISOLanguageName
                       select new CategoryApi(s.Name, s.Icon!.Path!, s.Id);
        return [.. category];
    }

    [ComputeMethod]
    public async virtual Task<TableResponse<CategoryView>> GetAll(TableOptions options, CancellationToken cancellationToken = default)
    {
        await Invalidate();
        await using var dbContext = await DbHub.CreateDbContext(cancellationToken);
        var category = from s in dbContext.Categories select s;

        if (!String.IsNullOrEmpty(options.Search))
        {
            category = category.Where(s =>
                     s.Name.Contains(options.Search)
                    || s.Description.Contains(options.Search)
            );
        }

        #region Search by Language

        if (!String.IsNullOrEmpty(options.Lang))
            category = category.Where(x => x.Locale.Equals(options.Lang));

        #endregion

        Sorting(ref category, options);

        category = category.Include(x => x.Contents).Include(x=>x.Icon);
        var count = await category.AsNoTracking().CountAsync(cancellationToken: cancellationToken);
        var items = await category.AsNoTracking().Paginate(options).ToListAsync(cancellationToken: cancellationToken);
        return new TableResponse<CategoryView>() { Items = items.MapToViewList(), TotalItems = count };
    }

    [ComputeMethod]
    public async virtual Task<List<CategoryView>> Get(long Id, CancellationToken cancellationToken = default)
    {
        await Invalidate();
        await using var dbContext = await DbHub.CreateDbContext(cancellationToken);
        var category = dbContext.Categories
        .Include(x => x.Contents)
        .Where(x => x.Id == Id).ToList();

        return category == null ? throw new ValidationException("CategoryEntity Not Found") : category.MapToViewList();
    }

    #endregion
    #region Mutations
    long maxId;
    public async virtual Task Create(CreateCategoryCommand command, CancellationToken cancellationToken = default)
    {
        if (Invalidation.IsActive)
        {
            _ = await Invalidate();
            return;
        }

        await using var dbContext = await DbHub.CreateOperationDbContext(cancellationToken);
        maxId = dbContext.Categories.Count() == 0 ? 0 : dbContext.Categories.Max(x => x.Id);
        maxId++;
        foreach (var item in command.Entity)
        {
            CategoryEntity category = new CategoryEntity();
            Reattach(category, item, dbContext);
            category.Id = maxId;
            dbContext.Add(category);

        }

        await dbContext.SaveChangesAsync(cancellationToken);

    }


    public async virtual Task Delete(DeleteCategoryCommand command, CancellationToken cancellationToken = default)
    {
        if (Invalidation.IsActive)
        {
            _ = await Invalidate();
            return;
        }
        await using var dbContext = await DbHub.CreateOperationDbContext(cancellationToken);
        var category = await dbContext.Categories
        .Include(x => x.Contents)
        .FirstOrDefaultAsync(x => x.Id == command.Id);
        if (category == null) throw new ValidationException("CategoryEntity Not Found");
        dbContext.Remove(category);
        await dbContext.SaveChangesAsync(cancellationToken);
    }


    public async virtual Task Update(UpdateCategoryCommand command, CancellationToken cancellationToken = default)
    {
        var cat = command.Entity.First();
        if (Invalidation.IsActive)
        {
            _ = await Invalidate();
            return;
        }
        await using var dbContext = await DbHub.CreateOperationDbContext(cancellationToken);
        var category = dbContext.Categories
        .Include(x => x.Contents)
        .Include(x=>x.Icon)
        .Where(x => x.Id == cat.Id).ToList();

        if (category == null) throw new ValidationException("CategoryEntity Not Found");

        foreach (var item in command.Entity)
        {
            Reattach(category.First(x => x.Locale == item.Locale), item, dbContext);
            dbContext.Update(category.First(x => x.Locale == item.Locale));
        }



        await dbContext.SaveChangesAsync(cancellationToken);
    }
    #endregion



    #region Helpers

    [ComputeMethod]
    public virtual Task<Unit> Invalidate() => TaskExt.UnitTask;
    private void Reattach(CategoryEntity category, CategoryView categoryView, AppDbContext dbContext)
    {
        CategoryMapper.From(categoryView, category);


        if (category.Contents != null)
            category.Contents = dbContext.Contents
            .Where(x => category.Contents.Select(tt => tt.Id).ToList().Contains(x.Id)).ToList();

        if (category.Icon != null)
            category.Icon = dbContext.Files
            .Where(x => x.Name == category.Icon.Name).First();

    }

    private void Sorting(ref IQueryable<CategoryEntity> category, TableOptions options) => category = options.SortLabel switch
    {
        "Name" => category.Ordering(options, o => o.Name),
        "Description" => category.Ordering(options, o => o.Description),
        "Contents" => category.Ordering(options, o => o.Contents),
        "Id" => category.Ordering(options, o => o.Id),
        _ => category.OrderBy(o => o.Id),

    };

  
    #endregion
}
