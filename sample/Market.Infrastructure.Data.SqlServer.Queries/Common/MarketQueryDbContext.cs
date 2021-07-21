using Lipar.Infrastructure.Data.SqlServer.Queries;
using Microsoft.EntityFrameworkCore;

namespace Market.Infrastructure.Data.SqlServerQuery.Common
{
    public class MarketQueryDbContext : BaseQueryDbContext
    {
        public MarketQueryDbContext(DbContextOptions options) : base(options)
        {
        }
    }
}
