using System.Collections.Generic;

namespace Lipar.Infrastructure.Events.OutboxEvent
{
    public interface IOutBoxEventRepository
    {
        List<OutBoxEventItem> GetOutBoxEventItemsForPublish(int maxCount);
        void MarkAsRead(List<OutBoxEventItem> outBoxEventItems);
    }
}
