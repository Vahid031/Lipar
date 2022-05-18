using Lipar.Infrastructure.Data.SqlServer.Commands;
using Lipar.Infrastructure.Data.SqlServer.EntityChangeInterceptor.Configs;
using Lipar.Infrastructure.Data.SqlServer.OutBoxEvents.Configs;
using Market.Core.Domain.Products.Entities;
using Microsoft.EntityFrameworkCore;

namespace Market.Infrastructure.Data.SqlServer.Commands.Common
{
    public class MarketCommandDbContext : BaseCommandDbContext
    {
        #region Create and Configuration
        public MarketCommandDbContext(DbContextOptions<MarketCommandDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);

            modelBuilder.ApplyConfiguration(new OutBoxEventConfiguration());
            modelBuilder.ApplyConfiguration(new EntityChangesInterceptorConfiguration());
            modelBuilder.ApplyConfiguration(new EntityChangesInterceptorDetailConfiguration());

            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=Market;Trusted_Connection=True;");
            base.OnConfiguring(optionsBuilder);
        }
        #endregion
        
        DbSet<Product> Products { get; set; }

    }
}