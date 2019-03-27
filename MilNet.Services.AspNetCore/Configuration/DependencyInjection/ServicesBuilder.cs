using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;
using System;

namespace MilNet.Services.AspNetCore.Configuration
{
    /// <summary>Services helper class for DI configuration</summary>
    public class ServicesBuilder<TOptions> : IServicesBuilder<TOptions>
        where TOptions : ServicesOptions, new()
    {
        /// <summary>Constructor</summary>
        public ServicesBuilder(IServiceCollection services)
        {
            Services = services ?? throw new ArgumentNullException(nameof(services));
            
            var serviceProvider = services.BuildServiceProvider();
            Options = serviceProvider.GetService<IOptions<TOptions>>()?.Value ?? new TOptions();
        }

        public IHealthChecksBuilder Add(HealthCheckRegistration registration)
        {
            if (registration == null)
            {
                throw new ArgumentNullException(nameof(registration));
            }

            Services.Configure<HealthCheckServiceOptions>(options => options.Registrations.Add(registration));
            return this;
        }

        /// <summary>Service collection</summary>
        public IServiceCollection Services { get; }

        /// <summary>Options</summary>
        public TOptions Options { get; }
    }
}
