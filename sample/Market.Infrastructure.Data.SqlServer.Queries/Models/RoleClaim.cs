using System;
using System.Collections.Generic;

#nullable disable

namespace Market.Infrastructure.Data.SqlServer.Queries.Models;

public partial class RoleClaim
{
public int Id { get; set; }
public string RoleId { get; set; }
public string ClaimType { get; set; }
public string ClaimValue { get; set; }
    
public virtual Role Role { get; set; }
}


