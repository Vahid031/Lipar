using System;

namespace Lipar.Core.Contract.Services;

public interface IUserInfoService
{
    int UserId { get; }
    string UserAgent { get; }
    string UserName { get; }
    string IpAddress { get; }
    string FirstName { get; }
    string LastName { get; }
}


