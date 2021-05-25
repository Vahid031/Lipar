using Lipar.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Lipar.Infrastructure.Data.SqlServer.Extensions
{
    public static class ModelBuilderExtensions
    {
        public static void AddEntityId(this ModelBuilder modelBuilder)
        {
            Parallel.ForEach(
                modelBuilder.Model
                .GetEntityTypes()
                .Where(e => typeof(Entity).IsAssignableFrom(e.ClrType)),
                entityType =>
                {
                    modelBuilder.Entity(entityType.ClrType)
                                .Property<EntityId>("Id")
                                .HasConversion(c => c.Id, d => Lipar.Core.Domain.Entities.EntityId.FromGuid(d));
                    modelBuilder.Entity(entityType.ClrType)
                                .HasKey("Id")
                                .IsClustered(false);

                    modelBuilder.Entity(entityType.ClrType)
                                .Property<DateTime>(CreatedDate)
                                .IsRequired();
                    modelBuilder.Entity(entityType.ClrType)
                                .HasIndex(CreatedDate)
                                .IsUnique()
                                .IsClustered(true);
                });
        }

        public static void AddAuditableProperties(this ModelBuilder modelBuilder)
        {
            Parallel.ForEach(
                modelBuilder.Model
                .GetEntityTypes()
                .Where(e => typeof(IAuditable).IsAssignableFrom(e.ClrType)),
                entityType =>
                {
                    modelBuilder.Entity(entityType.ClrType)
                                .Property<Guid?>(CreatedBy);
                    modelBuilder.Entity(entityType.ClrType)
                                .Property<DateTime?>(ModifedDate);
                    modelBuilder.Entity(entityType.ClrType)
                                .Property<Guid?>(ModifedBy);
                });
        }


        public static readonly string EntityId = "Id";

        public static readonly string CreatedDate = nameof(CreatedDate);

        public static readonly string ModifedDate = nameof(ModifedDate);

        public static readonly string CreatedBy = nameof(CreatedBy);

        public static readonly string ModifedBy = nameof(ModifedBy);
    }
}
