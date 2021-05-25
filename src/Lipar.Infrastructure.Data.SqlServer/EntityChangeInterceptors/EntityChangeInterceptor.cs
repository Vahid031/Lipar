using Lipar.Core.Domain.Entities;
using Lipar.Infrastructure.Data.SqlServer.EntityChangeInterceptors.Entities;
using Lipar.Infrastructure.Data.SqlServer.Extensions;
using Lipar.Infrastructure.Tools.Utilities.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lipar.Infrastructure.Data.SqlServer.EntityChangeInterceptors
{
    public static class EntityChangeInterceptor
    {
        public static void AuditAllChangeTracking(this DbContext db, IUserInfo userInfo, IDateTime dateTime)
        {
            var auditProperties = new List<string>
            {
                ModelBuilderExtensions.CreatedBy,
                ModelBuilderExtensions.CreatedDate,
                ModelBuilderExtensions.ModifedBy,
                ModelBuilderExtensions.ModifedDate,
                ModelBuilderExtensions.EntityId,
            };

            foreach (EntityEntry entry in db.ChangeTracker.Entries<AggregateRoot>()
                .Where(m => m.State == EntityState.Added || m.State == EntityState.Modified))
                db.ApplyAuditLog(entry, auditProperties, userInfo.UserId, dateTime.DateTime);
        }

        private static void ApplyAuditLog(this DbContext db, EntityEntry entry, List<string> auditProperties,
            string userId, DateTime dateTime)
        {
            var log = new EntityChangeLog()
            {
                Id = Guid.NewGuid(),
                EntityType = entry.Entity.GetType().FullName,
                EntityId = entry.Property(ModelBuilderExtensions.EntityId).CurrentValue.ToString(),
                State = entry.State.ToString(),
                UserId = userId,
                Date = dateTime,
                PropertyChangeLogs = new List<PropertyChangeLog>()
            };

            foreach (var item in entry.Properties.Where(m => auditProperties.All(p => p != m.Metadata.Name)))
            {
                if (entry.State == EntityState.Added || item.IsModified)
                {
                    var logDetail = new PropertyChangeLog
                    {
                        Id = Guid.NewGuid(),
                        EntityChangeLogId = log.Id,
                        Key = item.Metadata.Name,
                        Value = item.CurrentValue?.ToString()
                    };
                    log.PropertyChangeLogs.Add(logDetail);
                }
            }

            //Parallel.ForEach(entry.Properties.Where(m => auditProperties.All(p => p != m.Metadata.Name)),
            //    item =>
            //    {
            //        if (entry.State == EntityState.Added || item.IsModified)
            //        {
            //            var logDetail = new PropertyChangeLog
            //            {
            //                Id = Guid.NewGuid(),
            //                Key = item.Metadata.Name,
            //                Value = item.CurrentValue?.ToString()
            //            };
            //            log.PropertyChangeLogs.Add(logDetail);
            //        }
            //    });

            db.Set<EntityChangeLog>().Add(log);

        }
    }
}
