using MilNet.Services.Models;
using Swashbuckle.AspNetCore.Swagger;
using System.Collections.Generic;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>Builder extension methods for registering documentation services</summary>
    public static class ServicesBuilderExtensionsDocumentation
    {
        /// <summary>Add the core services</summary>
        public static IServicesBuilder<TOptions> AddDocumentationServices<TOptions>(this IServicesBuilder<TOptions> builder)
            where TOptions : ServicesOptions, new()
        {
            // Documentation microservices
            builder.AddSwaggerServices();

            return builder;
        }

        public static IServicesBuilder<TOptions> AddSwaggerServices<TOptions>(this IServicesBuilder<TOptions> builder)
            where TOptions : ServicesOptions, new()
        {
            var applicationName = builder.Options.ApplicationName;
            var documentation = builder.Options.Documentation ?? new DocumentationOptions();
            var version = documentation.Version ?? "v1";
            var contact = new Contact
            {
                Email = builder.Options.Contact?.Technical?.Email
            };

            builder.Services.AddSwaggerGen(c =>
            {
                // Main information
                c.SwaggerDoc(version, new Info
                {
                    Title = $"{applicationName} API",
                    Version = version,
                    Description = $"All public API proposed by {applicationName} application",
                    Contact = contact
                });

                // Security
                if (documentation.EnableSecurity)
                {
                    c.AddSecurityDefinition("Bearer", new ApiKeyScheme
                    {
                        Description = "JWT Authorization header using the Bearer scheme. Example: \"Bearer {token}\"",
                        Name = "Authorization",
                        In = "header",
                        Type = "apiKey"
                    });

                    c.AddSecurityRequirement(new Dictionary<string, IEnumerable<string>>()
                    {
                        { "Bearer", new string[]{ } }
                    });
                }

                // XML comments file
                if (!string.IsNullOrEmpty(documentation.XmlCommentsFilePath))
                {
                    c.IncludeXmlComments(documentation.XmlCommentsFilePath);
                }

                // Formatting
                if (documentation.DescribeAllEnumsAsStrings)
                {
                    c.DescribeAllEnumsAsStrings();
                }
                if (documentation.DescribeAllParametersInCamelCase)
                {
                    c.DescribeAllParametersInCamelCase();
                }
                if (documentation.DescribeStringEnumsInCamelCase)
                {
                    c.DescribeStringEnumsInCamelCase();
                }
            });

            return builder;
        }
    }
}
