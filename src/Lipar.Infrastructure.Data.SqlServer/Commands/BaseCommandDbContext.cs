using Microsoft.EntityFrameworkCore;
using Lipar.Infrastructure.Data.SqlServer.Extensions;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Linq;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Lipar.Core.Contract.Data;
using Lipar.Core.Domain.Events;
using Lipar.Core.Contract.Services;

namespace Lipar.Infrastructure.Data.SqlServer.Commands;

public abstract class BaseCommandDbContext : DbContext
{

    #region Ctreator and Configuration
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

        base.OnModelCreating(modelBuilder);
    }

    #endregion

    #region Commit Process

    public async Task<int> SaveChangesAsync()
    {
        ChangeTracker.DetectChanges();
        ChangeTracker.AutoDetectChangesEnabled = false;

        ChangeTracker.SetShadowProperties();
        await AddEntityChangesInterceptors();
        await AddOutboxEvetItems();

        var rowAffect = await base.SaveChangesAsync();

        ChangeTracker.AutoDetectChangesEnabled = true;

        return rowAffect;
    }

    private async Task AddEntityChangesInterceptors()
    {
        var entityChangesInterceptors = ChangeTracker.GetEntityChangesInterceptor();
        var repository = this.GetService<IEntityChangesInterceptorRepository>();

        await repository.AddEntityChanges(entityChangesInterceptors);
    }

    private async Task AddOutboxEvetItems()
    {
        var changedAggregates = ChangeTracker.GetAggregatesWithEvent();
        var repository = this.GetService<IOutBoxEventRepository>();
        var dateTimeService = this.GetService<IDateTimeService>();
        var userInfoService = this.GetService<IUserInfoService>();
        var jsonService = this.GetService<IJsonService>();

        List<OutBoxEvent> outBoxEvents = new();

        changedAggregates.ForEach(aggregate =>
        {
            outBoxEvents.AddRange(
                aggregate.GetChanges().Select(@event => new OutBoxEvent
                {
                    Id = Guid.NewGuid(),
                    AccuredByUserId = userInfoService.UserId.ToString(),
                    AccuredOn = dateTimeService.Now,
                    //AggregateId = aggregate.Id.ToString(),
                    //AggregateName = aggregate.GetType().Name,
                    //AggregateTypeName = aggregate.GetType().FullName,
                    EventName = @event.GetType().Name,
                    EventTypeName = @event.GetType().FullName,
                    EventPayload = jsonService.SerializeObject(@event),
                    IsProcessed = false
                })
            );
        });

        await repository.AddOutboxEvetItems(outBoxEvents);
    }

    #endregion

    #region Methods

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

    #endregion
}


