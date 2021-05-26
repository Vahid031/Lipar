using System;

namespace Lipar.Infrastructure.Tools.Utilities.Services
{
    public interface IUserInfo
    {
        Guid UserId { get; }
        string UserAgent { get; }
        string UserName { get; }
        string IpAddress { get; }
        string FirstName { get; }
        string LastName { get; }
    }
}
