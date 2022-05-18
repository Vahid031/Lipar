using Lipar.Infrastructure.Data.SqlServer.Queries;
using Market.Infrastructure.Data.SqlServer.Queries.Models;
using Microsoft.EntityFrameworkCore;

namespace Market.Infrastructure.Data.SqlServerQuery.Common
{
    public partial class MarketQueryDbContext : BaseQueryDbContext
    {
        public MarketQueryDbContext(DbContextOptions options) : base(options)
        {
        }

        public virtual DbSet<EntityChangeLog> EntityChangeLogs { get; set; }
        public virtual DbSet<OutBoxEvent> OutBoxEventItems { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<PropertyChangeLog> PropertyChangeLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<EntityChangeLog>(entity =>
            {
                entity.HasKey(e => e.Id)
                    .IsClustered(false);

                entity.ToTable("EntityChangeLog");

                entity.HasIndex(e => e.Date, "IX_EntityChangeLog_Date")
                    .IsUnique()
                    .IsClustered();

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.EntityType).HasMaxLength(50);

                entity.Property(e => e.State).HasMaxLength(10);
            });

            modelBuilder.Entity<OutBoxEvent>(entity =>
            {
                entity.HasKey(e => e.Id)
                    .IsClustered(false);

                entity.HasIndex(e => e.AccuredOn, "IX_OutBoxEventItems_AccuredOn")
                    .IsUnique()
                    .IsClustered();

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.AccuredByUserId).HasMaxLength(40);

                entity.Property(e => e.AggregateId).HasMaxLength(40);

                entity.Property(e => e.AggregateName).HasMaxLength(200);

                entity.Property(e => e.AggregateTypeName).HasMaxLength(500);

                entity.Property(e => e.EventName).HasMaxLength(100);

                entity.Property(e => e.EventTypeName).HasMaxLength(500);
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(e => e.Id)
                    .IsClustered(false);

                entity.HasIndex(e => e.CreatedDate, "IX_Products_CreatedDate")
                    .IsUnique()
                    .IsClustered();

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Barcode).HasMaxLength(10);

                entity.Property(e => e.Name).HasMaxLength(50);
            });

            modelBuilder.Entity<PropertyChangeLog>(entity =>
            {
                entity.ToTable("PropertyChangeLog");

                entity.HasIndex(e => e.EntityChangeLogId, "IX_PropertyChangeLog_EntityChangeLogId");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Key).HasMaxLength(50);

                entity.Property(e => e.Value).HasMaxLength(250);

                entity.HasOne(d => d.EntityChangeLog)
                    .WithMany(p => p.PropertyChangeLogs)
                    .HasForeignKey(d => d.EntityChangeLogId);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
