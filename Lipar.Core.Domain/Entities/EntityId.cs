using Lipar.Core.Domain.Exceptions;
using System;

namespace Lipar.Core.Domain.Entities
{
    public class EntityId : ValueObject<EntityId>
    {
        public Guid Id { get; private set; }

        private EntityId() { }
        public EntityId(string id)
        {
            if (Guid.TryParse(id, out Guid guid))
                Id = guid;
            else
                throw new InvalidValueObjectException("cannot cast {0} to {1} type", id, nameof(Guid));
        }

        public static EntityId FromString(string id) => new EntityId(id);
        public static EntityId FromGuid(Guid id) => new EntityId() { Id = id };
        public string ToString(EntityId id) => Id.ToString();

        public override int ObjectGetHashCode() => Id.GetHashCode();
        public override bool ObjectIsEqual(EntityId otherObject) => Id.Equals(otherObject.Id);

        public static explicit operator Guid(EntityId entityId) => entityId.Id;
        public static implicit operator EntityId(Guid id) => new EntityId { Id = id };
    }
}
