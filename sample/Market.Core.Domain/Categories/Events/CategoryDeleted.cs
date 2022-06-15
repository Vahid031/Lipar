using Lipar.Core.Domain.Events;
using System;

namespace Market.Core.Domain.Categories.Events
{
    public class CategoryDeleted : IEvent
    {
        public Guid Id { get; }

        private CategoryDeleted() { }

        public CategoryDeleted(Guid id)
        {
            Id = id;
        }
    }
}
