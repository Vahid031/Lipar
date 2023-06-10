using Lipar.Core.Contract.Data;
using Lipar.Core.Contract.Services;
using Lipar.Core.Domain.Events;
using Lipar.Infrastructure.Tools.Utilities.Configurations;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lipar.Infrastructure.Data.Mongo.Repositories;

public class MongoDbEntityChangeInterceptorRepository : IEntityChangesInterceptorRepository
{
    private readonly IUserInfoService _userInfoService;
    private readonly IDateTimeService _dateTimeService;
    private readonly IJsonService _jsonService;
    private readonly IMongoCollection<EntityChangesInterception> _collection;

    static MongoDbEntityChangeInterceptorRepository() =>
        MongoDefaults.GuidRepresentation = GuidRepresentation.Standard;

    public MongoDbEntityChangeInterceptorRepository(LiparOptions liparOptions, IUserInfoService userInfoService, IJsonService jsonService, IDateTimeService dateTimeService)
    {
        var mongoClient = new MongoClient(liparOptions.EntityChangesInterception.MongoDb.Connection);
        var database = mongoClient.GetDatabase(liparOptions.EntityChangesInterception.MongoDb.DatabaseName);
        _collection = database.GetCollection<EntityChangesInterception>(liparOptions.EntityChangesInterception.MongoDb.Collection);
        _userInfoService = userInfoService;
        _jsonService = jsonService;
        _dateTimeService = dateTimeService;
    }

    public async Task AddEntityChanges(IEnumerable<EntityChangesInterception> entities)
    {
        if (!entities.Any())
            return;

        foreach (var entity in entities)
        {
            entity.SetDateTime(_dateTimeService.Now);
            entity.SetUserId(_userInfoService.UserId);
        }

        await _collection.InsertManyAsync(entities);
    }
}


