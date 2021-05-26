using Lipar.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lipar.Infrastructure.Data.SqlServer.Extensions
{
    public static class ChangeTrackerExtensions
    {
        public static void SetShadowProperties(this ChangeTracker changeTracker)
        {
            var userId = Guid.NewGuid();

            // add Entity shadow properties
            Parallel.ForEach(
                changeTracker.Entries<Entity>()
                              .Where(e => e.State == EntityState.Added),
                entity =>
                {
                    entity.Property<DateTime>(ModelBuilderExtensions.CreatedDate).CurrentValue = DateTime.UtcNow;
                });


            // add IAuditable shadow properties
            Parallel.ForEach(
                changeTracker.Entries<IAuditable>(),
                entity =>
                {
                    switch (entity.State)
                    {
                        case EntityState.Deleted:
                            /// when configuration for soft delete were develope, complete this place
                            break;
                        case EntityState.Modified:
                            entity.Property<Guid?>(ModelBuilderExtensions.ModifedBy).CurrentValue = userId;
                            entity.Property<DateTime?>(ModelBuilderExtensions.ModifedDate).CurrentValue = DateTime.UtcNow;
                            break;
                        case EntityState.Added:
                            entity.Property<Guid?>(ModelBuilderExtensions.CreatedBy).CurrentValue = userId;
                            break;
                        default:
                            break;
                    }
                });
        }

        public static IEnumerable<EntityEntry<Entity>> GetTrackingAggrigates(this ChangeTracker changeTracker)
        {
            return changeTracker
                .Entries<Entity>()
                .Where(m => m.State == EntityState.Added || m.State == EntityState.Modified);
        }
    }
}
