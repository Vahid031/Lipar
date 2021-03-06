using Lipar.Core.Domain.Queries;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Lipar.Infrastructure.Data.SqlServer.Extensions;

public static class LinqExtensions
{
    public static PagedData<T> Paging<T>(this IQueryable<T> query, IPageQuery pageQuery)
    {
        var pagedData = query.PrepareObject(pageQuery);

        pagedData.Result =
        query
        .Skip((pagedData.PageNumber - 1) * pagedData.PageSize)
        .Take(pagedData.PageSize)
        .ToList();

        return pagedData;
    }

    public static async Task<PagedData<T>> PagingAsync<T>(this IQueryable<T> query, IPageQuery pageQuery)
    {
        var pagedData = query.PrepareObject(pageQuery);

        pagedData.Result =
        await query
        .Skip((pagedData.PageNumber - 1) * pagedData.PageSize)
        .Take(pagedData.PageSize)
        .ToListAsync();

        return pagedData;
    }

    private static PagedData<T> PrepareObject<T>(this IQueryable<T> query, IPageQuery pageQuery)
    {
        var pagedData = new PagedData<T>();

        if (pageQuery.PageNumber > 0)
            pagedData.PageNumber = pageQuery.PageNumber;

        if (pageQuery.PageSize > 0)
            pagedData.PageSize = pageQuery.PageSize;

        pagedData.TotalCount = (pageQuery.NeedTotalCount) ? query.Count() : 0;


        if (!string.IsNullOrEmpty(pageQuery.SortBy))
            if (pageQuery.SortAscending)
                query = query.OrderBy(m => EF.Property<T>(m, pageQuery.SortBy));
            else
                query = query.OrderByDescending(m => EF.Property<T>(m, pageQuery.SortBy));

        return pagedData;
    }
}


