using Lipar.Core.Contract.Extensions;
using System;
using System.Threading.Tasks;

namespace Lipar.Core.Contract.Common;

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


