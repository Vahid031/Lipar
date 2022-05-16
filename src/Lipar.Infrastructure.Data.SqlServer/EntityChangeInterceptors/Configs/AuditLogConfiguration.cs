using Lipar.Infrastructure.Data.SqlServer.EntityChangeInterceptors.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lipar.Infrastructure.Data.SqlServer.EntityChangeInterceptors.Configs
{
    public class AuditLogConfiguration : IEntityTypeConfiguration<AuditLog>
    {
        public void Configure(EntityTypeBuilder<AuditLog> builder)
        {
            builder.Property(m => m.EntityType)
                .HasMaxLength(50);

            builder.Property(m => m.State)
                .HasMaxLength(10);

            builder.HasMany(b => b.PropertyChangeLogs)
                .WithOne().HasForeignKey(m => m.AuditLogId);

            builder.HasIndex(m => m.Date)
                .IsUnique()
                .IsClustered(true);

            builder.HasKey(m => m.Id)             
                .IsClustered(false);
        }
    }
}
