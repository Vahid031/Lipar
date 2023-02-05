using Lipar.Core.Domain.Queries;
using MongoDB.Driver;
using System.Linq;
using System.Threading.Tasks;

namespace Lipar.Infrastructure.Data.Mongo.Extensions;

public static class LinqExtensions
{
    public static PagedData<TProjection> Paging<TDocument, TProjection>(this IFindFluent<TDocument, TProjection> query, IPageQuery pageQuery)
    {
        var pagedData = query.PrepareObject(pageQuery);

        pagedData.Result =
            query
            .Skip((pagedData.PageNumber - 1) * pagedData.PageSize)
            .Limit(pagedData.PageSize)
            .ToList();

        return pagedData;
    }

    public static async Task<PagedData<TProjection>> PagingAsync<TDocument, TProjection>(this IFindFluent<TDocument, TProjection> query, IPageQuery pageQuery)
    {
        var pagedData = query.PrepareObject(pageQuery);

        pagedData.Result = await
            query
           .Skip((pagedData.PageNumber - 1) * pagedData.PageSize)
           .Limit(pagedData.PageSize)
           .ToListAsync();

        return pagedData;
    }

    private static PagedData<TProjection> PrepareObject<TDocument, TProjection>(this IFindFluent<TDocument, TProjection> query, IPageQuery pageQuery)
    {
        var pagedData = new PagedData<TProjection>();

        if (pageQuery.PageNumber > 0)
            pagedData.PageNumber = pageQuery.PageNumber;

        if (pageQuery.PageSize > 0)
            pagedData.PageSize = pageQuery.PageSize;

        pagedData.TotalCount = pageQuery.NeedTotalCount ? (int)query.CountDocuments() : 0;


        if (!string.IsNullOrEmpty(pageQuery.SortBy))
        {
            if (pageQuery.SortAscending)
            {
                query = query.Sort(Builders<TDocument>.Sort.Ascending(pageQuery.SortBy));
            }
            else
            {
                query = query.Sort(Builders<TDocument>.Sort.Descending(pageQuery.SortBy));
            }
        }
        return pagedData;
    }
}


