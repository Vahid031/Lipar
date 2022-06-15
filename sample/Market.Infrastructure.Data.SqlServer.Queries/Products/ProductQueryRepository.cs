using Lipar.Core.Domain.Queries;
using Lipar.Infrastructure.Data.SqlServer.Queries;
using Lipar.Infrastructure.Data.SqlServer.Extensions;
using Market.Infrastructure.Data.SqlServerQuery.Common;
using System.Linq;
using System.Threading.Tasks;
using Market.Core.Domain.Products.QueryResults;
using Market.Core.Domain.Products.Contracts;

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

            var query = db.Products.Select(m => new ProductDto
            {
                Id = m.Id,
                Name = m.Name,
                Barcode = m.Barcode,
            });

            if (!string.IsNullOrEmpty(input.Name))
                query = query.Where(m => m.Name.Contains(input.Name));


            if (!string.IsNullOrEmpty(input.Barcode))
                query = query.Where(m => m.Barcode.Contains(input.Barcode));


            return query.PagingAsync(input);
        }
    }
}
