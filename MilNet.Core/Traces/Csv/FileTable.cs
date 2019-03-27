using System;
using MilNet.Core.Types;

namespace MilNet.Core.Traces.Csv
{
    /// <summary>Fichier CSV utilisable pour un affichage comme tableau</summary>
    public class FileTable : TraceFile
    {
        /// <summary>Header line</summary>
        public DataCollection<string> Header { get; } = new DataCollection<string>();
        /// <summary>Content lines</summary>
        public DataCollection<string[]> Content { get; } = new DataCollection<string[]>();
        /// <summary>Readed</summary>
        public bool Readed { get; set; } = false;

        /// <summary>File type</summary>
        public override TypeFile Type => TypeFile.Csv;

        /// <summary>Constructor</summary>
        /// <param name="ext">File extension</param>
        public FileTable(string ext) : base(ext) { }

        /// <summary>Clear all previous readed data</summary>
        public override void CleanData()
        {
            Readed = false;
            Header.Clear();
            Content.Clear();
        }

        /// <summary>Read the file</summary>
        /// <param name="fileName">Complete file name (with repository and extension)</param>
        /// <param name="separator">Columns separator</param>
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="UnauthorizedAccessException"/>
        /// <exception cref="NotSupportedException"/>
        /// <exception cref="System.IO.IOException">I/O exception</exception>
        /// <exception cref="System.Security.SecurityException"/>
        public void Read(string fileName, char separator)
        {
            if (string.IsNullOrEmpty(fileName))
                throw new ArgumentNullException(nameof(fileName));
;
            CleanData();

            string[] text = System.IO.File.ReadAllLines(fileName);
            if (text.Length == 0)
                return;

            Header.AddRange(text[0].Split(separator));
            for (int i = 1; i < text.Length; i++)
            {
                string[] line = text[i].Split(separator);
                Array.Resize(ref line, Header.Count);
                Content.Add(line);
            }

            Readed = true;
        }
    }
}
