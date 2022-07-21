using System;

namespace Lipar.Core.Contract.Services;

public interface IUserInfoService
{
    Guid UserId { get; }
    string UserAgent { get; }
    string UserName { get; }
    string IpAddress { get; }
    string FirstName { get; }
    string LastName { get; }
}


