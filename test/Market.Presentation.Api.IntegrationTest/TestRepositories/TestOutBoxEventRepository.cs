using Lipar.Core.Contract.Data;
using Lipar.Core.Domain.Entities;
using Lipar.Core.Domain.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Market.Presentation.Api.IntegrationTest.TestRepositories;

internal class TestOutBoxEventRepository : IOutBoxEventRepository
{
    private  List<OutBoxEvent> outBoxEvents;
    public TestOutBoxEventRepository()
    {
        outBoxEvents = new List<OutBoxEvent>();
    }
    public Task AddOutboxEvetItems(List<AggregateRoot> outBoxEventItems)
    {
        outBoxEvents.AddRange(outBoxEvents);

        return Task.CompletedTask;
    }

    public Task<List<OutBoxEvent>> GetOutBoxEventItemsForPublish(int maxCount)
    {
        return Task.FromResult(outBoxEvents.Where(m => !m.IsProcessed).ToList());
    }

    public Task MarkAsRead(List<OutBoxEvent> outBoxEventItems)
    {
        outBoxEvents
            .Where(m => outBoxEventItems.Select(d => d.Id).Contains(m.Id))
            .ToList()
            .ForEach(m =>
            {
                m.IsProcessed = true;
            });

        return Task.CompletedTask;
    }
}
