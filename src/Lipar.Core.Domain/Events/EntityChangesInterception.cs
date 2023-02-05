using System;
using System.Collections.Generic;
using System.Linq;

namespace Lipar.Core.Domain.Events;

public class EntityChangesInterception
{
    public Guid Id { get; private set; }
    public string EntityType { get; private set; }
    public Guid EntityId { get; private set; }
    public string State { get; private set; }
    public DateTime Date { get; private set; }
    public int UserId { get; private set; }
    public IReadOnlyCollection<EntityChangesInterceptionDetail> Details => _details.ToList();
    private HashSet<EntityChangesInterceptionDetail> _details { get; set; } = new HashSet<EntityChangesInterceptionDetail>();

    private EntityChangesInterception() { }

    public EntityChangesInterception(Guid id, string entityType, Guid entityId, string state)
    {
        Id = id;
        EntityType = entityType;
        EntityId = entityId;
        State = state;
    }

    public void SetDateTime(DateTime date)
    {
        Date = date;
    }

    public void SetUserId(int userId)
    {
        UserId = userId;
    }

    public void AddDetail(string key, string value) =>
    _details.Add(new EntityChangesInterceptionDetail(Guid.NewGuid(), key, value));
}


