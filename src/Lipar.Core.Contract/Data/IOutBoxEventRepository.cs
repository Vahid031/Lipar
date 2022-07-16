using Lipar.Core.Domain.Entities;
using Lipar.Core.Domain.Events;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lipar.Core.Contract.Data;

public interface IOutBoxEventRepository
{
    Task<List<OutBoxEvent>> GetOutBoxEventItemsForPublish(int maxCount);
    Task MarkAsRead(List<OutBoxEvent> outBoxEventItems);
    Task AddOutboxEvetItems(List<AggregateRoot> outBoxEventItems);
}


