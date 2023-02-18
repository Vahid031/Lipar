using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Bson.Serialization;
using System;
using Lipar.Core.Domain.Entities;

namespace Lipar.Infrastructure.Data.Mongo.NewFolder
{
    public class EntityIdSerializer : SerializerBase<EntityId>
    {
        private readonly IBsonSerializer<Guid?> _serializer;
        public EntityIdSerializer(IBsonSerializer<Guid?> serializer) => _serializer = serializer;

        public override EntityId Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
            => _serializer.Deserialize(context, args).HasValue ? EntityId.FromGuid(_serializer.Deserialize(context, args).Value) : null;

        public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, EntityId value)
            => _serializer.Serialize(context, args, value?.Value);
    }
}
