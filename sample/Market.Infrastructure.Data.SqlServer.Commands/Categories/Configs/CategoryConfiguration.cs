using Market.Core.Domain.Categories.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Market.Infrastructure.Data.SqlServer.Commands.Categories.Configs
{
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.ToTable("Categories");

            builder.Property(m => m.Name).HasMaxLength(50);
            builder.Property(m => m.ParentId)
                .HasConversion(d => d.Value, c => c);

            builder.HasMany<Category>()
                .WithOne()
                .HasForeignKey(m => m.ParentId);
        }
    }
}
