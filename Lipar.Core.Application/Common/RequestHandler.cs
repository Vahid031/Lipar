using System.Threading;
using System.Threading.Tasks;

namespace Lipar.Core.Application.Common
{
    public abstract class RequestHandler<TRequest, TResponse> : IRequestHandler<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        public abstract Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken = default);
    }

    public abstract class RequestHandler<TRequest> : IRequestHandler<TRequest> where TRequest : IRequest
    {
        public abstract Task Handle(TRequest request, CancellationToken cancellationToken = default);
    }
}
