using MilNet.Services.AspNetCore.Configuration;
using MilNet.Services.Models;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>Builder extension methods for registering health checks</summary>
    public static class ServicesBuilderExtensionsHealthChecks
    {
        /// <summary>Add the required health checks</summary>
        public static IServicesBuilder<TOptions> AddHealthChecks<TOptions>(this IServicesBuilder<TOptions> builder)
            where TOptions : ServicesOptions, new()
        {
            builder.Services.AddHealthChecks()
                .AddDiskStorageHealthCheck(setup => setup.AddDrive(System.IO.Path.GetPathRoot(Environment.CurrentDirectory), 1024), name: "Disk storage")
                .AddPrivateMemoryHealthCheck(1024 * 1024 * 1024, name: "Private memory");

            return builder;
        }
    }
}
