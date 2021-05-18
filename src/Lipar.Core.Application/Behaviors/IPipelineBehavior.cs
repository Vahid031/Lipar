using Lipar.Core.Application.Common;
using System.Threading;
using System.Threading.Tasks;

namespace Lipar.Core.Application.Behaviors
{
    public interface IPipelineBehavior<in TRequest> where TRequest : IRequest
    {
        Task Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate next);
    }

    public interface IPipelineBehavior<in TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next);
    }
}
