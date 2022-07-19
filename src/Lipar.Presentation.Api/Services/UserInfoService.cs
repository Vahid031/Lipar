using System;
using System.Security.Claims;
using Lipar.Core.Contract.Services;
using Microsoft.AspNetCore.Http;

namespace Lipar.Presentation.Api.Services;

public class UserInfoService : IUserInfoService
{
    private readonly HttpContext context;
    public UserInfoService(IHttpContextAccessor httpContextAccessor)
    {
        context = httpContextAccessor.HttpContext;
    }

    public Guid UserId
    {
        get
        {
            Guid.TryParse(context.User?.GetClaim(ClaimTypes.NameIdentifier), out Guid userId);
            return userId;
        }
    }

    public string UserAgent => context.Request.Headers["User-Agent"];

    public string IpAddress => context.Connection.RemoteIpAddress.ToString();

    public string FirstName => context.User?.GetClaim(ClaimTypes.GivenName);

    public string LastName => context.User?.GetClaim(ClaimTypes.Surname);

    public string UserName => context.User?.GetClaim(ClaimTypes.Name);
}


