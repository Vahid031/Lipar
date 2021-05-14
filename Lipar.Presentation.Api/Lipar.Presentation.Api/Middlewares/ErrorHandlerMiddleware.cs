using FluentValidation;
using Lipar.Tools.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace Lipar.Presentation.Api.Middlewares
{
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate next;
        private readonly IJson json;
        private readonly ILogger<ErrorHandlerMiddleware> logger;
        private readonly ApiExceptionOptions options;

        public ErrorHandlerMiddleware(RequestDelegate next, IJson json,
            ILogger<ErrorHandlerMiddleware> logger,
            ApiExceptionOptions options)
        {
            this.next = next;
            this.json = json;
            this.logger = logger;
            this.options = options;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (ValidationException ex)
            {
                var problem = HandleValidationException(ex, context) ;
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                context.Response.ContentType = "application/problem+json";
                await context.Response.WriteAsync(json.SerializeObject(problem));
            }
            catch (Exception ex)
            {
                var problem = HandleUnknownException(ex, context);
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                context.Response.ContentType = "application/problem+json";
                await context.Response.WriteAsync(json.SerializeObject(problem));
            }

        }

        private ApiProblemDetails HandleValidationException(ValidationException ex, HttpContext context)
        {
            var errors = new Dictionary<string, string[]>();
            foreach (var error in ex.Errors)
            {
                if (errors.ContainsKey(error.PropertyName))
                {
                    var strArray = errors[error.PropertyName];
                    Array.Resize(ref strArray, strArray.Length + 1);
                    strArray[strArray.Length - 1] = error.ErrorMessage;
                    errors[error.PropertyName] = strArray;
                }
                else
                {
                    errors.Add(error.PropertyName, new string[] { error.ErrorMessage });
                }
            }
            
            return new ApiProblemDetails
            {
                TraceId = Guid.NewGuid(),
                Status = (int)HttpStatusCode.BadRequest,
                Title = "an error has been accured!",
                Details = GetInnerExceptionMessage(ex),
                Instance = context.Request.Path,
                Errors = errors
            };
        }

        private ApiProblemDetails HandleUnknownException(Exception ex, HttpContext context)
        {
            var problem = new ApiProblemDetails
            {
                TraceId = Guid.NewGuid(),
                Status = (int)HttpStatusCode.BadRequest,
                Title = ex.Message,
                Details = context.Request.PathBase,
                Instance = context.Request.Path
            };

            options.AddResponseDetails?.Invoke(context, ex, problem);

            var innerExMessage = GetInnerExceptionMessage(ex);

            var level = options.DetermineLogLevel?.Invoke(ex) ?? LogLevel.Error;

            logger.Log(level, ex, "BADNESS!!! " + innerExMessage + " -- {ErrorId}.", problem.TraceId);


            return problem;
        }

        private string GetInnerExceptionMessage(Exception exception)
        {
            if (exception.InnerException != null)
                return GetInnerExceptionMessage(exception.InnerException);

            return exception.Message;
        }
    }
}