using System.Threading.Tasks;
using Lipar.Presentation.Api.Controllers;
using Market.Core.Application.Accounts.Commands;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    //[ApiVersion("1.0")]
    public class AccountController : BaseController
    {

        [HttpPost("authentication")]
        public async Task<IActionResult> Authentication(AuthenticationCommand command)
        {
            var s =User.Identity.IsAuthenticated;


            return await SendAsync(command);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterCommand command)
        {
            return await SendAsync(command);
        }

        [HttpPost("confirm-email")]
        [Authorize]
        public async Task<IActionResult> ConfirmEmail(ConfirmEmailCommand command)
        {
            return await SendAsync(command);
        }

        [HttpPost("forgot-password")]
        [Authorize]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordCommand command)
        {
            return await SendAsync(command);
        }

        [HttpPost("reset-password")]
        [Authorize]
        public async Task<IActionResult> ResetPassword(ResetPasswordCommand command)
        {
            return await SendAsync(command);
        }
    }
}