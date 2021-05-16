using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Lipar.Infrastructure.Tools.Utilities.Services;
using Microsoft.AspNetCore.Http;

namespace Lipar.Presentation.Api.Services
{
    public class UserInfo : IUserInfo
    {
        private readonly HttpContext context;
        public UserInfo(IHttpContextAccessor httpContextAccessor)
        {
            context = httpContextAccessor.HttpContext;
        }

        Guid IUserInfo.UserId => Guid.Parse(context.User?.GetClaim(ClaimTypes.NameIdentifier));

        public string UserAgent => context.Request.Headers["User-Agent"];

        public string IpAddress => context.Connection.RemoteIpAddress.ToString();

        public string FirstName => context.User?.GetClaim(ClaimTypes.GivenName);

        public string LastName => context.User?.GetClaim(ClaimTypes.Surname);

        public string UserName => context.User?.GetClaim(ClaimTypes.Name);
    }
}
