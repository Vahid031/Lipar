using Lipar.Core.Contract.Common;
using Lipar.Core.Contract.Data;
using Lipar.Core.Contract.Services;
using Lipar.Core.Domain.Events;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Lipar.Core.Application.Common;

public class Mediator : IMediator
{
    private readonly ServiceFactory serviceFactory;
    private readonly IOutBoxEventRepository _outBoxEventRepository;
    private readonly IUserInfoService _userInfoService;
    private readonly IJsonService _jsonService;
    private readonly IDateTimeService _dateTimeService;
    private static readonly ConcurrentDictionary<Type, RequestHandlerBase> requestHandlers = new();

    public Mediator(ServiceFactory serviceFactory, IOutBoxEventRepository outBoxEventRepository, IUserInfoService userInfoService, IJsonService jsonService, IDateTimeService dateTimeService)
    {
        this.serviceFactory = serviceFactory;
        _outBoxEventRepository = outBoxEventRepository;
        _userInfoService = userInfoService;
        _jsonService = jsonService;
        _dateTimeService = dateTimeService;
    }

    public Task Send<TRequest>(in TRequest request, CancellationToken cancellationToken = default) where TRequest : IRequest
    {
        if (request == null)
        {
            throw new ArgumentNullException(nameof(request));
        }

        var handler = (RequestHandlerWrapper)requestHandlers.GetOrAdd(request.GetType(),
        static t => (RequestHandlerBase)(Activator.CreateInstance(typeof(RequestHandlerWrapperImpl))
    ?? throw new InvalidOperationException($"Could not create wrapper type for {t}")));

        return handler.Handle(request, cancellationToken, serviceFactory);
    }

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

    public async Task Publish<TEvent>(TEvent @event, CancellationToken cancellationToken = default) where TEvent : IEvent
    {
        await _outBoxEventRepository.AddOutboxEvetItems(
            new OutBoxEvent
            {
                Id = Guid.NewGuid(),
                AccuredByUserId = _userInfoService.UserId.ToString(),
                AccuredOn = _dateTimeService.Now,
                EventName = @event.GetType().Name,
                EventPayload = _jsonService.SerializeObject(@event),
                EventTypeName = @event.GetType().FullName,
                IsProcessed = false
            });
    }
}


