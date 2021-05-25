using System;

namespace Lipar.Infrastructure.Data.SqlServer.EntityChangeInterceptors.Entities
{
    public class PropertyChangeLog
    {
        public Guid Id { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
        public EntityChangeLog EntityChangeLog { get; set; }
    }
}
