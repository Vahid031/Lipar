using System;
using System.Collections.Generic;

#nullable disable

namespace Market.Infrastructure.Data.SqlServer.Queries.Models;

public partial class InBoxEvent
{
public Guid Id { get; set; }
public string MessageId { get; set; }
public string OwnerService { get; set; }
public DateTime ReceivedDate { get; set; }
}


