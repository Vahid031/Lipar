using Lipar.Infrastructure.Data.SqlServer.Queries;
using Microsoft.EntityFrameworkCore;

namespace Market.Infrastructure.Data.SqlServerQuery.Common
{
    public class MarketQueryDbContext : QueryDbContext
    {
        public MarketQueryDbContext(DbContextOptions options) : base(options)
        {
        }
    }
}
