using FluentValidation;
using Lipar.Core.Contract.Common;
using Market.Infrastructure.Data.Identity.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Market.Core.Application.Accounts.Commands;

public class ResetPasswordCommand : IRequest
{
    public string Email { get; set; }
    public string Token { get; set; }
    public string Password { get; set; }
    public string ConfirmPassword { get; set; }

    public class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand>
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public ResetPasswordCommandHandler(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
        {
            var account = await _userManager.FindByEmailAsync(request.Email);
            if (account == null)
                throw new ApplicationException($"No Accounts Registered with {request.Email}.");

            var result = await _userManager.ResetPasswordAsync(account, request.Token, request.Password);

            if (!result.Succeeded)
                throw new ApplicationException($"Error occured while reseting the password.");

        }
    }

    public class ResetPasswordValidator : AbstractValidator<ResetPasswordCommand>
    {
        public ResetPasswordValidator()
        {
            RuleFor(m => m.Email)
            .NotEmpty()
            .EmailAddress();

            RuleFor(m => m.Token)
            .NotEmpty();

            RuleFor(m => m.Password)
            .NotEmpty()
            .MinimumLength(6);

            RuleFor(m => m.ConfirmPassword)
            .NotEmpty()
            .Equal(m => m.Password);

        }
    }
}


