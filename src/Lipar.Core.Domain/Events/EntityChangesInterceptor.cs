using System;
using System.Collections.Generic;
using System.Linq;

namespace Lipar.Core.Domain.Events
{
    public class EntityChangesInterceptor
    {
        public Guid Id { get; private set; }
        public string EntityType { get; private set; }
        public Guid EntityId { get; private set; }
        public string State { get; private set; }
        public DateTime Date { get; private set; }
        public Guid UserId { get; private set; }
        public IReadOnlyCollection<EntityChangesInterceptorDetail> EntityChangesInterceptorDetails => _entityChangesInterceptorDetails.ToList();
        private HashSet<EntityChangesInterceptorDetail> _entityChangesInterceptorDetails { get; set; } = new HashSet<EntityChangesInterceptorDetail>();

        private EntityChangesInterceptor() { }

        public EntityChangesInterceptor(Guid id, string entityType, Guid entityId, string state)
        {
            Id = id;
            EntityType = entityType;
            EntityId = entityId;
            State = state;
        }

        public void SetDateTime(DateTime date)
        {
            Date = date;
        }

        public void SetUserId(Guid userId)
        {
            UserId = userId;
        }

        public void AddPropertyChangeLog(string key, string value) =>
            _entityChangesInterceptorDetails.Add(new EntityChangesInterceptorDetail(Guid.NewGuid(), key, value, Id));
    }
}
