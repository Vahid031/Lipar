using Lipar.Core.Contract.Data;
using Lipar.Core.Contract.Services;
using Lipar.Core.Domain.Events;
using Lipar.Infrastructure.Tools.Utilities.Configurations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lipar.Infrastructure.Data.Mongo.Repositories;

public class MongoDbInBoxEventRepository : IInBoxEventRepository
{
    private readonly IDateTimeService _dateTimeService;
    private readonly IMongoCollection<InBoxEvent> _collection;

    static MongoDbInBoxEventRepository() =>
        BsonSerializer.TryRegisterSerializer(
            new GuidSerializer(GuidRepresentation.Standard));

    public MongoDbInBoxEventRepository(LiparOptions liparOptions, IDateTimeService dateTimeService)
    {
        var mongoClient = new MongoClient(liparOptions.InBoxEvent.MongoDb.Connection);
        var database = mongoClient.GetDatabase(liparOptions.InBoxEvent.MongoDb.DatabaseName);
        _collection = database.GetCollection<InBoxEvent>(liparOptions.InBoxEvent.MongoDb.Collection);
        _dateTimeService = dateTimeService;
    }

    public Task FailEventHandeling(InBoxEvent @event)
    {
        throw new NotImplementedException();
    }

    public Task<bool> MakeUnknownStatus(List<InBoxEvent> events)
    {
        throw new NotImplementedException();
    }

    public Task ReceiveNewEvent(InBoxEvent @event)
    {
        throw new NotImplementedException();
    }

    public Task<InBoxEvent> ScheduleIncomingEvent()
    {
        throw new NotImplementedException();
    }

    public Task SuccessEventHandeling(InBoxEvent @event)
    {
        throw new NotImplementedException();
    }

    //public async Task<bool> AllowReceiveAsync(string messageId)
    //{
    //    var result = await _collection.FindAsync(m => m.MessageId == messageId);

    //    return !result.Any();
    //}

    //public async Task Receive(InBoxEvent @event)
    //{
    //    await _collection.InsertOneAsync(@event);
    //}
}


