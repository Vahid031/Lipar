using Lipar.Core.Domain.Events;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lipar.Infrastructure.Data.SqlServer.EntityChangeInterceptor.Configs
{
    public class EntityChangesInterceptorConfiguration : IEntityTypeConfiguration<EntityChangesInterceptor>
    {
        public void Configure(EntityTypeBuilder<EntityChangesInterceptor> builder)
        {
            builder.ToTable($"{nameof(EntityChangesInterceptor)}s");

            builder.Property(m => m.EntityType)
                .HasMaxLength(50);

            builder.Property(m => m.State)
                .HasMaxLength(10);

            builder.HasMany(b => b.EntityChangesInterceptorDetails)
                .WithOne().HasForeignKey(m => m.EntityChangesInterceptorId);

            builder.HasIndex(m => m.Date)
                .IsUnique()
                .IsClustered(true);

            builder.HasKey(m => m.Id)
                .IsClustered(false);
        }
    }
}
