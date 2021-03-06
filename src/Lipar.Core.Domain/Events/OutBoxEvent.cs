using System;

namespace Lipar.Core.Domain.Events;

public class OutBoxEvent
{
    public Guid Id { get; set; }
    public string AccuredByUserId { get; set; }
    public DateTime AccuredOn { get; set; }
    public string EventName { get; set; }
    public string EventTypeName { get; set; }
    public string EventPayload { get; set; }
    public bool IsProcessed { get; set; }
}


