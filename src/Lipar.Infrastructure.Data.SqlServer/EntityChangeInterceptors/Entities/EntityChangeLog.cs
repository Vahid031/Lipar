using System;
using System.Collections.Generic;

namespace Lipar.Infrastructure.Data.SqlServer.EntityChangeInterceptors.Entities
{
    public class EntityChangeLog
    {
        public Guid Id { get; set; }
        public string EntityType { get; set; }
        public string EntityId { get; set; }
        public string State { get; set; }
        public DateTime Date { get; set; }
        public string UserId { get; set; }
        public List<PropertyChangeLog> PropertyChangeLogs { get; set; }
    }
}
