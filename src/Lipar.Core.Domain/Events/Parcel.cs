using System.Collections.Generic;

namespace Lipar.Core.Domain.Events;

public class Parcel
{
    public string MessageId { get; set; }
    public string ServiceId { get; set; }
    public string MessageName { get; set; }
    public string MessageBody { get; set; }
    public string Topic { get; set; }
    public Dictionary<string, object> Headers { get; set; }
}


