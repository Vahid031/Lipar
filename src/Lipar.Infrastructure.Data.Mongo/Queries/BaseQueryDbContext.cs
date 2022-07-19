using Lipar.Infrastructure.Tools.Utilities.Configurations;
using MongoDB.Driver;

namespace Lipar.Infrastructure.Data.Mongo.Queries;

public abstract class BaseQueryDbContext
{
    private IMongoDatabase Database { get; }
    private MongoClient MongoClient { get; set; }

    private BaseQueryDbContext() { }

    protected BaseQueryDbContext(LiparOptions liparOptions)
    {
        MongoClient = new MongoClient(liparOptions.MongoDb.Connection);
        Database = MongoClient.GetDatabase(liparOptions.MongoDb.DatabaseName);
    }

    public IMongoCollection<T> GetCollection<T>(string name) => Database.GetCollection<T>(name);

}



