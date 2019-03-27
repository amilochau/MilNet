using Microsoft.Extensions.Configuration;
using MilNet.Services.AspNetCore.Configuration;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>Extension methods for setting up MilNet Services related services in an <see cref="IServiceCollection"/></summary>
    public static class ServicesServiceCollectionExtensions
    {
        /// <summary>Create a builder</summary>
        /// <param name="services"></param>
        public static IServicesBuilder<TOptions> AddMilNetServicesBuilder<TOptions>(this IServiceCollection services)
            where TOptions : ServicesOptions, new()
        {
            return new ServicesBuilder<TOptions>(services);
        }

        /// <summary>Add MilNet Services</summary>
        public static IServicesBuilder<TOptions> AddMilNetServices<TOptions>(this IServiceCollection services)
            where TOptions : ServicesOptions, new()
        {
            var builder = services.AddMilNetServicesBuilder<TOptions>();
            
            builder
                .AddRequiredPlatformServices<TOptions>()
                .AddCoreServices<TOptions>()
                .AddCorsServices()
                .AddBusinessServices<TOptions>()
                .AddDocumentationServices()
                .AddCookiePolicy()
                .AddIISServices()
                .AddHealthChecks();
            
            return builder;
        }

        /// <summary>Add MilNet Services</summary>
        public static IServicesBuilder<TOptions> AddMilNetServices<TOptions>(this IServiceCollection services, Action<TOptions> setupAction)
            where TOptions : ServicesOptions, new()
        {
            services.Configure(setupAction);
            return services.AddMilNetServices<TOptions>();
        }

        /// <summary>Add MilNet Services</summary>
        public static IServicesBuilder<TOptions> AddMilNetServices<TOptions>(this IServiceCollection services, IConfiguration configuration)
            where TOptions : ServicesOptions, new()
        {
            services.Configure<TOptions>(configuration);
            return services.AddMilNetServices<TOptions>();
        }
    }
}
