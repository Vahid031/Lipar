using Lipar.Infrastructure.Events.OutboxEvent;
using System.Collections.Generic;

namespace Lipar.Core.Contract.Data
{
    public interface IOutBoxEventRepository
    {
        List<OutBoxEventItem> GetOutBoxEventItemsForPublish(int maxCount);
        void MarkAsRead(List<OutBoxEventItem> outBoxEventItems);
    }
}
