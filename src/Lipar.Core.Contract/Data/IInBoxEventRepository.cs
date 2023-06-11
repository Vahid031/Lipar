using Lipar.Core.Domain.Events;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lipar.Core.Contract.Data;

public interface IInBoxEventRepository
{
    Task ReceiveNewEvent(InBoxEvent @event);
    Task<InBoxEvent> ScheduleIncomingEvent();
    Task SuccessEventHandeling(InBoxEvent @event);
    Task FailEventHandeling(InBoxEvent @event);
    Task<bool> MakeUnknownStatus(List<InBoxEvent> events);
}


