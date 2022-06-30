using System.Threading;
using System.Threading.Tasks;

namespace Lipar.Core.Contract.Common;

public interface IMediator
{
    Task Send<TRequest>(in TRequest request, CancellationToken cancellationToken = default) where TRequest : IRequest;
    Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default);
}


