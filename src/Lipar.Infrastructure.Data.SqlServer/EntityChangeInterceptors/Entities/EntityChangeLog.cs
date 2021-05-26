using System;
using System.Collections.Generic;
using System.Linq;

namespace Lipar.Infrastructure.Data.SqlServer.EntityChangeInterceptors.Entities
{
    public class EntityChangeLog
    {
        public Guid Id { get; private set; }
        public string EntityType { get; private set; }
        public Guid EntityId { get; private set; }
        public string State { get; private set; }
        public DateTime Date { get; private set; }
        public Guid UserId { get; private set; }
        public IReadOnlyCollection<PropertyChangeLog> PropertyChangeLogs  => _propertyChangeLogs.ToList();
        private HashSet<PropertyChangeLog> _propertyChangeLogs { get; set; } = new HashSet<PropertyChangeLog>();

        private EntityChangeLog() { }

        public EntityChangeLog(Guid id, string entityType, Guid entityId, string state, DateTime date, Guid userId)
        {
            Id = id;
            EntityType = entityType;
            EntityId = entityId;
            State = state;
            Date = date;
            UserId = userId;
        }

        public void AddPropertyChangeLog(string key, string value) =>
            _propertyChangeLogs.Add(new PropertyChangeLog(Guid.NewGuid(), key, value, Id));
    }
}
