using Market.Core.Domain.Accounts.Entities;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace Market.Infrastructure.Data.Identity.Models;

public class ApplicationUser : IdentityUser
{
public string FirstName { get; set; }
public string LastName { get; set; }
public List<RefreshToken> RefreshTokens { get; set; }
    public bool OwnsToken(string token)
    {
        return RefreshTokens?.Find(x => x.Token == token) != null;
    }
}


