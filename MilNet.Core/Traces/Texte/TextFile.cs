using System;
using System.Globalization;
using System.IO;

namespace MilNet.Core.Traces.Texte
{
    /// <summary>Text trace file</summary>
    public class TextFile : TraceFile
    {
        /// <summary>File type</summary>
        public override TypeFile Type => TypeFile.Texte;

        /// <summary>Text file content</summary>
        public string Text { get; private set; }

        /// <summary>Constructor</summary>
        /// <param name="ext">File extension</param>
        public TextFile(string ext) : base(ext) { }

        /// <summary>Read the file</summary>
        /// <param name="fileName">File name (with directory and extension)</param>
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="UnauthorizedAccessException"/>
        /// <exception cref="NotSupportedException"/>
        /// <exception cref="IOException">I/O exception; a specific message is put into <see cref="Text"/> and can be displayed to users</exception>
        public void ReadFile(string fileName)
        {
            try
            {
                if (File.Exists(fileName))
                    Text = File.ReadAllText(fileName);
                else
                    Text = string.Format(CultureInfo.CurrentCulture, Resources.Resources.FileNotFound, fileName);
            }
            catch (IOException) // SystemException not catched: must return an error
            {
                Text = string.Format(CultureInfo.CurrentCulture, Resources.Resources.FileCantBeOpened, fileName);
                throw;
            }
        }

        /// <summary>Clear all previous readed data</summary>
        /// <remarks>Configuration is maintained</remarks>
        public override void CleanData() { }
    }
}
