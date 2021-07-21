using System.Collections.Generic;

namespace Lipar.Infrastructure.Data.SqlServer.OutBoxEvents
{
    public interface IOutBoxEventItemRepository
    {
        List<OutBoxEventItem> GetOutBoxEventItemsForPublishe(int maxCount = 100);
        void MarkAsRead(List<OutBoxEventItem> outBoxEventItems);
    }
}
