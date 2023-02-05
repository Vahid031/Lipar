using Lipar.Core.Domain.Queries;
using Market.Core.Domain.Products.QueryResults;
using Market.Core.Domain.Products.Contracts;
using Lipar.Infrastructure.Data.Mongo.Queries;
using Market.Infrastructure.Data.Mongo.Queries.Common;
using MongoDB.Driver;

namespace Market.Infrastructure.Data.Mongo.Queries.Products;

public class ProductQueryRepository : BaseQueryRepository<MongoDbMarketQueryDbContext>,
IProductQueryRepository
{
    public ProductQueryRepository(MongoDbMarketQueryDbContext db) : base(db)
    {
    }

    public Task<PagedData<GetProduct>> Select(IGetProduct input)
    {

        var query = Products.Find().Select(m => new GetProduct
        {
            Id = m.Id,
            Name = m.Name,
            Barcode = m.Barcode,
        });

        if (!string.IsNullOrEmpty(input.Name))
            query = query.Where(m => m.Name.Contains(input.Name));


        if (!string.IsNullOrEmpty(input.Barcode))
            query = query.Where(m => m.Barcode.Contains(input.Barcode));


        return query.PagingAsync(input);
    }

    public async Task<GetByIdProduct> Select(IGetByIdProduct input) =>
    await Products
    .Where(m => m.Id == input.Id)
    .Select(m => new GetByIdProduct
    {
        Barcode = m.Barcode,
        Name = m.Name,
        Id = m.Id
    })
    .FirstOrDefaultAsync();
}


