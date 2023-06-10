using System.Collections.Generic;
using Lipar.Infrastructure.Tools.Utilities.Configurations;
using Lipar.Core.Contract.Data;
using Lipar.Core.Domain.Events;
using System.Threading.Tasks;
using Lipar.Core.Contract.Services;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Linq;

namespace Lipar.Infrastructure.Data.Mongo.Repositories;

public class MongoDbOutBoxEventRepository : IOutBoxEventRepository
{
    private readonly IMongoCollection<OutBoxEvent> _collection;

    static MongoDbOutBoxEventRepository() =>
        MongoDefaults.GuidRepresentation = GuidRepresentation.Standard;

    public MongoDbOutBoxEventRepository(LiparOptions liparOptions)
    {
        var mongoClient = new MongoClient(liparOptions.OutBoxEvent.MongoDb.Connection);
        var database = mongoClient.GetDatabase(liparOptions.OutBoxEvent.MongoDb.DatabaseName);
        _collection = database.GetCollection<OutBoxEvent>(liparOptions.OutBoxEvent.MongoDb.Collection);
    }


    public async Task AddOutboxEvent(OutBoxEvent outBoxEvent)
    {
        await _collection.InsertOneAsync(outBoxEvent);
    }

    public async Task<List<OutBoxEvent>> GetOutBoxEventItemsForPublish(int maxCount)
    {
        return await _collection.Find(m => !m.IsProcessed).Limit(maxCount).ToListAsync();
    }

    public async Task MarkAsRead(List<OutBoxEvent> outBoxEvents)
    {
        await _collection.UpdateManyAsync(
            Builders<OutBoxEvent>.Filter.Where(m => outBoxEvents.Select(o => o.Id).Contains(m.Id)), 
            Builders<OutBoxEvent>.Update.Set(m => m.IsProcessed, true));
    }
}


