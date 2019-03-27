using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using MilNet.Core;
using MilNet.Services.Models;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>Builder extension methods for registering core services</summary>
    public static class ServicesBuilderExtensionsCore
    {
        /// <summary>Add the required platform services</summary>
        public static IServicesBuilder<TOptions> AddRequiredPlatformServices<TOptions>(this IServicesBuilder<TOptions> builder)
            where TOptions : ServicesOptions, new()
        {
            // Required platform services
            builder.Services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            builder.Services.AddOptions();
            builder.Services.AddSingleton(resolver => resolver.GetRequiredService<IOptions<TOptions>>().Value);

            return builder;
        }

        /// <summary>Add the core services</summary>
        public static IServicesBuilder<TOptions> AddCoreServices<TOptions>(this IServicesBuilder<TOptions> builder)
            where TOptions : ServicesOptions, new()
        {
            // Core services
            builder.Services.AddHttpClient();
            builder.Services.AddRequestProvider();

            return builder;
        }

        /// <summary>Add the IIS services</summary>
        public static IServicesBuilder<TOptions> AddCorsServices<TOptions>(this IServicesBuilder<TOptions> builder)
            where TOptions : ServicesOptions, new()
        {
            builder.Services.AddCors(options => options.AddPolicy("AllowAll", p => p
                .AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod()));

            return builder;
        }

        /// <summary>Add the IIS services</summary>
        public static IServicesBuilder<TOptions> AddIISServices<TOptions>(this IServicesBuilder<TOptions> builder)
            where TOptions : ServicesOptions, new()
        {
            builder.Services.Configure<IISOptions>(options =>
            {
                options.AutomaticAuthentication = false;
            });

            return builder;
        }

        public static IServicesBuilder<TOptions> AddCookiePolicy<TOptions>(this IServicesBuilder<TOptions> builder)
            where TOptions : ServicesOptions, new()
        {
            // Cookies policy (user content for non-essential cookies is needed for a given request)
            builder.Services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            return builder;
        }
    }
}
