using Microsoft.EntityFrameworkCore;
using Lipar.Infrastructure.Data.SqlServer.Extensions;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Linq;
using Lipar.Infrastructure.Data.SqlServer.EntityChangeInterceptors.Entities;
using Lipar.Infrastructure.Data.SqlServer.EntityChangeInterceptors.Configs;
using Lipar.Infrastructure.Data.SqlServer.EntityChangeInterceptors;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Lipar.Infrastructure.Tools.Utilities.Services;
using Lipar.Infrastructure.Data.SqlServer.OutBoxEvents.Configs;
//using Lipar.Infrastructure.Events.OutboxEvent;

namespace Lipar.Infrastructure.Data.SqlServer.Commands
{
    public abstract class BaseCommandDbContext : DbContext
    {
        //public DbSet<OutBoxEventItem> OutBoxEventItems { get; set; }
        public BaseCommandDbContext(DbContextOptions options) : base(options)
        {

        }

        protected BaseCommandDbContext()
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.AddEntityId();
            modelBuilder.AddAuditableProperties();
            modelBuilder.ApplyConfiguration(new OutBoxEventItemConfiguration());
            modelBuilder.ApplyConfiguration(new EntityChangeLogConfiguration());
            modelBuilder.ApplyConfiguration(new PropertyChangeLogConfiguration());

            base.OnModelCreating(modelBuilder);
        }

        public async Task<int> SaveChangesAsync()
        {
            var userInfo = this.GetService<IUserInfo>();
            var dateTime = this.GetService<IDateTime>();

            ChangeTracker.DetectChanges();
            ChangeTracker.AutoDetectChangesEnabled = false;
            ChangeTracker.SetShadowProperties();
            var list = EntityChangeInterceptor.AuditAllChangeTracking(
                  ChangeTracker.GetTrackingAggrigates(), userInfo, dateTime).ToList();
            Set<EntityChangeLog>().AddRange(list);

            AddOutboxEvetItems();

            var rowAffect = await base.SaveChangesAsync();

            ChangeTracker.AutoDetectChangesEnabled = true;

            return rowAffect;
        }

        public IEnumerable<string> GetIncludePaths(Type clrEntityType)
        {
            var entityType = Model.FindEntityType(clrEntityType);
            var includedNavigations = new HashSet<INavigation>();
            var stack = new Stack<IEnumerator<INavigation>>();
            while (true)
            {
                var entityNavigations = new List<INavigation>();
                foreach (var navigation in entityType.GetNavigations())
                {
                    if (includedNavigations.Add(navigation))
                        entityNavigations.Add(navigation);
                }
                if (entityNavigations.Count == 0)
                {
                    if (stack.Count > 0)
                        yield return string.Join(".", stack.Reverse().Select(e => e.Current.Name));
                }
                else
                {
                    foreach (var navigation in entityNavigations)
                    {
                        var inverseNavigation = navigation.Inverse;
                        if (inverseNavigation != null)
                            includedNavigations.Add(inverseNavigation);
                    }
                    stack.Push(entityNavigations.GetEnumerator());
                }
                while (stack.Count > 0 && !stack.Peek().MoveNext())
                    stack.Pop();
                if (stack.Count == 0) break;
                entityType = stack.Peek().Current.TargetEntityType;
            }
        }

        private void AddOutboxEvetItems()
        {
            var changedAggregates = ChangeTracker.GetAggregatesWithEvent();
            var userInfo = this.GetService<IUserInfo>();
            var jsonConvert = this.GetService<IJson>();

            foreach (var aggregate in changedAggregates)
            {
                var events = aggregate.GetChanges();
                //foreach (var @event in events)
                //{
                //    OutBoxEventItems.Add(new OutBoxEventItem
                //    {
                //        Id = Guid.NewGuid(),
                //        AccuredByUserId = userInfo.UserId.ToString(),
                //        AccuredOn = DateTime.UtcNow,
                //        AggregateId = aggregate.Id.ToString(),
                //        AggregateName = aggregate.GetType().Name,
                //        AggregateTypeName = aggregate.GetType().FullName,
                //        EventName = @event.GetType().Name,
                //        EventTypeName = @event.GetType().FullName,
                //        EventPayload = jsonConvert.SerializeObject(@event),
                //        IsProcessed = false
                //    });
                //}
            }
        }
    }
}
