using Lipar.Core.Domain.Events;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lipar.Infrastructure.Data.SqlServer.EntityChangeInterceptor.Configs
{
    public class EntityChangesInterceptorDetailConfiguration : IEntityTypeConfiguration<EntityChangesInterceptorDetail>
    {
        public void Configure(EntityTypeBuilder<EntityChangesInterceptorDetail> builder)
        {
            builder.ToTable($"_{nameof(EntityChangesInterceptorDetail)}s");

            builder.Property(m => m.Key)
                .HasMaxLength(50);

            builder.Property(m => m.Value)
                .HasMaxLength(255);
        }
    }
}
