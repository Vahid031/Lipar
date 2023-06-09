using System;

namespace Lipar.Core.Domain.Events;

public class InBoxEvent
{
    public Guid Id { get; set; }
    public string OwnerService { get; set; }
    public string Paload { get; set; }
    public InBoxEventStatus Status { get; set; }
    public int RetryCount { get; set; }
    public DateTime ReceivedAt { get; set; }
}

public enum InBoxEventStatus
{
    Unknown = 0,
    Received = 1,
    Scheduled = 2,
    InProcess = 3,
    Succeded = 4,
    Failed = 5,
}


