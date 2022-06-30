using FluentValidation;
using Lipar.Core.Contract.Common;
using Market.Infrastructure.Data.Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Market.Core.Application.Accounts.Commands;

public class ConfirmEmailCommand : IRequest
{
    public string UserId { get; set; }
    public string Code { get; set; }

    public class ConfirmEmailCommandHandler : IRequestHandler<ConfirmEmailCommand>
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public ConfirmEmailCommandHandler(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task Handle(ConfirmEmailCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(request.UserId);
            var code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(request.Code));
            var result = await _userManager.ConfirmEmailAsync(user, code);

            if (!result.Succeeded)
                throw new ApplicationException($"An error occured while confirming {user.Email}.");

        }
    }

    public class ConfirmEmailValidator : AbstractValidator<ConfirmEmailCommand>
    {
        public ConfirmEmailValidator()
        {
            RuleFor(m => m.UserId)
            .NotEmpty();

            RuleFor(m => m.Code)
            .NotEmpty();
        }
    }
}


