using Lipar.Core.Contract.Data;
using Lipar.Core.Domain.Events;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Market.Presentation.Api.IntegrationTest.TestRepositories;

internal class TestEntityChangesInterceptorRepository : IEntityChangesInterceptorRepository
{
    public Task AddEntityChanges(IEnumerable<EntityChangesInterception> entities)
    {
        return Task.CompletedTask;
    }
}
