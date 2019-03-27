using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using MilNet.Core.Middlewares;
using MilNet.Core.Services;

namespace MilNet.Core
{
    public static class ServicesExtensions
    {
        public static IApplicationBuilder UseApplicationExceptionHandlerMiddleware(this IApplicationBuilder app)
        {
            return app.UseMiddleware<ApplicationExceptionHandlerMiddleware>();
        }

        /// <summary>Add IRequestProvider with scoped lifetime</summary>
        /// <remarks>HttpClient must be provided into service collection</remarks>
        public static IServiceCollection AddRequestProvider(this IServiceCollection services)
        {
            return services.AddScoped<IRequestProvider, RequestProvider>();
        }
    }
}
