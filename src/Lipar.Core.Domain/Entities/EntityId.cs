using Lipar.Core.Domain.Exceptions;
using System;

namespace Lipar.Core.Domain.Entities;

public class EntityId : ValueObject<EntityId>
{
    public Guid Value { get; private set; }

    private EntityId() { }
    public EntityId(string id)
    {
        if (Guid.TryParse(id, out Guid guid))
            Value = guid;
        else
            throw new InvalidValueObjectException("cannot cast {0} to {1} type", id, nameof(Guid));
    }

    public static EntityId FromString(string id) => new(id);
    public static EntityId FromGuid(Guid id) => new() { Value = id };
    public override string ToString() => Value.ToString();

    public override int ObjectGetHashCode() => Value.GetHashCode();
    public override bool ObjectIsEqual(EntityId otherObject) => Value.Equals(otherObject.Value);

    public static explicit operator string(EntityId id) => id.Value.ToString();
    public static implicit operator EntityId(string value) => new(value);

    public static explicit operator Guid(EntityId id) => id.Value;
    public static implicit operator EntityId(Guid value) => new() { Value = value };
}


