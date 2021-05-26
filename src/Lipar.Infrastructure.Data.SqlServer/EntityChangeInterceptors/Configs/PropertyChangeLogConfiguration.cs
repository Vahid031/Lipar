using Lipar.Infrastructure.Data.SqlServer.EntityChangeInterceptors.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lipar.Infrastructure.Data.SqlServer.EntityChangeInterceptors.Configs
{
    public class PropertyChangeLogConfiguration : IEntityTypeConfiguration<PropertyChangeLog>
    {
        public void Configure(EntityTypeBuilder<PropertyChangeLog> builder)
        {
            builder.Property(m => m.Key)
                .HasMaxLength(50);

            builder.Property(m => m.Value)
                .HasMaxLength(250);
        }
    }
}
