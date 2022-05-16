using Lipar.Core.Application.Common;
using Lipar.Core.Contract.Common;
using Lipar.Core.Domain.Queries;
using Market.Core.Domain.Products.Queries;
using Market.Core.Domain.Products.Repositories;
using System.Threading;
using System.Threading.Tasks;

namespace Market.Core.Application.Products.Queries
{
    public class GetProductQuery : ProductVM, IRequest<PagedData<ProductDto>>
    {
        public class GetProductQueryHandler : IRequestHandler<GetProductQuery, PagedData<ProductDto>>
        {
            private readonly IProductQueryRepository repository;

            public GetProductQueryHandler(IProductQueryRepository repository)
            {
                this.repository = repository;
            }

            public async Task<PagedData<ProductDto>> Handle(GetProductQuery request, CancellationToken cancellationToken = default)
            {
                return await repository.Select(request);
            }
        }
    }

}