//using Lipar.Infrastructure.Events.OutboxEvent;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore.Metadata.Builders;

//namespace Lipar.Infrastructure.Data.SqlServer.OutBoxEvents.Configs
//{
//    public class OutBoxEventItemConfiguration : IEntityTypeConfiguration<OutBoxEventItem>
//    {
//        public void Configure(EntityTypeBuilder<OutBoxEventItem> builder)
//        {
//            builder.Property(c => c.AccuredByUserId)
//                .HasMaxLength(40);

//            builder.Property(c => c.AggregateId)
//                .HasMaxLength(40);

//            builder.Property(c => c.EventName)
//                .HasMaxLength(100);

//            builder.Property(c => c.AggregateName)
//                .HasMaxLength(200);

//            builder.Property(c => c.EventTypeName)
//                .HasMaxLength(500);

//            builder.Property(c => c.AggregateTypeName)
//                .HasMaxLength(500);

//            builder.HasKey(m => m.Id)
//                .IsClustered(false);

//            builder.HasIndex(m => m.AccuredOn)
//                .IsClustered(true)
//                .IsUnique();

//        }
//    }
//}
