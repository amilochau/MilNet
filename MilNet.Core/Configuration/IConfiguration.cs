using System;
using System.Collections.ObjectModel;
using System.IO;
using MilNet.Core.Application.Dependencies;
using static MilNet.Core.Logs.Log;

namespace MilNet.Core.Configuration
{
    /// <summary>Interface of configuration</summary>
    [System.Obsolete("Will be removed in version 3. Please use standard .NET Core features instead.")]
    public interface IConfiguration
    {
        /// <summary>Open configuration file to read settings</summary>
        /// <exception cref="FileNotFoundException"/>
        /// <exception cref="IOException"/>
        /// <exception cref="NotSupportedException"/>
        void OpenSettings();
        /// <summary>Save settings into application configuration file</summary>
        /// <exception cref="DirectoryNotFoundException"/>
        /// <exception cref="IOException"/>
        /// <exception cref="UnauthorizedAccessException"/>
        /// <exception cref="NotSupportedException"/>
        /// <exception cref="System.Security.SecurityException"/>
        void SaveSettings();

        /// <summary>Log file name</summary>
        string LogFileName { get; }
        /// <summary>Log maximum size</summary>
        long LogMaxSize { get; set; }

        /// <summary>Read the log file</summary>
        /// <exception cref="System.Security.SecurityException"/>
        /// <exception cref="UnauthorizedAccessException"/>
        /// <exception cref="NotSupportedException"/>
        string ReadAllLog(bool inverseOrder, bool detectWarnings);
        /// <summary>Write a message into log file</summary>
        /// <remarks>Log message is horodated</remarks>
        /// <exception cref="IOException">I/O exception</exception>
        /// <exception cref="System.Security.SecurityException"/>
        /// <exception cref="UnauthorizedAccessException"/>
        /// <exception cref="NotSupportedException"/>
        void WriteLog(string message);
        /// <summary>Write a message into log file</summary>
        /// <remarks>Log message is horodated</remarks>
        /// <exception cref="IOException">I/O exception</exception>
        /// <exception cref="System.Security.SecurityException"/>
        /// <exception cref="UnauthorizedAccessException"/>
        /// <exception cref="NotSupportedException"/>
        void WriteLog(string message, Gravity gravity);
        /// <summary>Write an exception into log file</summary>
        /// <remarks>Log message is horodated</remarks>
        /// <exception cref="IOException">I/O exception</exception>
        /// <exception cref="System.Security.SecurityException"/>
        /// <exception cref="UnauthorizedAccessException"/>
        /// <exception cref="NotSupportedException"/>
        bool WriteLog(Exception ex, bool catchException);
        /// <summary>Write an exception into log file</summary>
        /// <remarks>Log message is horodated</remarks>
        /// <exception cref="IOException">I/O exception</exception>
        /// <exception cref="System.Security.SecurityException"/>
        /// <exception cref="UnauthorizedAccessException"/>
        /// <exception cref="NotSupportedException"/>
        bool WriteLog(Exception ex, Gravity gravity, bool catchException);
        /// <summary>Write an exception into log file</summary>
        /// <remarks>Log message is horodated</remarks>
        /// <exception cref="IOException">I/O exception</exception>
        /// <exception cref="System.Security.SecurityException"/>
        /// <exception cref="UnauthorizedAccessException"/>
        /// <exception cref="NotSupportedException"/>
        void WriteLog(Exception ex);
        /// <summary>Write an exception into log file</summary>
        /// <remarks>Log message is horodated</remarks>
        /// <exception cref="IOException">I/O exception</exception>
        /// <exception cref="System.Security.SecurityException"/>
        /// <exception cref="UnauthorizedAccessException"/>
        /// <exception cref="NotSupportedException"/>
        void WriteLog(Exception ex, Gravity gravity);

        /// <summary>Application dependencies</summary>
        Collection<Dependency> Dependencies { get; }
    }
}
