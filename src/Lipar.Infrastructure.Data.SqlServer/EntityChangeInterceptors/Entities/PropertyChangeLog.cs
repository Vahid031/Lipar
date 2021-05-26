using System;

namespace Lipar.Infrastructure.Data.SqlServer.EntityChangeInterceptors.Entities
{
    public class PropertyChangeLog
    {
        public Guid Id { get; private set; }
        public string Key { get; private set; }
        public string Value { get; private set; }
        public Guid EntityChangeLogId { get; private set; }

        private PropertyChangeLog() { }

        public PropertyChangeLog(Guid id, string key, string value, Guid entityChangeLogId)
        {
            Id = id;
            Key = key;
            Value = value;
            EntityChangeLogId = entityChangeLogId;
        }
    }
}
