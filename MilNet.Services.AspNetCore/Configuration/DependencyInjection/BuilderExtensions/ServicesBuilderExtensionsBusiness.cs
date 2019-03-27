using MilNet.Services.AspNetCore.Configuration;
using MilNet.Services.AspNetCore.Services;
using MilNet.Services.AspNetCore.Services.Abstractions;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>Builder extension methods for registering business services</summary>
    public static class ServicesBuilderExtensionsBusiness
    {
        /// <summary>Add the core services</summary>
        public static IServicesBuilder<TOptions> AddBusinessServices<TOptions>(this IServicesBuilder<TOptions> builder)
            where TOptions : ServicesOptions, new()
        {
            // Business services
            builder.Services.AddTransient<IFeedbackService, FeedbackService<TOptions>>();
            builder.Services.AddTransient<IEmailService, EmailService<TOptions>>();
            builder.Services.AddTransient<IIdentityService, IdentityService<TOptions>>();
            builder.Services.AddTransient<IApplicationService, ApplicationService<TOptions>>();

            return builder;
        }
    }
}
