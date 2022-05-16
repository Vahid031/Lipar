using Lipar.Core.Contract.Utilities;
using Lipar.Core.Domain.Entities;
using Lipar.Infrastructure.Data.SqlServer.EntityChangeInterceptors.Entities;
using Lipar.Infrastructure.Data.SqlServer.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Lipar.Infrastructure.Data.SqlServer.EntityChangeInterceptors
{
    public class EntityChangeInterceptor
    {
        public static IEnumerable<AuditLog> AuditAllChangeTracking(IEnumerable<EntityEntry<Entity>> entries, 
            IUserInfo userInfo, IDateTime dateTime)
        {
            var auditProperties = new List<string>
            {
                ModelBuilderExtensions.CreatedBy,
                ModelBuilderExtensions.CreatedDate,
                ModelBuilderExtensions.ModifedBy,
                ModelBuilderExtensions.ModifedDate,
                ModelBuilderExtensions.EntityId,
            };

            foreach (EntityEntry entry in entries)
                yield return ApplyAuditLog(entry, auditProperties, userInfo.UserId, dateTime.DateTime);
        }

        private static AuditLog ApplyAuditLog(EntityEntry entry, List<string> auditProperties,
            Guid userId, DateTime dateTime)
        {
            var log = new AuditLog(
                Guid.NewGuid(),
                entry.Entity.GetType().Name,
                ((EntityId)entry.Property(ModelBuilderExtensions.EntityId).CurrentValue).Value,
                entry.State.ToString(),
                dateTime,
                userId);

            foreach (var item in entry.Properties.Where(m => auditProperties.All(p => p != m.Metadata.Name)))
            {
                if (entry.State == EntityState.Added || item.IsModified)
                {
                    log.AddPropertyChangeLog(item.Metadata.Name, item.CurrentValue?.ToString());
                }
            }
            return log;
        }
    }
}
