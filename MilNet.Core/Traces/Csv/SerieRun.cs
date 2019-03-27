using System;

namespace MilNet.Core.Traces.Csv
{
    /// <summary>Serie of trace file runs</summary>
    public class SerieRun : SerieInt
    {
        /// <summary>Constructor</summary>
        /// <param name="type">Serie data type</param>
        /// <param name="header">File serie header</param>
        /// <param name="index">Associated property index</param>
        public SerieRun(Enum type, string header, params int[] index) : base(type, header, index) { }

        /// <summary>Add a new run</summary>
        /// <param name="value">Value of the run</param>
        public override void AddRun(int value) => AddData(value);
    }
}
