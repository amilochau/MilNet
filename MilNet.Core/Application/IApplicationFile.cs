using MilNet.Core.Application.Dependencies;
using MilNet.Core.Application.Versions;

namespace MilNet.Core.Application
{
    /// <summary>File containing application details</summary>
    [System.Obsolete("Will be removed in version 3. Please use standard .NET Core features instead.")]
    public interface IApplicationFile : IVersionsFile, IDependenciesFile { }
}
