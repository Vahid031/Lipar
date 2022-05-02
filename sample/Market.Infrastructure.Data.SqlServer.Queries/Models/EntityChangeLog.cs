using System;
using System.Collections.Generic;

#nullable disable

namespace Market.Infrastructure.Data.SqlServer.Queries.Models
{
    public partial class EntityChangeLog
    {
        public EntityChangeLog()
        {
            PropertyChangeLogs = new HashSet<PropertyChangeLog>();
        }

        public Guid Id { get; set; }
        public string EntityType { get; set; }
        public Guid EntityId { get; set; }
        public string State { get; set; }
        public DateTime Date { get; set; }
        public Guid UserId { get; set; }

        public virtual ICollection<PropertyChangeLog> PropertyChangeLogs { get; set; }
    }
}
