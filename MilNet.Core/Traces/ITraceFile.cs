namespace MilNet.Core.Traces
{
    /// <summary>Trace file interface</summary>
    public interface ITraceFile
    {
        /// <summary>File type</summary>
        TraceFile.TypeFile Type { get; }

        /// <summary>File extension</summary>
        string Ext { get; }
    }
}
