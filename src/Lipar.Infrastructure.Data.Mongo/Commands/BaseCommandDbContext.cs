using System.Threading.Tasks;
using System.Collections.Generic;
using System;
using System.Linq;
using Lipar.Core.Contract.Data;

namespace Lipar.Infrastructure.Data.Mongo.Commands
{
    public abstract class BaseCommandDbContext 
    {

        #region Ctreator and Configuration
        //public BaseCommandDbContext(DbContextOptions options) : base(options)
        //{

        //}

        protected BaseCommandDbContext()
        {
        }

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    modelBuilder.AddEntityId();
        //    modelBuilder.AddAuditableProperties();

        //    base.OnModelCreating(modelBuilder);
        //}

        #endregion

        #region Commit Process

        //public async Task<int> SaveChangesAsync()
        //{
        //    ChangeTracker.DetectChanges();
        //    ChangeTracker.AutoDetectChangesEnabled = false;

        //    ChangeTracker.SetShadowProperties();
        //    AddEntityChangesInterceptors();
        //    AddOutboxEvetItems();

        //    var rowAffect = await base.SaveChangesAsync();

        //    ChangeTracker.AutoDetectChangesEnabled = true;

        //    return rowAffect;
        //}

        //private void AddEntityChangesInterceptors()
        //{
        //    var entityChangesInterceptors = ChangeTracker.GetEntityChangesInterceptor();
        //    var repository = this.GetService<IEntityChangesInterceptorRepository>();

        //    repository.AddEntityChanges(entityChangesInterceptors);
        //}

        //private void AddOutboxEvetItems()
        //{
        //    var changedAggregates = ChangeTracker.GetAggregatesWithEvent();
        //    var repository = this.GetService<IOutBoxEventRepository>();

        //    repository.AddOutboxEvetItems(changedAggregates);
        //}

        #endregion
    }
}
