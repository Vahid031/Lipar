using Lipar.Infrastructure.Data.Mongo.Queries;
using Market.Infrastructure.Data.Mongo.Queries.Models;
using MongoDB.Driver;

namespace Market.Infrastructure.Data.Mongo.Queries.Common;

public class MongoDbMarketQueryDbContext : BaseQueryDbContext
{
    public MongoDbMarketQueryDbContext(IServiceProvider serviceProvider) : base(serviceProvider)
    {
    }

    public IMongoCollection<Product> Products => GetCollection<Product>(nameof(Product));
}


