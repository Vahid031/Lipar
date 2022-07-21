using Lipar.Core.Contract.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Market.Presentation.Api.IntegrationTest.TestRepositories;

internal class TestInBoxEventRepository : IInBoxEventRepository
{
    private Dictionary<string, string> inBoxEvents = new();

    public bool AllowReceive(string messageId, string fromService)
    {
        return !inBoxEvents.Any(m => m.Key == fromService && m.Value == messageId);
    }

    public Task Receive(string messageId, string fromService)
    {
        inBoxEvents.Add(fromService, messageId);

        return Task.CompletedTask;  
    }
}

