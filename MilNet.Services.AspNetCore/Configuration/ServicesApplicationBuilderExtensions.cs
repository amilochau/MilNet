﻿using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MilNet.Core;
using MilNet.Services.AspNetCore.Configuration;

namespace Microsoft.AspNetCore.Builder
{
    /// <summary>Pipeline extension methods for adding MilNet Services</summary>
    public static class ServicesApplicationBuilderExtensions
    {
        /// <summary>Add MilNet Services to the pipeline</summary>
        public static IApplicationBuilder UseMilNetServices<TOptions>(this IApplicationBuilder app)
            where TOptions : ServicesOptions, new()
        {
            var options = app.ApplicationServices.GetService<IOptions<TOptions>>()?.Value ?? new TOptions();

            app.UseApplicationExceptionHandlerMiddleware();
            app.UseCors("AllowAll");
            app.UseCookiePolicy();

            // Health checks
            app.UseHealthChecks("/health", new HealthCheckOptions()
            {
                Predicate = _ => true,
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            });

            // Swagger
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", $"{options.ApplicationName} API V1");
            });

            return app;
        }
    }
}
