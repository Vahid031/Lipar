using Lipar.Core.Domain.Events;
using System;

namespace Market.Core.Domain.Categories.Events;

public class CategoryDeleted : IEvent
{
    public string Id { get; }

    private CategoryDeleted() { }

    public CategoryDeleted(string id)
    {
        Id = id;
    }
}
