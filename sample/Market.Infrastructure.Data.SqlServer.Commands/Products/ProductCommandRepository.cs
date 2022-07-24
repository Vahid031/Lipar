using Lipar.Infrastructure.Data.SqlServer.Commands;
using Market.Core.Domain.Products.Contracts;
using Market.Core.Domain.Products.Entities;
using Market.Infrastructure.Data.SqlServer.Commands.Common;

namespace Market.Infrastructure.Data.SqlServer.Commands.Products;

public class ProductCommandRepository : BaseCommandRepository<Product, SqlServerMarketCommandDbContext>,
    IProductCommandRepository
{
    public ProductCommandRepository(SqlServerMarketCommandDbContext db) : base(db)
    {
    }
}
