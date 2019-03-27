namespace MilNet.Core.Traces
{
    /// <summary>Trace file</summary>
    public abstract class TraceFile : ITraceFile, ICleanable
    {
        /// <summary>File type</summary>
        public abstract TypeFile Type { get; }

        /// <summary>File extension</summary>
        public string Ext { get; }
        
        /// <summary>File name base</summary>
        protected internal string BaseFileName { get; set; }
        
        /// <summary>File name</summary>
        protected internal string FileName => string.IsNullOrEmpty(BaseFileName) ? null : BaseFileName + Ext;

        /// <summary>Clear all previous readed data</summary>
        /// <remarks>Configuration is maintained</remarks>
        public abstract void CleanData();

        /// <summary>Constructor</summary>
        /// <param name="ext">File extension</param>
        protected TraceFile(string ext)
        {
            Ext = ext;
        }

        /// <summary>File type</summary>
        public enum TypeFile
        {
            /// <summary>Fichier de données CSV</summary>
            Csv,
            /// <summary>Fichier de données textuelles</summary>
            Texte
        }

        /// <summary>Data type to use to display series</summary>
        public enum TypeData
        {
            /// <summary>Displayed with Serie.IsDate</summary>
            NotSet = 0,
            /// <summary>Displayed as decimal numbers</summary>
            Double = 1,
            /// <summary>Displayed as short integers</summary>
            Int32 = 2,
            /// <summary>Displayed as time (hours and minutes)</summary>
            Time = 3
        }
    }
}
