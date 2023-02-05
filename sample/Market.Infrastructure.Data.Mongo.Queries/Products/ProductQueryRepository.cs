using Lipar.Core.Domain.Queries;
using Market.Core.Domain.Products.Contracts;
using Market.Infrastructure.Data.Mongo.Queries.Common;
using Lipar.Infrastructure.Data.Mongo.Queries;
using MongoDB.Driver;
using Lipar.Infrastructure.Data.Mongo.Extensions;
using Market.Core.Domain.Products.QueryResults;
using Market.Infrastructure.Data.Mongo.Queries.Models;

namespace Market.Infrastructure.Data.Mongo.Queries.Products;

public class ProductQueryRepository : BaseQueryRepository<MongoDbMarketQueryDbContext>,
IProductQueryRepository
{
    public ProductQueryRepository(MongoDbMarketQueryDbContext db) : base(db)
    {
    }

    public async Task<PagedData<GetProduct>> Select(IGetProduct input)
    {
        var filter = Builders<Product>.Filter.Empty;

        if (!string.IsNullOrEmpty(input.Name))
            filter &= Builders<Product>.Filter.Where(m => m.Name.Contains(input.Name));

        if (!string.IsNullOrEmpty(input.Barcode))
            filter &= Builders<Product>.Filter.Where(m => m.Barcode.Contains(input.Barcode));



        var query = _db.Products
            .Find(filter)
            .Project(m => new GetProduct
            {
                Id = m.Id,
                Name = m.Name,
                Barcode = m.Barcode,
            });
         
        return await query.PagingAsync(input);
    }

    public async Task<GetByIdProduct> Select(IGetByIdProduct input)
    {
        var query = _db.Products
           .Find(m => m.Id == input.Id)
           .Project(m => new GetByIdProduct
           {
               Barcode = m.Barcode,
               Name = m.Name,
               Id = m.Id
           });

        return await query.FirstOrDefaultAsync();
    }
}


