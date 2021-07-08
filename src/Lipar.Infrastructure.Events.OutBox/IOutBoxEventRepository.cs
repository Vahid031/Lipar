using System.Collections.Generic;

namespace Lipar.Infrastructure.Events.OutBox
{
    public interface IOutBoxEventRepository
    {
        List<OutBoxEvent> GetOutBoxEvents();
        void ProceedEvent(OutBoxEvent outBoxEvent);

    }
}
