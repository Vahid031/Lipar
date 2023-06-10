using Lipar.Core.Contract.Data;
using Lipar.Core.Contract.Services;
using Lipar.Core.Domain.Events;
using Lipar.Infrastructure.Tools.Utilities.Configurations;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Threading.Tasks;

namespace Lipar.Infrastructure.Data.Mongo.Repositories;

public class MongoDbInBoxEventRepository : IInBoxEventRepository
{
    private readonly IDateTimeService _dateTimeService;
    private readonly IMongoCollection<InBoxEvent> _collection;

    static MongoDbInBoxEventRepository() =>
        MongoDefaults.GuidRepresentation = GuidRepresentation.Standard;

    public MongoDbInBoxEventRepository(LiparOptions liparOptions, IDateTimeService dateTimeService)
    {
        var mongoClient = new MongoClient(liparOptions.InBoxEvent.MongoDb.Connection);
        var database = mongoClient.GetDatabase(liparOptions.InBoxEvent.MongoDb.DatabaseName);
        _collection = database.GetCollection<InBoxEvent>(liparOptions.InBoxEvent.MongoDb.Collection);
        _dateTimeService = dateTimeService;
    }

    public bool AllowReceive(string messageId, string fromService)
    {
        var result = _collection.Find(m => m.OwnerService == fromService && m.MessageId == messageId).Any();

        return !result;
    }

    public async Task Receive(string messageId, string fromService)
    {
        await _collection.InsertOneAsync(
            new InBoxEvent
            {
                Id = Guid.NewGuid(),
                OwnerService = fromService,
                MessageId = messageId,
                ReceivedDate = _dateTimeService.Now
            });
    }
}


