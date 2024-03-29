using Lipar.Infrastructure.Data.Mongo.Commands;
using Market.Core.Domain.Products.Contracts;
using Market.Core.Domain.Products.Entities;
using Market.Infrastructure.Data.Mongo.Commands.Common;

namespace Market.Infrastructure.Data.Mongo.Commands.Products;

public class ProductCommandRepository : BaseCommandRepository<Product, MongoDbMarketCommandDbContext>,
    IProductCommandRepository
{
    public ProductCommandRepository(MongoDbMarketCommandDbContext db) : base(db)
    {
    }
}


