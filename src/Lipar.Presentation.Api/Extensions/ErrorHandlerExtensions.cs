using Microsoft.AspNetCore.Builder;
using System;

namespace Lipar.Presentation.Api.Middlewares;

public static class ErrorHandlerMiddlewareExtensions
{
    public static IApplicationBuilder UseApiExceptionHandler(this IApplicationBuilder builder)
    {
        var options = new ApiExceptionOptions();
        return builder.UseMiddleware<ErrorHandlerMiddleware>(options);
    }

    public static IApplicationBuilder UseApiExceptionHandler(this IApplicationBuilder builder,
    Action<ApiExceptionOptions> configureOptions)
    {
        var options = new ApiExceptionOptions();
        configureOptions(options);

        return builder.UseMiddleware<ErrorHandlerMiddleware>(options);
    }
}


