using Lipar.Infrastructure.Data.SqlServer.Commands;
using Market.Core.Domain.Products.Entities;
using Market.Core.Domain.Products.Repositories;
using Market.Infrastructure.Data.SqlServerCommand.Common;

namespace Market.Infrastructure.Data.SqlServerCommand.Products
{
    public class ProductCommandRepository : CommandRepository<Product, MarketCommandDbContext>,
        IProductCommandRepository
    {
        public ProductCommandRepository(MarketCommandDbContext db) : base(db)
        {
        }
    }
}
