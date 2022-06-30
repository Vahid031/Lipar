using System;
using System.Collections.Generic;

#nullable disable

namespace Market.Infrastructure.Data.SqlServer.Queries.Models;

public partial class UserToken
{
public string UserId { get; set; }
public string LoginProvider { get; set; }
public string Name { get; set; }
public string Value { get; set; }
    
public virtual User User { get; set; }
}


