﻿using Lipar.Core.Contract.Common;
using Lipar.Core.Domain.Queries;
using Market.Core.Domain.Products.Contracts;
using Market.Core.Domain.Products.QueryResults;
using System.Threading;
using System.Threading.Tasks;

namespace Market.Core.Application.Products.Queries;

public class GetProductQuery : PageQuery, IGetProduct, IRequest<PagedData<GetProduct>>
{
    public string Name { get; init; }
    public string Barcode { get; init; }

    public class GetProductQueryHandler : IRequestHandler<GetProductQuery, PagedData<GetProduct>>
    {
        private readonly IProductQueryRepository _repository;

        public GetProductQueryHandler(IProductQueryRepository repository)
        {
            _repository = repository;
        }

        public async Task<PagedData<GetProduct>> Handle(GetProductQuery request, CancellationToken cancellationToken = default)
        {
            return await _repository.Select(request);
        }
    }
}

