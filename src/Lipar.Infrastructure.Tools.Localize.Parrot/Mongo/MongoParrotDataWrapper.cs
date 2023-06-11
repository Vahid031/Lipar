using System;
using System.Collections.Generic;
using System.Linq;
using Lipar.Infrastructure.Tools.Utilities.Configurations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;

namespace Lipar.Infrastructure.Tools.Localize.Parrot.Mongo;

public class MongoParrotDataWrapper : ParrotDataWrapper
{
    private readonly IMongoCollection<LocalizationRecord> _collection;
    private static ParrotDataWrapper _instance;
    private List<LocalizationRecord> _localizationRecords;
    public static ParrotDataWrapper CreateFactory(LiparOptions liparOptions)
    {
        if (_instance is null)
            _instance = new MongoParrotDataWrapper(liparOptions);

        return _instance;
    }

    static MongoParrotDataWrapper() =>
        BsonSerializer.TryRegisterSerializer(
            new GuidSerializer(GuidRepresentation.Standard));

    private MongoParrotDataWrapper(LiparOptions liparOptions)
    {
        var mongoClient = new MongoClient(liparOptions.Translation.MongoDb.Connection);
        var database = mongoClient.GetDatabase(liparOptions.Translation.MongoDb.DatabaseName);
        _collection = database.GetCollection<LocalizationRecord>(liparOptions.Translation.MongoDb.Collection);
        _localizationRecords = _collection.Find(Builders<LocalizationRecord>.Filter.Empty).ToList();
    }

    public override string Get(string key, string culture)
    {
        var record = _localizationRecords.FirstOrDefault(c => c.Key == key && c.Culture == culture);
        if (record == null)
        {
            record = new LocalizationRecord
            {
                Id = Guid.NewGuid(),
                Key = key,
                Culture = culture,
                Value = key
            };

            _collection.InsertOne(record);
        }
        return record.Value;
    }
}


