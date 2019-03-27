using System.Collections.ObjectModel;
using System.IO;

namespace MilNet.Core.Application.Dependencies
{
    /// <summary>Interface of file containing dependencies details</summary>
    [System.Obsolete("Will be removed in version 3. Please use standard .NET Core features instead.")]
    public interface IDependenciesFile
    {
        /// <summary>List of dependencies</summary>
        Collection<Dependency> Dependencies { get; }

        /// <summary>Read a file containing dependencies details</summary>
        /// <param name="fileName">File name</param>
        /// <exception cref="System.ArgumentNullException"/>
        /// <exception cref="FileNotFoundException"/>
        /// <exception cref="IOException"/>
        void Read(string fileName);
    }
}
