using Lipar.Core.Domain.Queries;
using Lipar.Infrastructure.Data.SqlServer.Queries;
using Market.Core.Domain.Products.Queries;
using Market.Core.Domain.Products.Repositories;
using Market.Infrastructure.Data.SqlServerQuery.Common;
using System.Linq;
using System.Threading.Tasks;

namespace Market.Infrastructure.Data.SqlServerQuery.Products
{
    public class ProductQueryRepository : BaseQueryRepository<MarketQueryDbContext>,
        IProductQueryRepository
    {
        public ProductQueryRepository(MarketQueryDbContext db) : base(db)
        {
        }

        public Task<PagedData<ProductDto>> Select(ProductVM input)
        {

            var result = db.Products.Select(m => new ProductDto
            {
                Id = m.Id,
                Name = m.Name,
                Barcode = m.Barcode,
            });


            return Task.FromResult(new PagedData<ProductDto>
            {
                PageNumber = input.PageNumber,
                PageSize = input.PageSize,
                TotalCount = result.Count(),
                Result = result.ToList()
            });
        }
    }
}
