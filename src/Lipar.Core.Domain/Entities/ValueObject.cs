using System;

namespace Lipar.Core.Domain.Entities
{
    /// <summary>
    /// https://martinfowler.com/bliki/ValueObject.html
    /// </summary>
    /// <typeparam name="TValueObject"></typeparam>
    public abstract class ValueObject<TValueObject> : IEquatable<TValueObject>
            where TValueObject : ValueObject<TValueObject>
    {
        public bool Equals(TValueObject other)
        {
            if(other == null)
                return false;
            return this == other;
        }
        public override bool Equals(object obj)
        {
            var otherObject = obj as TValueObject;
            if (otherObject == null)
                return false;
            return ObjectIsEqual(otherObject);
        }
        public override int GetHashCode()
        {
            return ObjectGetHashCode();
        }
        public abstract bool ObjectIsEqual(TValueObject otherObject);
        public abstract int ObjectGetHashCode();
        public static bool operator ==(ValueObject<TValueObject> right, ValueObject<TValueObject> left)
        {
            if (right is null && left is null)
                return true;
            if (right is null || left is null)
                return false;
            return right.Equals(left);
        }
        public static bool operator !=(ValueObject<TValueObject> right, ValueObject<TValueObject> left)
        {
            return !(right == left);
        }

    }
}
