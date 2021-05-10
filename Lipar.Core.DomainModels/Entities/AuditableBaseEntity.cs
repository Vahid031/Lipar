using System;

namespace Lipar.Core.DomainModels.Entities
{
    public abstract class AuditableBaseEntity
    {
        public long ClusteredId { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedTime { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime? LastModifiedTime { get; set; }
    }
}
