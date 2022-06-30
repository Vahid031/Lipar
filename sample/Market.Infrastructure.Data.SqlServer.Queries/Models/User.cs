using System;
using System.Collections.Generic;

#nullable disable

namespace Market.Infrastructure.Data.SqlServer.Queries.Models;

public partial class User
{
    public User()
    {
        RefreshTokens = new HashSet<RefreshToken>();
        UserClaims = new HashSet<UserClaim>();
        UserLogins = new HashSet<UserLogin>();
        UserRoles = new HashSet<UserRole>();
        UserTokens = new HashSet<UserToken>();
    }
    
public string Id { get; set; }
public string FirstName { get; set; }
public string LastName { get; set; }
public string UserName { get; set; }
public string NormalizedUserName { get; set; }
public string Email { get; set; }
public string NormalizedEmail { get; set; }
public bool EmailConfirmed { get; set; }
public string PasswordHash { get; set; }
public string SecurityStamp { get; set; }
public string ConcurrencyStamp { get; set; }
public string PhoneNumber { get; set; }
public bool PhoneNumberConfirmed { get; set; }
public bool TwoFactorEnabled { get; set; }
public DateTimeOffset? LockoutEnd { get; set; }
public bool LockoutEnabled { get; set; }
public int AccessFailedCount { get; set; }
    
public virtual ICollection<RefreshToken> RefreshTokens { get; set; }
public virtual ICollection<UserClaim> UserClaims { get; set; }
public virtual ICollection<UserLogin> UserLogins { get; set; }
public virtual ICollection<UserRole> UserRoles { get; set; }
public virtual ICollection<UserToken> UserTokens { get; set; }
}


