using FluentValidation;
using Lipar.Core.Contract.Common;
using Lipar.Core.Contract.Services;
using Market.Core.Domain.Accounts.Entities;
using Market.Core.Domain.Accounts.Queries;
using Market.Infrastructure.Data.Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Security.Cryptography;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Sockets;
using Market.Infrastructure.Data.Identity.Seeds;

namespace Market.Core.Application.Accounts.Commands;

public class LoginCommand : IRequest<LoginDto>
{
    public string UserName { get; set; }
    public string Password { get; set; }

    public class LoginCommandHandler : IRequestHandler<LoginCommand, LoginDto>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly JWTSetting _JWTSetting;
        private readonly IDateTimeService _dateTimeService;
        private readonly IUserInfoService _userInfoService;

        public LoginCommandHandler(UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager,
        SignInManager<ApplicationUser> signInManager,
        IOptions<JWTSetting> options,
        IDateTimeService dateTimeService,
        IUserInfoService userInfoService)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _JWTSetting = options.Value;
            _dateTimeService = dateTimeService;
            _userInfoService = userInfoService;
        }

        public async Task<LoginDto> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            //await DefaultRole.SeedAsync(_roleManager);
            //await DefaultUser.SeedAsync(_userManager);
            var user = await _userManager.FindByNameAsync(request.UserName);
            if (user == null)
            {
                throw new ApplicationException($"No Accounts Registered with {request.UserName}.");
            }
            var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, lockoutOnFailure: false);
            if (!result.Succeeded)
            {
                throw new ApplicationException($"Invalid Credentials for '{request.UserName}'.");
            }
            if (!user.EmailConfirmed)
            {
                throw new ApplicationException($"Account Not Confirmed for '{request.UserName}'.");
            }

            JwtSecurityToken jwtSecurityToken = await GenerateJWToken(user);
            var rolesList = await _userManager.GetRolesAsync(user).ConfigureAwait(false);

            var response = new LoginDto
            {
                Id = user.Id,
                AccessToken = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
                Email = user.Email,
                UserName = user.UserName,
                Roles = rolesList.ToList(),
                IsVerified = user.EmailConfirmed,
                RefreshToken = GenerateRefreshToken(_userInfoService.IpAddress).Token,

            };
            return response;
        }

        private async Task<JwtSecurityToken> GenerateJWToken(ApplicationUser user)
        {
            var userClaims = await _userManager.GetClaimsAsync(user);
            var roles = await _userManager.GetRolesAsync(user);

            var roleClaims = new List<Claim>();

            for (int i = 0; i < roles.Count; i++)
            {
                roleClaims.Add(new Claim("roles", roles[i]));
            }

            string ipAddress = GetHostIpAddress();

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("uid", user.Id),
                new Claim("ip", ipAddress)
            }
            .Union(userClaims)
            .Union(roleClaims);

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_JWTSetting.Key));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(
            issuer: _JWTSetting.Issuer,
            audience: _JWTSetting.Audience,
            claims: claims,
            expires: _dateTimeService.Now.AddMinutes(30),
            signingCredentials: signingCredentials);
            return jwtSecurityToken;
        }

        private RefreshToken GenerateRefreshToken(string ipAddress)
        {
            return new RefreshToken
            {
                Token = RandomTokenString(),
                Expires = _dateTimeService.Now.AddDays(7),
                Created = _dateTimeService.Now,
                CreatedByIp = ipAddress
            };
        }

        private string RandomTokenString()
        {
            using var rngCryptoServiceProvider = RandomNumberGenerator.Create();
            var randomBytes = new byte[40];
            rngCryptoServiceProvider.GetBytes(randomBytes);
            // convert random bytes to hex string
            return BitConverter.ToString(randomBytes).Replace("-", "");
        }

        private string GetHostIpAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            return string.Empty;
        }
    }

    public class LoginValidator : AbstractValidator<LoginCommand>
    {
        public LoginValidator()
        {
            RuleFor(m => m.UserName)
            .NotEmpty();

            RuleFor(m => m.Password)
            .NotEmpty();
        }
    }
}


