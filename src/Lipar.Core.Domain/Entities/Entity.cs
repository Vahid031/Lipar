namespace Lipar.Core.Domain.Entities
{
    public abstract class Entity
    {
        public EntityId Id { get; protected set; }
        protected Entity() { }
    }
}
