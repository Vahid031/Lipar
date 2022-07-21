using Lipar.Core.Contract.Common;
using Market.Core.Domain.Products.Contracts;
using Market.Core.Domain.Products.QueryResults;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Market.Core.Application.Products.Queries;

public class GetByIdProductQuery : IGetByIdProduct, IRequest<GetByIdProduct>
{
    public Guid Id { get; init; }

    public class GetByIdProductQueryHandler : IRequestHandler<GetByIdProductQuery, GetByIdProduct>
    {
        private readonly IProductQueryRepository _repository;

        public GetByIdProductQueryHandler(IProductQueryRepository repository)
        {
            _repository = repository;
        }

        public async Task<GetByIdProduct> Handle(GetByIdProductQuery request, CancellationToken cancellationToken = default)
        {
            return await _repository.Select(request);
        }
    }
}

