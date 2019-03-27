using System;
using System.Collections.ObjectModel;
using System.IO;
using MilNet.Core.Application.Dependencies;
using MilNet.Core.Logs;
using static MilNet.Core.Logs.Log;

namespace MilNet.Core.Configuration
{
    /// <summary>Configuration</summary>
    [System.Obsolete("Will be removed in version 3. Please use standard .NET Core features instead.")]
    public class Configuration : PropCollection, IConfiguration
    {
        #region Constructor

        /// <summary>Constructor</summary>
        /// <exception cref="ArgumentNullException"/>
        public Configuration(string configurationPath, string dependenciesPath, string logPath)
        {
            if (string.IsNullOrEmpty(configurationPath))
                throw new ArgumentNullException(nameof(configurationPath));
            if (string.IsNullOrEmpty(dependenciesPath))
                throw new ArgumentNullException(nameof(dependenciesPath));
            if (string.IsNullOrEmpty(logPath))
                throw new ArgumentNullException(nameof(logPath));

            ConfigurationPath = configurationPath;
            DependenciesPath = dependenciesPath;
            LogPath = logPath;

            log = new Log(logPath);
        }

        #endregion
        #region Files

        /// <summary>Configuration file absolute path</summary>
        public string ConfigurationPath { get; private set; }
        /// <summary>Dependencies file absolute path</summary>
        public string DependenciesPath { get; private set; }
        /// <summary>Log file absolute path</summary>
        public string LogPath { get; private set; }

        #endregion
        #region Settings

        /// <summary>Open configuration file to read settings</summary>
        /// <exception cref="FileNotFoundException"/>
        /// <exception cref="IOException"/>
        /// <exception cref="NotSupportedException"/>
        public void OpenSettings() => Open(ConfigurationPath);
        /// <summary>Open configuration file to read settings</summary>
        /// <param name="fileName">File name</param>
        /// <exception cref="FileNotFoundException"/>
        /// <exception cref="IOException"/>
        /// <exception cref="NotSupportedException"/>
        public void OpenSettings(string fileName) => Open(fileName);
        /// <summary>Save settings into application configuration file</summary>
        /// <exception cref="DirectoryNotFoundException"/>
        /// <exception cref="IOException"/>
        /// <exception cref="UnauthorizedAccessException"/>
        /// <exception cref="NotSupportedException"/>
        /// <exception cref="System.Security.SecurityException"/>
        public void SaveSettings() => Save(ConfigurationPath);

        #endregion
        #region Log

        /// <summary>General application log</summary>
        private Log log;

        /// <summary>Log file name</summary>
        public string LogFileName => log.FileName;
        /// <summary>Log maximum size</summary>
        public long LogMaxSize
        {
            get => log.MaxSize;
            set => log.MaxSize = value;
        }

        /// <summary>Read the log file</summary>
        /// <exception cref="System.Security.SecurityException"/>
        /// <exception cref="UnauthorizedAccessException"/>
        /// <exception cref="NotSupportedException"/>
        public string ReadAllLog(bool inverseOrder, bool detectWarnings) => log.ReadAll(inverseOrder, detectWarnings);
        /// <summary>Write a message into log file</summary>
        /// <remarks>Log message is horodated</remarks>
        /// <exception cref="IOException">I/O exception</exception>
        /// <exception cref="System.Security.SecurityException"/>
        /// <exception cref="UnauthorizedAccessException"/>
        /// <exception cref="NotSupportedException"/>
        public void WriteLog(string message) => WriteLog(message, Gravity.Information);
        /// <summary>Write a message into log file</summary>
        /// <remarks>Log message is horodated</remarks>
        /// <exception cref="IOException">I/O exception</exception>
        /// <exception cref="System.Security.SecurityException"/>
        /// <exception cref="UnauthorizedAccessException"/>
        /// <exception cref="NotSupportedException"/>
        public void WriteLog(string message, Gravity gravity) => log.Write(message, gravity);

        /// <summary>Write an exception into log file</summary>
        /// <remarks>Log message is horodated</remarks>
        /// <exception cref="IOException">I/O exception</exception>
        /// <exception cref="System.Security.SecurityException"/>
        /// <exception cref="UnauthorizedAccessException"/>
        /// <exception cref="NotSupportedException"/>
        public bool WriteLog(Exception ex, bool catchException) => WriteLog(ex, Gravity.Information, catchException);
        /// <summary>Write an exception into log file</summary>
        /// <remarks>Log message is horodated</remarks>
        /// <exception cref="IOException">I/O exception</exception>
        /// <exception cref="System.Security.SecurityException"/>
        /// <exception cref="UnauthorizedAccessException"/>
        /// <exception cref="NotSupportedException"/>
        public bool WriteLog(Exception ex, Gravity gravity, bool catchException)
        {
            WriteLog(ex, gravity);
            return catchException;
        }
        /// <summary>Write an exception into log file</summary>
        /// <remarks>Log message is horodated</remarks>
        /// <exception cref="IOException">I/O exception</exception>
        /// <exception cref="System.Security.SecurityException"/>
        /// <exception cref="UnauthorizedAccessException"/>
        /// <exception cref="NotSupportedException"/>
        public void WriteLog(Exception ex) => WriteLog(ex, Gravity.Information);
        /// <summary>Write an exception into log file</summary>
        /// <remarks>Log message is horodated</remarks>
        /// <exception cref="IOException">I/O exception</exception>
        /// <exception cref="System.Security.SecurityException"/>
        /// <exception cref="UnauthorizedAccessException"/>
        /// <exception cref="NotSupportedException"/>
        public void WriteLog(Exception ex, Gravity gravity) => log.Write(ex, gravity);

        #endregion
        #region Dependencies

        /// <summary>Application dependencies</summary>
        private IDependenciesFile dependenciesFile = new DependenciesFile();

        /// <summary>Application dependencies</summary>=
        public Collection<Dependency> Dependencies
        {
            get
            {
                // Open dependencies file if needed
                try
                {
                    if (dependenciesFile.Dependencies.Count == 0)
                        dependenciesFile.Read(DependenciesPath);
                }
                catch (IOException ex)
                {
                    log.Write(ex, Gravity.Warning);
                }
                return dependenciesFile.Dependencies;
            }
        }

        #endregion
    }
}
