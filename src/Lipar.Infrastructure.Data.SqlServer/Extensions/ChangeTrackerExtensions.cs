using Lipar.Core.Domain.Entities;
using Lipar.Core.Domain.Events;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lipar.Infrastructure.Data.SqlServer.Extensions;

public static class ChangeTrackerExtensions
{
    public static void SetShadowProperties(this ChangeTracker changeTracker)
    {
        var userId = Guid.NewGuid();

        // add Entity shadow properties
        changeTracker
        .Entries<Entity>()
        .Where(e => e.State == EntityState.Added).ToList()
        .ForEach(entity =>
        {
            entity.Property<DateTime>(ModelBuilderExtensions.CreatedOn).CurrentValue = DateTime.UtcNow;
        });


        // add IAuditable shadow properties
        changeTracker
        .Entries<IAuditable>()
        .ToList()
        .ForEach(entity =>
        {
            switch (entity.State)
            {
                case EntityState.Deleted:
                    /// when configuration for soft delete were develope, complete this place
                    break;
                case EntityState.Modified:
                    entity.Property<Guid?>(ModelBuilderExtensions.ModifedBy).CurrentValue = userId;
                    entity.Property<DateTime?>(ModelBuilderExtensions.ModifedOn).CurrentValue = DateTime.UtcNow;
                    break;
                case EntityState.Added:
                    entity.Property<Guid?>(ModelBuilderExtensions.CreatedBy).CurrentValue = userId;
                    break;
                default:
                    break;
            }
        });
    }

    public static List<EntityEntry<Entity>> GetTrackingAggrigates(this ChangeTracker changeTracker) =>
    changeTracker.Entries<Entity>()
    .Where(m => m.State == EntityState.Added || m.State == EntityState.Modified)
    .ToList();

    public static List<AggregateRoot> GetAggregatesWithEvent(this ChangeTracker changeTracker) =>
    changeTracker.Entries<AggregateRoot>()
    .Where(x => x.State != EntityState.Detached)
    .Select(c => c.Entity)
    .Where(c => c.GetEvents().Any())
    .ToList();

    public static IEnumerable<EntityChangesInterception> GetEntityChangesInterceptor(this ChangeTracker changeTracker)
    {
        IEnumerable<EntityEntry<Entity>> entries = changeTracker.GetTrackingAggrigates();

        var auditProperties = new List<string>
        {
            ModelBuilderExtensions.CreatedBy,
            ModelBuilderExtensions.CreatedOn,
            ModelBuilderExtensions.ModifedBy,
            ModelBuilderExtensions.ModifedOn,
            ModelBuilderExtensions.Id,
        };

        foreach (EntityEntry entry in entries)
            yield return ApplyAuditLog(entry, auditProperties);
    }

    private static EntityChangesInterception ApplyAuditLog(EntityEntry entry, List<string> auditProperties)
    {
        var log = new EntityChangesInterception(
        Guid.NewGuid(),
        entry.Entity.GetType().Name,
        ((EntityId)entry.Property(ModelBuilderExtensions.Id).CurrentValue).Value,
        entry.State.ToString());

        foreach (var item in entry.Properties.Where(m => auditProperties.All(p => p != m.Metadata.Name)))
        {
            if (entry.State == EntityState.Added || item.IsModified)
            {
                log.AddDetail(item.Metadata.Name, item.CurrentValue?.ToString());
            }
        }
        return log;
    }

}


