using Lipar.Core.Application.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;

namespace Lipar.Presentation.Api.Controllers
{
    //[Route("api/v{version:apiVersion}/[controller]")]
    public abstract class BaseController : ControllerBase
    {
        private IMediator _mediator;

        protected IMediator mediator => _mediator ??= HttpContext.RequestServices.GetService<IMediator>();

        protected async Task<IActionResult> Execute<TResponse>(IRequest<TResponse> command) where TResponse : notnull
        {
            var result = await mediator.Send(command);
            if (result is null)
            {
                return NoContent();
            }
            return Ok(result);
        }

        protected async Task<IActionResult> Execute<TRequest>(TRequest command) where TRequest : IRequest
        {
            await mediator.Send(command);
            return Ok();
        }
    }
}