using Lipar.Core.Domain.Entities;
using Lipar.Core.Domain.Events;
using System.Collections.Generic;

namespace Lipar.Core.Contract.Data;

public interface IOutBoxEventRepository
{
    List<OutBoxEvent> GetOutBoxEventItemsForPublish(int maxCount);
    void MarkAsRead(List<OutBoxEvent> outBoxEventItems);
    void AddOutboxEvetItems(List<AggregateRoot> outBoxEventItems);
}


