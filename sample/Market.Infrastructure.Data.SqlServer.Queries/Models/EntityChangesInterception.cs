using System;
using System.Collections.Generic;

#nullable disable

namespace Market.Infrastructure.Data.SqlServer.Queries.Models;

public partial class EntityChangesInterception
{
public Guid Id { get; set; }
public string EntityType { get; set; }
public Guid EntityId { get; set; }
public string State { get; set; }
public DateTime Date { get; set; }
public Guid UserId { get; set; }
public string Payload { get; set; }
}


