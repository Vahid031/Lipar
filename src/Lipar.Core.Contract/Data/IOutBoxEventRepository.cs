using Lipar.Core.Domain.Entities;
using Lipar.Core.Domain.Events;
using System.Collections.Generic;

namespace Lipar.Core.Contract.Data
{
    public interface IOutBoxEventRepository
    {
        List<OutBoxEventItem> GetOutBoxEventItemsForPublish(int maxCount);
        void MarkAsRead(List<OutBoxEventItem> outBoxEventItems);
        void AddOutboxEvetItems(List<AggregateRoot> outBoxEventItems);
    }
}
