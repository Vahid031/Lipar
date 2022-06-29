using Lipar.Core.Domain.Events;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Collections.Generic;

namespace Lipar.Infrastructure.Data.SqlServer.EntityChangeInterceptor.Configs
{
    public class EntityChangesInterceptionConfiguration : IEntityTypeConfiguration<EntityChangesInterception>
    {
        public void Configure(EntityTypeBuilder<EntityChangesInterception> builder)
        {
            builder.ToTable($"_{nameof(EntityChangesInterception)}s");

            builder.Property(m => m.EntityType)
                .HasMaxLength(50);

            builder.Property(m => m.State)
                .HasMaxLength(10);

            builder.Property<string>("Payload")
                .HasColumnType("nvarchar(max)");

            builder.Ignore(m => m.Details);

            builder.HasIndex(m => m.Date)
                .IsUnique()
                .IsClustered(true);

            builder.HasKey(m => m.Id)
                .IsClustered(false);
        }

        private Dictionary<string, object> ToDictionary(List<EntityChangesInterceptionDetail> list)
        {
            var details = new Dictionary<string, object>();
            foreach (var item in list)
            {
                details.Add(item.Key, item.Value);
            }

            return details;
        }
    }
}
