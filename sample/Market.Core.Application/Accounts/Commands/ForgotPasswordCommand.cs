using FluentValidation;
using Lipar.Core.Contract.Common;
using Lipar.Core.Contract.Services;
using Lipar.Core.Domain.Entities;
using Market.Infrastructure.Data.Identity.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Market.Core.Application.Accounts.Commands
{
    public class ForgotPasswordCommand : IRequest
    {
        public string Email { get; set; }

        public class ForgotPasswordCommandHandler : IRequestHandler<ForgotPasswordCommand>
        {
            private readonly UserManager<ApplicationUser> _userManager;
            private readonly IEmailService _emailService;
            private readonly string _origin;

            public ForgotPasswordCommandHandler(UserManager<ApplicationUser> userManager, IEmailService emailService, IHttpContextAccessor httpContextAccessor)
            {
                _userManager = userManager;
                _emailService = emailService;
                _origin = httpContextAccessor.HttpContext.Request.Headers["origin"];
            }

            public async Task Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
            {
                var account = await _userManager.FindByEmailAsync(request.Email);

                // always return ok response to prevent email enumeration
                if (account == null) return;

                var code = await _userManager.GeneratePasswordResetTokenAsync(account);
                var route = "api/account/reset-password/";
                var _enpointUri = new Uri(string.Concat($"{_origin}/", route));
                var emailRequest = new EmailRequest()
                {
                    Body = $"You reset token is - {code}",
                    To = request.Email,
                    Subject = "Reset Password",
                };
                await _emailService.SendAsync(emailRequest);
            }
        }

        public class ForgotPasswordValidator : AbstractValidator<ForgotPasswordCommand>
        {
            public ForgotPasswordValidator()
            {
                RuleFor(m => m.Email)
                    .NotEmpty()
                    .EmailAddress();

            }
        }
    }
}
