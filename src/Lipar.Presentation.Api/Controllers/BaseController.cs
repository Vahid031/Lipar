using Lipar.Core.Contract.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Threading.Tasks;

namespace Lipar.Presentation.Api.Controllers;

[ApiController]
//[Authorize]
[Route("api/v{version:apiVersion}/[controller]/[action]")]
public abstract class BaseController : ControllerBase
{
    private IMediator _mediator;

    protected IMediator mediator => _mediator ??= HttpContext.RequestServices.GetService<IMediator>();

    protected async Task<IActionResult> SendAsync<TResponse>(IRequest<TResponse> command,
    HttpStatusCode statusCode = HttpStatusCode.OK) where TResponse : notnull
    {
        var result = await mediator.Send(command);
        return StatusCode(200, result);
    }

    public async Task<IActionResult> SendAsync<TRequest>(TRequest command,
    HttpStatusCode statusCode = HttpStatusCode.OK) where TRequest : IRequest
    {
        await mediator.Send(command);
        return StatusCode((int)statusCode);
    }
}

