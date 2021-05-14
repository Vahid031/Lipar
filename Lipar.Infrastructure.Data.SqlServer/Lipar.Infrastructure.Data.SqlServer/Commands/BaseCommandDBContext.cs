using Microsoft.EntityFrameworkCore;
using Lipar.Infrastructure.Data.SqlServer.Extensions;
using System.Threading.Tasks;

namespace Lipar.Infrastructure.Data.SqlServer.Commands
{
    public abstract class CommandDBContext : DbContext
    {
        public CommandDBContext(DbContextOptions<CommandDBContext> options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.AddEntityId();
            modelBuilder.AddAuditableProperties();

            base.OnModelCreating(modelBuilder);
        }

        public async Task<int> SaveChangesAsync()
        {
            ChangeTracker.DetectChanges();
            ChangeTracker.AutoDetectChangesEnabled = false;
            ChangeTracker.SetShadowProperties();
            var rowAffect = await base.SaveChangesAsync();
            ChangeTracker.AutoDetectChangesEnabled = true;

            return rowAffect;
        }
    }
}
