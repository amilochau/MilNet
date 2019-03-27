using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using MilNet.Core.Exceptions;
using System;
using System.Threading.Tasks;

namespace MilNet.Core.Middlewares
{
    /// <summary>Custom exception handler middleware</summary>
    public class ApplicationExceptionHandlerMiddleware
    {
        private readonly RequestDelegate next;
        private readonly ILogger<ApplicationExceptionHandlerMiddleware> logger;

        /// <summary>Constructor</summary>
        public ApplicationExceptionHandlerMiddleware(RequestDelegate next,
            ILogger<ApplicationExceptionHandlerMiddleware> logger)
        {
            this.next = next;
            this.logger = logger;
        }

        /// <summary>Middleware invoke</summary>
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (ForbiddenException ex)
            {
                logger.LogInformation(ex, "ForbiddenException raised.");

                // We can't do anything if the response has already started, just abort
                if (context.Response.HasStarted)
                    throw;

                // Return new ForbidResult
                await context.ForbidAsync();
            }
            catch (NotFoundException ex)
            {
                logger.LogInformation(ex, "NotFoundException raised.");
                
                // We can't do anything if the response has already started, just abort
                if (context.Response.HasStarted)
                    throw;

                // Set status code to 404 (not found)
                context.Response.Clear();
                context.Response.StatusCode = StatusCodes.Status404NotFound;
                await context.Response.WriteAsync(ex.Message);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error catched by exception handler middleware, then raised again.");
                throw ex;
            }
        }
    }
}
