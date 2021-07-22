using System.Collections.Generic;

namespace Lipar.Infrastructure.Data.SqlServer.OutBoxEvents
{
    public interface IOutBoxEventItemRepository
    {
        List<OutBoxEventItem> GetOutBoxEventItemsForPublish(int maxCount);
        void MarkAsRead(List<OutBoxEventItem> outBoxEventItems);
    }
}
