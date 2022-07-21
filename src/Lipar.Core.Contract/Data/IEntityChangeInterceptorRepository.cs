using Lipar.Core.Domain.Events;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lipar.Core.Contract.Data;

public interface IEntityChangesInterceptorRepository
{
    Task AddEntityChanges(IEnumerable<EntityChangesInterception> entities);
}


