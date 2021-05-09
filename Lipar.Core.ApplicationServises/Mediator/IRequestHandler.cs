using System.Threading;
using System.Threading.Tasks;

namespace Lipar.Core.ApplicationServises.Mediator
{
    public interface IRequestHandler<TRequest> where TRequest : IRequest
    {
        Task Handle(TRequest request, CancellationToken cancellationToken = default);
    }

    public interface IRequestHandler<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken = default);
    }
}
