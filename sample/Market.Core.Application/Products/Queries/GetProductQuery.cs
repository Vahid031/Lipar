using Lipar.Core.Contract.Common;
using Lipar.Core.Domain.Queries;
using Market.Core.Domain.Products.Contracts;
using Market.Core.Domain.Products.QueryResults;
using System.Threading;
using System.Threading.Tasks;

namespace Market.Core.Application.Products.Queries
{
    public class GetProductQuery : PageQuery, IProductDto, IRequest<PagedData<ProductDto>>
    {
        public string Name { get; set; }
        public string Barcode { get; set; }

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