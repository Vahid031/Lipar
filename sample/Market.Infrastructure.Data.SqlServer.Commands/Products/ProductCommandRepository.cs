using Lipar.Infrastructure.Data.SqlServer.Commands;
using Market.Core.Domain.Products.Entities;
using Market.Core.Domain.Products.Repositories;
using Market.Infrastructure.Data.SqlServer.Commands.Common;

namespace Market.Infrastructure.Data.SqlServer.Commands.Products
{
    public class ProductCommandRepository : BaseCommandRepository<Product, MarketCommandDbContext>,
        IProductCommandRepository
    {
        public ProductCommandRepository(MarketCommandDbContext db) : base(db)
        {
        }
    }
}
