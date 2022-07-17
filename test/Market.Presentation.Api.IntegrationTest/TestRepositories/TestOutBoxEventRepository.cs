using Lipar.Core.Contract.Data;
using Lipar.Core.Domain.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Market.Presentation.Api.IntegrationTest.TestRepositories;

internal class TestOutBoxEventRepository : IOutBoxEventRepository
{
    private List<OutBoxEvent> _outBoxEvents = new();

    public Task AddOutboxEvetItems(List<OutBoxEvent> outBoxEvents)
    {
        _outBoxEvents.AddRange(outBoxEvents);
        return Task.CompletedTask;
    }

    public Task<List<OutBoxEvent>> GetOutBoxEventItemsForPublish(int maxCount)
    {
        return Task.FromResult(_outBoxEvents.Where(m => !m.IsProcessed).ToList());
    }

    public Task MarkAsRead(List<OutBoxEvent> outBoxEventItems)
    {
        _outBoxEvents
            .Where(m => outBoxEventItems.Select(d => d.Id).Contains(m.Id))
            .ToList()
            .ForEach(m =>
            {
                m.IsProcessed = true;
            });

        return Task.CompletedTask;
    }
}
