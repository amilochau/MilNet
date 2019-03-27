using System;
using System.Globalization;
using static MilNet.Core.Traces.TraceFile;

namespace MilNet.Core.Traces.Csv
{

    /// <summary>Trace file data serie</summary>
    public interface ISerie : ICleanable
    {
        /// <summary>Base serie type</summary>
        Enum Type { get; }
        /// <summary>File serie header</summary>
        string Header { get; }
        /// <summary>Header is recognized with a regex</summary>
        bool IsRegexHeader { get; set; }

        /// <summary>Serie title displayed on the graphs</summary>
        string Title { get; set; }
        /// <summary>Index of the file serie column</summary>
        int ColumnIndex { get; set; }
        /// <summary>The serie can be used as an abcisse</summary>
        bool IsAbscisse { get; set; }
        /// <summary>The serie is a string</summary>
        bool IsString { get; set; }
        /// <summary>Type of data to used to display the serie</summary>
        TypeData TypeDataToDisplay { get; set; }

        /// <summary>The serie is readed; useful to reset the column index</summary>
        bool Readed { get; set; }

        /// <summary>Convert and add a new value to the data collection</summary>
        /// <param name="value">Value to add</param>
        /// <param name="culture">Culture associated to the int and double values types</param>
        /// <param name="cultureDate">Culture associated to the dates</param>
        void Add(string value, CultureInfo culture, CultureInfo cultureDate);
        /// <summary>Add a new run</summary>
        /// <param name="value">Run number</param>
        void AddRun(int value);
        /// <summary>Add a new empty value</summary>
        void AddNull();
        /// <summary>Remove the last value from the data collection</summary>
        void RemoveLast();
        /// <summary>Remove a value from the data collection</summary>
        /// <param name="index">Index of the value to remove</param>
        /// <exception cref="ArgumentOutOfRangeException"/>
        void RemoveAt(int index);
        /// <summary>Data count</summary>
        int Count { get; }

        /// <summary>Correspondance with the associated property index</summary>
        /// <param name="index">Index table to test</param>
        /// <exception cref="ArgumentNullException"/>
        bool MatchIndex(int[] index);
    }
}
