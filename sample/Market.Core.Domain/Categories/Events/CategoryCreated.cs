using Lipar.Core.Domain.Events;
using System;

namespace Market.Core.Domain.Categories.Events;

[EventTopic("MarketService.CategoryCreated")]
public class CategoryCreated : IEvent
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
