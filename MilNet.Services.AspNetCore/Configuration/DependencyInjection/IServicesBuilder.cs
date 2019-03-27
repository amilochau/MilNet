using MilNet.Services.AspNetCore.Configuration;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>Services builder interface</summary>
    public interface IServicesBuilder<TOptions> : IHealthChecksBuilder
        where TOptions : ServicesOptions, new()
    {
        /// <summary>Options</summary>
        TOptions Options { get; }
    }
}
