using Microsoft.EntityFrameworkCore;

namespace Lipar.Infrastructure.Data.SqlServer.Commands
{
    public abstract class BaseCommandDBContext : DbContext
    {
        public BaseCommandDBContext(DbContextOptions<BaseCommandDBContext> options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
