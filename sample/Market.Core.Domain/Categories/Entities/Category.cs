using Lipar.Core.Domain.Entities;
using Market.Core.Domain.Categories.Events;

namespace Market.Core.Domain.Categories.Entities;

public class Category : AggregateRoot, IAuditable
{
    public string Name { get; private set; }
    public EntityId ParentId { get; private set; }

    private Category() { }

    public Category(EntityId id, string name, EntityId parentId)
    {
        Id = id;
        Name = name;
        ParentId = parentId;

        Apply(new CategoryCreated(Id.ToString(), Name, ParentId?.Value));
    }

    public void Update(string name, EntityId parentId)
    {
        Name = name;
        ParentId = parentId;

        Apply(new CategoryUpdated(Id.ToString(), Name, ParentId?.Value));
    }

    public void Delete()
    {
        Apply(new CategoryDeleted(Id.ToString()));
    }
}
