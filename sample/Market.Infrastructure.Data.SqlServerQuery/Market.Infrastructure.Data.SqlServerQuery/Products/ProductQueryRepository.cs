using Lipar.Infrastructure.Data.SqlServer.Queries;
using Market.Core.Domain.Products.Repositories;
using Market.Infrastructure.Data.SqlServerQuery.Common;

namespace Market.Infrastructure.Data.SqlServerQuery.Products
{
    public class ProductQueryRepository : QueryRepository<MarketQueryDbContext>,
        IProductQueryRepository
    {
        public ProductQueryRepository(MarketQueryDbContext db) : base(db)
        {
        }
    }
}
