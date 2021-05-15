using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;

namespace Lipar.Presentation.Api.Middlewares
{
    public class ApiExceptionOptions
    {
        public Action<HttpContext, Exception, ApiProblemDetails> AddResponseDetails { get; set; }
        public Func<Exception, LogLevel> DetermineLogLevel { get; set; }
    }
}
