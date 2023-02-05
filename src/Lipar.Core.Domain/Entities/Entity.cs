using System;

namespace Lipar.Core.Domain.Entities;

public abstract class Entity
{
    public EntityId Id { get; protected set; }
    public int CreatedBy { get; protected set; }
    public DateTime CreatedOn { get; protected set; }
    public int? ModifiedBy { get; protected set; }
    public DateTime? ModifiedOn { get; protected set; }
    protected Entity() { }

    public void SetCreateEntityDetails(int createdBy, DateTime createdOn)
    {
        CreatedBy = createdBy;
        CreatedOn = createdOn;
    }

    public void SetModifyEntityDetails(int modifiedBy, DateTime modifiedOn)
    {
        ModifiedBy = modifiedBy;
        ModifiedOn = modifiedOn;
    }
}


