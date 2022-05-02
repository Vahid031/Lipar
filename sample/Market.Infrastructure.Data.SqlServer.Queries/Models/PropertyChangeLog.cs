using System;
using System.Collections.Generic;

#nullable disable

namespace Market.Infrastructure.Data.SqlServer.Queries.Models
{
    public partial class PropertyChangeLog
    {
        public Guid Id { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
        public Guid EntityChangeLogId { get; set; }

        public virtual EntityChangeLog EntityChangeLog { get; set; }
    }
}
