using System;
using System.Collections.Generic;

#nullable disable

namespace Market.Infrastructure.Data.SqlServer.Queries.Models;

public partial class RefreshToken
{
public int Id { get; set; }
public string Token { get; set; }
public DateTime Expires { get; set; }
public DateTime Created { get; set; }
public string CreatedByIp { get; set; }
public DateTime? Revoked { get; set; }
public string RevokedByIp { get; set; }
public string ReplacedByToken { get; set; }
public string ApplicationUserId { get; set; }
    
public virtual User ApplicationUser { get; set; }
}


