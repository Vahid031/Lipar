using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Market.Infrastructure.Data.SqlServer.Commands.Common
{
    public class MarketCommandDbContextFactory : IDesignTimeDbContextFactory<MarketCommandDbContext>
    {
        public MarketCommandDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<MarketCommandDbContext>();
            builder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=Market;Trusted_Connection=True;");
            return new MarketCommandDbContext(builder.Options);
        }
    }
}