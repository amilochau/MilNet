using System.Collections.ObjectModel;
using System.IO;

namespace MilNet.Core.Application.Versions
{
    /// <summary>Interface of file containing versions details</summary>
    [System.Obsolete("Will be removed in version 3. Please use standard .NET Core features instead.")]
    public interface IVersionsFile
    {
        /// <summary>List of versions</summary>
        Collection<Version> Versions { get; }

        /// <summary>Read a file containing versions details</summary>
        /// <param name="fileName">File name</param>
        /// <exception cref="System.ArgumentNullException"/>
        /// <exception cref="FileNotFoundException"/>
        /// <exception cref="IOException"/>
        void Read(string fileName);
    }
}
