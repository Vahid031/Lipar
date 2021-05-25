using Lipar.Infrastructure.Data.SqlServer.EntityChangeInterceptors.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lipar.Infrastructure.Data.SqlServer.EntityChangeInterceptors.Configs
{
    public class EntityChangeLogConfiguration : IEntityTypeConfiguration<EntityChangeLog>
    {
        public void Configure(EntityTypeBuilder<EntityChangeLog> builder)
        {
            builder.Property(m => m.EntityId).HasMaxLength(25);
            builder.Property(m => m.EntityType).HasMaxLength(50);
            builder.Property(m => m.State).HasMaxLength(25);
            builder.Property(m => m.UserId).HasMaxLength(25);
        }
    }
}
