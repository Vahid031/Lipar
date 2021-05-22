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
    public class EntityChangeInterceptor
    {
        public void AuditAllChangeTracking()
        {
            var auditProperties = new List<string>
            {
                ModelBuilderExtensions.CreatedBy,
                ModelBuilderExtensions.CreatedDate,
                ModelBuilderExtensions.ModifedBy,
                ModelBuilderExtensions.ModifedDate,
                ModelBuilderExtensions.EntityId,
            };

            //foreach (EntityEntry entry in appDbContext.ChangeTrackerEntries)
            //    ApplyAuditLog(entry, memberId);
        }

        private void ApplyAuditLog(EntityEntry entry, List<string> auditProperties, 
            string userId, DateTime dateTime)
        {
            var log = new PropertyLog()
            {
                Id = Guid.NewGuid(),
                EntityType = entry.Entity.GetType().FullName,
                EntityId = entry.Property(ModelBuilderExtensions.EntityId).CurrentValue.ToString(),
                State = entry.State.ToString(),
                UserId = userId,
                Date = dateTime,
                PropertyLogDetails = new List<PropertyLogDetail>()
            };


            Parallel.ForEach(entry.Properties.Where(c => auditProperties.All(d => d != c.Metadata.Name)),
                item =>
                {
                    if (entry.State == EntityState.Added || item.IsModified)
                    {
                        PropertyLogDetail logDetail = new PropertyLogDetail
                        {
                            Id = Guid.NewGuid(),
                            Key = item.Metadata.Name,
                            Value = item.CurrentValue?.ToString()
                        };
                        log.PropertyLogDetails.Add(logDetail);
                    }
                });

            appDbContext.Set<PropertyLog>().Add(log);

        }
    }
}
