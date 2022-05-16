using Lipar.Core.Contract.Data;
using Lipar.Core.Domain.Queries;
using Market.Core.Domain.Products.Queries;
using System.Threading.Tasks;

namespace Market.Core.Domain.Products.Repositories
{
    public interface IProductQueryRepository : IQueryRepository
    {
        Task<PagedData<ProductDto>> Select(ProductVM input);
    }
}
