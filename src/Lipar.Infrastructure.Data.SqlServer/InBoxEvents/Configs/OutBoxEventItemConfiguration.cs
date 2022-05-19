using Lipar.Core.Domain.Events;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lipar.Infrastructure.Data.SqlServer.OutBoxEvents.Configs
{
    public class InBoxEventConfiguration : IEntityTypeConfiguration<InBoxEvent>
    {
        public void Configure(EntityTypeBuilder<InBoxEvent> builder)
        {
            builder.ToTable($"_{nameof(InBoxEvent)}s");

            builder.Property(c => c.MessageId)
                .HasMaxLength(50);

            builder.Property(c => c.OwnerService)
                .HasMaxLength(100);

            builder.HasKey(m => m.Id)
                .IsClustered(false);

            builder.HasIndex(m => m.ReceivedDate)
                .IsClustered(true)
                .IsUnique();

        }
    }
}
