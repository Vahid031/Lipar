using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using Market.Core.Domain.Accounts.Enums;

namespace Market.Infrastructure.Data.Identity.Seeds;

public static class DefaultRole
{
    public static async Task SeedAsync(RoleManager<IdentityRole> roleManager)
    {
        //Seed Roles
        await roleManager.CreateAsync(new IdentityRole(Roles.SuperAdmin.ToString()));
        await roleManager.CreateAsync(new IdentityRole(Roles.Admin.ToString()));
        await roleManager.CreateAsync(new IdentityRole(Roles.Moderator.ToString()));
        await roleManager.CreateAsync(new IdentityRole(Roles.Basic.ToString()));
    }
}


