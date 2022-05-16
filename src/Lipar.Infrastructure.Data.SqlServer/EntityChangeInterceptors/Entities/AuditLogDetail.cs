using System;

namespace Lipar.Infrastructure.Data.SqlServer.EntityChangeInterceptors.Entities
{
    public class AuditLogDetail
    {
        public Guid Id { get; private set; }
        public string Key { get; private set; }
        public string Value { get; private set; }
        public Guid AuditLogId { get; private set; }

        private AuditLogDetail() { }

        public AuditLogDetail(Guid id, string key, string value, Guid auditLogId)
        {
            Id = id;
            Key = key;
            Value = value;
            AuditLogId = auditLogId;
        }
    }
}
