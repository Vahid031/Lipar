using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace Lipar.Core.ApplicationServises.Mediator
{
    public class Mediator : IMediator
    {
        private readonly ServiceFactory serviceFactory;
        private static readonly ConcurrentDictionary<Type, RequestHandlerBase> requestHandlers = new();

        public Mediator(ServiceFactory serviceFactory)
        {
            this.serviceFactory = serviceFactory;
        }

        public Task Send<TRequest>(in TRequest request, CancellationToken cancellationToken = default) where TRequest : IRequest
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var handler = (RequestHandlerWrapper2<TRequest>)requestHandlers.GetOrAdd(request.GetType(),
               static t => (RequestHandlerBase)(Activator.CreateInstance(typeof(RequestHandlerWrapperImpl<TRequest>))
                ?? throw new InvalidOperationException($"Could not create wrapper type for {t}")));

            return handler.Handle(request, cancellationToken, serviceFactory);
        }

        //private object GetHandler<TRequest, TResponse>(TRequest request) where TRequest : IRequest<TResponse>
        //{

        //    return serviceProvider.GetRequiredService(handlerType);
        //}


        public Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var handler = (RequestHandlerWrapper<TResponse>)requestHandlers.GetOrAdd(request.GetType(),
               static t => (RequestHandlerBase)(Activator.CreateInstance(typeof(RequestHandlerWrapperImpl<,>).MakeGenericType(t, typeof(TResponse)))
                ?? throw new InvalidOperationException($"Could not create wrapper type for {t}")));

            return handler.Handle(request, cancellationToken, serviceFactory);
        }
    }
}