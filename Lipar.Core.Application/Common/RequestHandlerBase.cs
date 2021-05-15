using Lipar.Core.Application.Extensions;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Lipar.Core.Application.Common
{

    public delegate object ServiceFactory(Type serviceType);
    public delegate Task<TResponse> RequestHandlerDelegate<TResponse>();
    public delegate Task RequestHandlerDelegate();


    public abstract class RequestHandlerBase
    {
        protected static THandler GetHandler<THandler>(ServiceFactory factory)
        {
            THandler handler = factory.GetInstance<THandler>();

            if (handler is null)
                throw new InvalidOperationException($"Handler was not found for request of type {typeof(THandler)}. Register your handlers with the container. See the samples in GitHub for examples.");

            return handler;
        }
    }

    public abstract class RequestHandlerWrapper : RequestHandlerBase
    {
        public abstract Task Handle<TRequest>(TRequest request, CancellationToken cancellationToken,
            ServiceFactory serviceFactory) where TRequest : IRequest;
    }

    public abstract class RequestHandlerWrapper<TResponse> : RequestHandlerBase
         where TResponse : notnull
    {
        public abstract Task<TResponse> Handle(IRequest<TResponse> request, CancellationToken cancellationToken,
            ServiceFactory serviceFactory);
    }



    public class RequestHandlerWrapperImpl<TRequest, TResponse> : RequestHandlerWrapper<TResponse>
       where TRequest : IRequest<TResponse>
    {
        public override Task<TResponse> Handle(IRequest<TResponse> request, CancellationToken cancellationToken,
            ServiceFactory serviceFactory)
        {
            Task<TResponse> Handler() => GetHandler<IRequestHandler<TRequest, TResponse>>(serviceFactory).Handle((TRequest)request, cancellationToken);

            return serviceFactory
                .GetInstances<IPipelineBehavior<TRequest, TResponse>>()
                .Reverse()
                .Aggregate((RequestHandlerDelegate<TResponse>)Handler, (next, pipeline) => () => pipeline.Handle((TRequest)request, cancellationToken, next))();
        }

    }

    public class RequestHandlerWrapperImpl : RequestHandlerWrapper
    {
        public override Task Handle<TRequest>(TRequest request, CancellationToken cancellationToken,
            ServiceFactory serviceFactory)
        {
            Task Handler() => GetHandler<IRequestHandler<TRequest>>(serviceFactory).Handle(request, cancellationToken);

            return serviceFactory
                .GetInstances<IPipelineBehavior<TRequest>>()
                .Reverse()
                .Aggregate((RequestHandlerDelegate)Handler, (next, pipeline) => () => pipeline.Handle(request, cancellationToken, next))();
        }
    }



}
