using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lipar.Core.DomainModels.Entities
{
    public abstract class Entity
    {
        public AggregateId Id { get; protected set; }
        protected Entity() { }
    }
}
