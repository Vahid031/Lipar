using Lipar.Core.Contract.Data;
using Lipar.Core.Domain.Queries;
using Market.Core.Domain.Products.QueryResults;
using System.Threading.Tasks;

namespace Market.Core.Domain.Products.Contracts
{
    public interface IProductQueryRepository : IQueryRepository
    {
        Task<PagedData<ProductDto>> Select(IProductDto input);
    }
}
