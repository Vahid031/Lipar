using Lipar.Core.Domain.Events;
using System.Collections.Generic;

namespace Lipar.Core.Contract.Data
{
    public interface IEntityChangesInterceptorRepository
    {
        void AddEntityChanges(IEnumerable<EntityChangesInterception> entities);
    }
}
