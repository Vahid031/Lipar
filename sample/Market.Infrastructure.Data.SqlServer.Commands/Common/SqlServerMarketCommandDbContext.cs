using Lipar.Infrastructure.Data.SqlServer.Commands;
using Microsoft.EntityFrameworkCore;

namespace Market.Infrastructure.Data.SqlServer.Commands.Common;

public class SqlServerMarketCommandDbContext : BaseCommandDbContext
{
    #region Create and Configuration
    public SqlServerMarketCommandDbContext(DbContextOptions<SqlServerMarketCommandDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);

        base.OnModelCreating(modelBuilder);
    }
    #endregion
}
