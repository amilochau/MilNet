using System;
using System.Globalization;

namespace MilNet.Core.Traces.Csv
{
    /// <summary>Trace file data serie</summary>
    /// <remarks>Data will be read as strings</remarks>
    public class SerieString : BaseSerie<string>
    {
        /// <summary>Constructor</summary>
        /// <param name="type">Serie type</param>
        /// <param name="header">File serie header</param>
        /// <param name="index">Associated property index</param>
        public SerieString(Enum type, string header, params int[] index) : base(type, header, index) { }

        /// <summary>Convert and add a new value to the data collection</summary>
        /// <param name="value">Value to add</param>
        /// <param name="culture">Culture associated to the int and double values types</param>
        /// <param name="cultureDate">Culture associated to the dates</param>
        public override void Add(string value, CultureInfo culture, CultureInfo cultureDate)
        {
            AddData(value);
        }

        /// <summary>Get the first readed point</summary>
        public override string GetFirstData() => GetFirstData(d => !string.IsNullOrEmpty(d));
        /// <summary>Get the last readed point</summary>
        public override string GetLastData() => GetLastData(d => !string.IsNullOrEmpty(d));
    }
}
