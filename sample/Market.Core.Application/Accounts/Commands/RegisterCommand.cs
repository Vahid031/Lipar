using FluentValidation;
using Lipar.Core.Contract.Common;
using Lipar.Core.Contract.Services;
using Lipar.Core.Domain.Entities;
using Market.Core.Domain.Accounts.Enums;
using Market.Infrastructure.Data.Identity.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Market.Core.Application.Accounts.Commands;

public class RegisterCommand : IRequest
{
public string FirstName { get; set; }
public string LastName { get; set; }
public string Email { get; set; }
public string UserName { get; set; }
public string Password { get; set; }
public string ConfirmPassword { get; set; }
    
    public class RegisterCommandHandler : IRequestHandler<RegisterCommand>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailService _emailService;
        private readonly string _origin;
        
        public RegisterCommandHandler(UserManager<ApplicationUser> userManager, IEmailService emailService, IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _emailService = emailService;
            _origin = httpContextAccessor.HttpContext.Request.Headers["origin"];
        }
        
        public async Task Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            var userWithSameUserName = await _userManager.FindByNameAsync(request.UserName);
            if (userWithSameUserName != null)
            {
            throw new ApplicationException($"Username '{request.UserName}' is already taken.");
            }
            var user = new ApplicationUser
            {
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                UserName = request.UserName
            };
            var userWithSameEmail = await _userManager.FindByEmailAsync(request.Email);
            if (userWithSameEmail == null)
            {
                var result = await _userManager.CreateAsync(user, request.Password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, Roles.Basic.ToString());
                    var verificationUri = await SendVerificationEmail(user, _origin);
                    //TODO: Attach Email Service here and configure it via appsettings
                await _emailService.SendAsync(new EmailRequest { To = user.Email, Body = $"Please confirm your account by visiting this URL {verificationUri}", Subject = "Confirm Registration" });
                //return new Response<string>(user.Id, message: $"User Registered. Please confirm your account by visiting this URL {verificationUri}");
                }
                else
                {
                throw new ApplicationException($"{result.Errors}");
                }
            }
            else
            {
            throw new ApplicationException($"Email {request.Email } is already registered.");
            }
        }
        
        private async Task<string> SendVerificationEmail(ApplicationUser user, string origin)
        {
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            var route = "api/account/confirm-email/";
        var _enpointUri = new Uri(string.Concat($"{origin}/", route));
            var verificationUri = QueryHelpers.AddQueryString(_enpointUri.ToString(), "userId", user.Id);
            verificationUri = QueryHelpers.AddQueryString(verificationUri, "code", code);
            //Email Service Call Here
            return verificationUri;
        }
        
    }
    
    public class RegisterValidator : AbstractValidator<RegisterCommand>
    {
        public RegisterValidator()
        {
            RuleFor(m => m.FirstName)
            .NotEmpty();
            
            RuleFor(m => m.LastName)
            .NotEmpty();
            
            RuleFor(m => m.Email)
            .NotEmpty()
            .EmailAddress();
            
            RuleFor(m => m.UserName)
            .NotEmpty()
            .MinimumLength(6);
            
            RuleFor(m => m.Password)
            .NotEmpty()
            .MinimumLength(6);
            
            RuleFor(m => m.ConfirmPassword)
            .NotEmpty()
            .Equal(m => m.Password);
        }
    }
}


