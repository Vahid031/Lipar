using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Market.Infrastructure.Data.SqlServer.Commands.Common
{
    public class MarketCommandDbContextFactory : IDesignTimeDbContextFactory<MarketCommandDbContext>
    {
        public MarketCommandDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<MarketCommandDbContext>();
            builder.UseSqlServer("Server=.;Database=Market;user id=sa;password=V@hid031;");
            return new MarketCommandDbContext(builder.Options);
        }
    }
}