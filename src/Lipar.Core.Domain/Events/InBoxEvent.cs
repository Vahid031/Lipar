using System;

namespace Lipar.Core.Domain.Events;

public class InBoxEvent
{
    public Guid Id { get; set; }
    public string MessageId { get; set; }
    public string OwnerService { get; set; }
    public DateTime ReceivedDate { get; set; }
}


