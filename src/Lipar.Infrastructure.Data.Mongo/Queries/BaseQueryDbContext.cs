using Lipar.Infrastructure.Tools.Utilities.Configurations;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson;
using MongoDB.Driver;
using System;

namespace Lipar.Infrastructure.Data.Mongo.Queries;

public abstract class BaseQueryDbContext
{
    private IMongoDatabase Database { get; }
    private MongoClient MongoClient { get; set; }
    public IServiceProvider ServiceProvider { get; }

    private BaseQueryDbContext() { }

    static BaseQueryDbContext() =>
    MongoDefaults.GuidRepresentation = GuidRepresentation.Standard;

    protected BaseQueryDbContext(IServiceProvider serviceProvider)
    {
        MongoDefaults.GuidRepresentation = GuidRepresentation.Standard;
        var liparOptions = serviceProvider.GetService<LiparOptions>();
        MongoClient = new MongoClient(liparOptions.MongoDb.Connection);
        Database = MongoClient.GetDatabase(liparOptions.MongoDb.DatabaseName);
        ServiceProvider = serviceProvider;
    }

    public IMongoCollection<T> GetCollection<T>(string name) => Database.GetCollection<T>(name);

}



