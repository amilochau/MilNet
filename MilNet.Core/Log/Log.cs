using System;
using System.Globalization;
using System.IO;
using System.Linq;

namespace MilNet.Core.Logs
{
    /// <summary>Log</summary>
    [System.Obsolete("Will be removed in version 3. Please use standard .NET Core features instead.")]
    public class Log
    {
        /// <summary>Absolute log file path</summary>
        public string Path { get; private set; }

        /// <summary>Maximum size authorized for the log file</summary>
        public long MaxSize { get; set; } = 1048576;
        /// <summary>Log file name</summary>
        public string FileName => System.IO.Path.GetFileName(Path);

        /// <summary>Constructor</summary>
        /// <exception cref="ArgumentNullException"/>
        public Log(string path)
        {
            if (string.IsNullOrEmpty(path))
                throw new ArgumentNullException(nameof(path));

            Path = path;
        }

        /// <summary>Gravity of a message to write</summary>
        public enum Gravity
        {
            /// <summary>Information message</summary>
            Information,
            /// <summary>Warning message</summary>
            Warning,
            /// <summary>Error message</summary>
            Error
        }

        /// <summary>Check if the log file exists ; check the log file size</summary>
        /// <exception cref="IOException">I/O exception</exception>
        /// <exception cref="System.Security.SecurityException"/>
        /// <exception cref="UnauthorizedAccessException"/>
        /// <exception cref="NotSupportedException"/>
        public void CheckSize() => CheckSize(MaxSize);
        /// <summary>Check if the log file exists ; check the log file size</summary>
        /// <exception cref="IOException">I/O exception</exception>
        /// <exception cref="System.Security.SecurityException"/>
        /// <exception cref="UnauthorizedAccessException"/>
        /// <exception cref="NotSupportedException"/>
        public void CheckSize(long maxSize)
        {
            if (File.Exists(Path))
            {
                FileInfo info = new FileInfo(Path);
                if (info.Length > maxSize)
                {
                    if (File.Exists(Path + "old"))
                        File.Delete(Path + "old");
                    File.Move(Path, Path + "old");
                    // Create a new log file
                    // Warning! Do not use Write to prevent an infinite loop!
                    using (StreamWriter writer = new StreamWriter(Path, true, System.Text.Encoding.UTF8))
                    {
                        writer.WriteLine(string.Format(CultureInfo.InvariantCulture, "{0:dd/MM/yyyy HH:mm:ss.f} - {1:g} - ", DateTime.Now, Gravity.Information) +
                            string.Format(CultureInfo.CurrentCulture, Resources.Resources.FileLogCreatedInto, Path));
                        writer.Flush();
                    }
                }
            }
            else // Log file doesn't exit: create a new one
            {
                using (StreamWriter writer = new StreamWriter(Path, true, System.Text.Encoding.UTF8))
                {
                    writer.WriteLine(string.Format(CultureInfo.InvariantCulture, "{0:dd/MM/yyyy HH:mm:ss.f} - {1:g} - ", DateTime.Now, Gravity.Information) +
                        string.Format(CultureInfo.CurrentCulture, Resources.Resources.FileLogCreatedInto, Path));
                    writer.Flush();
                }
            }
        }

        /// <summary>Write a message into log file</summary>
        /// <remarks>Log message is horodated</remarks>
        /// <exception cref="IOException">I/O exception</exception>
        /// <exception cref="System.Security.SecurityException"/>
        /// <exception cref="UnauthorizedAccessException"/>
        /// <exception cref="NotSupportedException"/>
        public void Write(string message, Gravity gravity)
        {
            CheckSize(); // Check log file size
            using (StreamWriter writer = new StreamWriter(Path, true, System.Text.Encoding.UTF8))
            {
                writer.WriteLine(string.Format(CultureInfo.InvariantCulture, "{0:dd/MM/yyyy HH:mm:ss.f} - {1:g} - {2}", DateTime.Now, gravity, message));
                writer.Flush();
            }
        }
        /// <summary>Write an exception into log file</summary>
        /// <remarks>Log message is horodated; no exception is thrown if ex is null</remarks>
        /// <exception cref="IOException">I/O exception</exception>
        /// <exception cref="System.Security.SecurityException"/>
        /// <exception cref="UnauthorizedAccessException"/>
        /// <exception cref="NotSupportedException"/>
        public void Write(Exception ex, Gravity gravity)
        {
            if (ex != null)
                Write(ex.Message + Environment.NewLine + ex.StackTrace, gravity);
        }

        /// <summary>Read log file content</summary>
        /// <param name="inverseOrder">Inverse lines order</param>
        /// <param name="detectWarnings">Detect warning and error lines</param>
        /// <returns>Log file content as flat string</returns>
        /// <exception cref="System.Security.SecurityException"/>
        /// <exception cref="UnauthorizedAccessException"/>
        /// <exception cref="NotSupportedException"/>
        public string ReadAll(bool inverseOrder, bool detectWarnings)
        {
            try
            {
                CheckSize(); // Check log file size
                if (!File.Exists(Path))
                    return string.Format(CultureInfo.CurrentCulture, Resources.Resources.NoLogFileFound, Path);

                string[] lines = File.ReadAllLines(Path, System.Text.Encoding.UTF8);
                if (detectWarnings)
                {
                    for (int i = 0; i < lines.Length; i++)
                        lines[i] = DetectWarnings(lines[i]);
                }

                string result;
                if (inverseOrder)
                    result = lines.Aggregate((t, next) => next + Environment.NewLine + t);
                else
                    result = lines.Aggregate((t, next) => t + Environment.NewLine + next);
                return result;
            }
            catch (IOException ex)
            {
                return Resources.Resources.LogFileCantBeOpened + Environment.NewLine + ex.Message;
            }
        }
        /// <summary>Read all lines from the log file</summary>
        /// <returns>Log file content as string array</returns>
        /// <exception cref="System.Security.SecurityException"/>
        /// <exception cref="UnauthorizedAccessException"/>
        /// <exception cref="NotSupportedException"/>
        public string[] ReadAllLines()
        {
            try
            {
                CheckSize(); // Check log file size
                if (File.Exists(Path))
                    return File.ReadAllLines(Path, System.Text.Encoding.UTF8);
                else
                    return new string[] { string.Format(CultureInfo.CurrentCulture, Resources.Resources.NoLogFileFound, Path) };
            }
            catch (IOException ex)
            {
                return new string[] { Resources.Resources.LogFileCantBeOpened + Environment.NewLine + ex.Message};
            }
        }
        /// <summary>Read all lines from the log file</summary>
        /// <returns>Log file content as byte array</returns>
        /// <exception cref="UnauthorizedAccessException"/>
        /// <exception cref="NotSupportedException"/>
        public byte[] ReadAllByte()
        {
            try
            {
                if (File.Exists(Path))
                {
                    using (FileStream stream = File.Open(Path, FileMode.Open))
                    {
                        byte[] content = new byte[stream.Length];
                        stream.Read(content, 0, Convert.ToInt32(stream.Length));
                        return content;
                    }
                }
                else
                    return null;
            }
            catch (IOException)
            {
                return null;
            }
        }

        /// <summary>Detect warnings and errors into a log line</summary>
        /// <param name="line">Log line</param>
        private static string DetectWarnings(string line)
        {
            if (line.IndexOf(Gravity.Warning.ToString(), StringComparison.OrdinalIgnoreCase) > 0)
                return "<span class=\"warning-log\">" + line + "</span>";
            else if (line.IndexOf(Gravity.Error.ToString(), StringComparison.OrdinalIgnoreCase) > 0)
                return "<span class=\"warning-log\">" + line + "</span>";
            else
                return line;
        }
    }
}
