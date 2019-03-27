using Microsoft.Extensions.Configuration;
using MilNet.Services.AspNetCore.Configuration;
using Serilog;
using Serilog.Exceptions;
using Serilog.Sinks.Elasticsearch;
using System;

namespace Microsoft.AspNetCore.Hosting
{
    public static class WebHostBuilderExtensions
    {
        public static IWebHostBuilder ConfigureMilNetServices<TOptions>(this IWebHostBuilder hostBuilder, Func<WebHostBuilderContext, TOptions> optionsFunc)
            where TOptions : ServicesOptions, new()
        {
            return hostBuilder.ConfigureLogging((hostingContext, logging) =>
            {
                var options = optionsFunc?.Invoke(hostingContext);
                if (options != null)
                {
                    Log.Logger = new LoggerConfiguration()
                        .Enrich.FromLogContext()
                        .Enrich.WithExceptionDetails()
                        .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(options.Log.ElasticSearchUri)
                        {
                            AutoRegisterTemplate = true,
                        })
                    .CreateLogger();
                    logging.AddSerilog();
                }
            });
        }
    }
}
