using Lipar.Core.Domain.Events;
using System;

namespace Market.Core.Domain.Categories.Events;

public class CategoryCreated : IDomainEvent
{
    public string Id { get; }
    public string Name { get; }
    public Guid? ParentId { get; }

    private CategoryCreated() { }

    public CategoryCreated(string id, string name, Guid? parentId)
    {
        Id = id;
        Name = name;
        ParentId = parentId;
    }
}
