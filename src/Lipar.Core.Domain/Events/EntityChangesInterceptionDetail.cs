using System;

namespace Lipar.Core.Domain.Events;

public class EntityChangesInterceptionDetail
{
public Guid Id { get; private set; }
public string Key { get; private set; }
public string Value { get; private set; }
    
private EntityChangesInterceptionDetail() { }
    
    public EntityChangesInterceptionDetail(Guid id, string key, string value)
    {
        Id = id;
        Key = key;
        Value = value;
    }
}


