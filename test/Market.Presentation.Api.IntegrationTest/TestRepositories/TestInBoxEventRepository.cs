using Lipar.Core.Contract.Data;
using Lipar.Core.Domain.Events;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Market.Presentation.Api.IntegrationTest.TestRepositories;

internal class TestInBoxEventRepository : IInBoxEventRepository
{
    private Dictionary<string, string> inBoxEvents = new();

    public bool AllowReceive(string messageId, string fromService)
    {
        return !inBoxEvents.Any(m => m.Key == fromService && m.Value == messageId);
    }

    public Task FailEventHandeling(InBoxEvent @event)
    {
        throw new System.NotImplementedException();
    }

    public Task<bool> MakeUnknownStatus(List<InBoxEvent> events)
    {
        throw new System.NotImplementedException();
    }

    public Task Receive(string messageId, string fromService)
    {
        inBoxEvents.Add(fromService, messageId);

        return Task.CompletedTask;  
    }

    public Task ReceiveNewEvent(InBoxEvent @event)
    {
        throw new System.NotImplementedException();
    }

    public Task<InBoxEvent> ScheduleIncomingEvent()
    {
        throw new System.NotImplementedException();
    }

    public Task SuccessEventHandeling(InBoxEvent @event)
    {
        throw new System.NotImplementedException();
    }
}

