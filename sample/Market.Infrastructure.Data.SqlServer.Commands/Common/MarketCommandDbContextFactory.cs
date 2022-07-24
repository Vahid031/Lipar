using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Market.Infrastructure.Data.SqlServer.Commands.Common;

public class MarketCommandDbContextFactory : IDesignTimeDbContextFactory<SqlServerMarketCommandDbContext>
{
    public SqlServerMarketCommandDbContext CreateDbContext(string[] args)
    {
        var builder = new DbContextOptionsBuilder<SqlServerMarketCommandDbContext>();
        builder.UseSqlServer("Server=.;Database=Market;user id=sa;password=V@hid031;");
        return new SqlServerMarketCommandDbContext(builder.Options);
    }
}
