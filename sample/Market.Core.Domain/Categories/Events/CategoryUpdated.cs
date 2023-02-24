using Lipar.Core.Domain.Events;
using System;

namespace Market.Core.Domain.Categories.Events;

public class CategoryUpdated : IDomainEvent
{
    public string Id { get; }
    public string Name { get; }
    public Guid? ParentId { get; }

    private CategoryUpdated() { }

    public CategoryUpdated(string id, string name, Guid? parentId)
    {
        Id = id;
        Name = name;
        ParentId = parentId;
    }
}
