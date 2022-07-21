using Lipar.Core.Domain.Entities;
using Lipar.Infrastructure.Data.SqlServer.ValueConverters;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace Lipar.Infrastructure.Data.SqlServer.Extensions;

public static class ModelBuilderExtensions
{
    public static void AddEntityId(this ModelBuilder modelBuilder)
    {
        modelBuilder.Model
        .GetEntityTypes()
        .Where(e => typeof(Entity).IsAssignableFrom(e.ClrType))
        .ToList()
        .ForEach(
        entityType =>
        {
            modelBuilder.Entity(entityType.ClrType)
            .Property<EntityId>(Id)
            .HasConversion<EntityIdConverter>();
            modelBuilder.Entity(entityType.ClrType)
            .HasKey(Id)
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
        modelBuilder.Model
        .GetEntityTypes()
        .Where(e => typeof(IAuditable).IsAssignableFrom(e.ClrType))
        .ToList()
        .ForEach(
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


    public const string Id = nameof(Id);

    public const string CreatedDate = nameof(CreatedDate);

    public const string ModifedDate = nameof(ModifedDate);

    public const string CreatedBy = nameof(CreatedBy);

    public const string ModifedBy = nameof(ModifedBy);
}


