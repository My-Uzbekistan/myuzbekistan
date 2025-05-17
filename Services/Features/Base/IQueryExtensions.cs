using myuzbekistan.Shared;
using System.Linq.Expressions;

namespace myuzbekistan.Services;

internal static class QueryExtensions
{
    /// <summary>
    /// Paginate 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="query"></param>
    /// <param name="options"></param>
    /// <returns></returns>
    public static IQueryable<T> Paginate<T>(this IQueryable<T> query, TableOptions options)
    {
        return query.Skip((options.Page > 0 ? options.Page - 1 : 0) * options.PageSize).Take(options.PageSize);
    }

    /// <summary>
    /// Ordering entity if SortDirection equal 0 to OrderBy column CreatedAt
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="query"></param>
    /// <param name="options"></param>
    /// <param name="expression"></param>
    /// <returns></returns>
    public static IQueryable<T> Ordering<T, TK>(this IQueryable<T> query, TableOptions options, Expression<Func<T, TK>> expression) where T : BaseEntity
    {

        query = options.SortDirection switch
        {
            1 => query.OrderByDescending(expression),
            2 => query.OrderBy(expression),
            _ => query.OrderBy((x) => x.CreatedAt)
        };

        return query;
    }

    /// <summary>
    /// Search if options.Search not null
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="query"></param>
    /// <param name="options"></param>
    /// <param name="expression"></param>
    /// <returns></returns>
    public static IQueryable<T> Search<T>(this IQueryable<T> query, TableOptions options, Expression<Func<T, bool>> expression) where T : BaseEntity
    {
        if (!String.IsNullOrEmpty(options.Search))
        {
            query = query.Where(expression);
        }
        return query;
    }


}
