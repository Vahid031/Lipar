using Lipar.Infrastructure.Data.SqlServer.Commands;
using Microsoft.EntityFrameworkCore;

namespace Market.Infrastructure.Data.SqlServer.Commands.Common;

public class MarketCommandDbContext : BaseCommandDbContext
{
    #region Create and Configuration
    public MarketCommandDbContext(DbContextOptions<MarketCommandDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);

        base.OnModelCreating(modelBuilder);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        //optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=Market;Trusted_Connection=True;");
        base.OnConfiguring(optionsBuilder);
    }
    #endregion
}
