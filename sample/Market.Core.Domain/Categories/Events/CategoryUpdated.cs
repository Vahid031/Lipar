using Lipar.Core.Domain.Events;
using System;

namespace Market.Core.Domain.Categories.Events
{
    public class CategoryUpdated : IEvent
    {
        public Guid Id { get; }
        public string Name { get; }
        public Guid? ParentId { get; }

        private CategoryUpdated() { }

        public CategoryUpdated(Guid id, string name, Guid? parentId)
        {
            Id = id;
            Name = name;
            ParentId = parentId;
        }
    }
}
