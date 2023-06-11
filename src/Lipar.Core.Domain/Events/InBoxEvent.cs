using System;

namespace Lipar.Core.Domain.Events;

public class InBoxEvent
{
    public string MessageId { get; set; }
    public string OwnerService { get; set; }
    public string Paload { get; set; }
    public string TypeName { get; set; }
    public InBoxEventStatus Status { get; set; }
    public int RetryCount { get; set; }
    public DateTime ReceivedAt { get; set; }
}

public enum InBoxEventStatus
{
    Unknown = -1,
    Scheduled = 1,
    InProcess = 2,
    Succeded = 3,
    Failed = 4,
}


