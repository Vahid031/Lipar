using System;

namespace Lipar.Core.Domain.Events
{
    public class EntityChangesInterceptorDetail
    {
        public Guid Id { get; private set; }
        public string Key { get; private set; }
        public string Value { get; private set; }
        public Guid EntityChangesInterceptorId { get; private set; }

        private EntityChangesInterceptorDetail() { }

        public EntityChangesInterceptorDetail(Guid id, string key, string value, Guid entityChangesInterceptorId)
        {
            Id = id;
            Key = key;
            Value = value;
            EntityChangesInterceptorId = entityChangesInterceptorId;
        }
    }
}
