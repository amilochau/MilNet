using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using static MilNet.Core.Traces.TraceFile;

namespace MilNet.Core.Traces.Csv
{
    /// <summary>Trace file data serie</summary>
    /// <typeparam name="T">Readed data type</typeparam>
    public abstract class BaseSerie<T> : ISerie
    {
        /// <summary>Base serie type</summary>
        public Enum Type { get; }
        /// <summary>File serie header</summary>
        public string Header { get; }
        /// <summary>Header is recognized with a regex</summary>
        public bool IsRegexHeader { get; set; } = false;

        /// <summary>Index of the associated property</summary>
        private int[] index { get; }

        /// <summary>Serie title displayed on the graphs</summary>
        public string Title { get; set; }
        /// <summary>Index of the file serie column</summary>
        public int ColumnIndex { get; set; } = -1;
        /// <summary>The serie can be used as an abcisse</summary>
        public bool IsAbscisse { get; set; } = false;
        /// <summary>The serie is a string</summary>
        public bool IsString { get; set; }
        /// <summary>Type of data to used to display the serie</summary>
        public TypeData TypeDataToDisplay { get; set; } = TypeData.NotSet;

        /// <summary>The serie is readed; useful to reset the column index</summary>
        public bool Readed
        {
            get { return ColumnIndex >= 0; }
            set { if (!value) ColumnIndex = -1; }
        }

        /// <summary>Readed data</summary>
        /// <remarks>All the serie runs are in this collection</remarks>
        private List<T> data { get; } = new List<T>();
        
        /// <summary>Constructor</summary>
        /// <param name="type">Serie type</param>
        /// <param name="header">File serie header</param>
        /// <param name="index">Index of the associated property</param>
        protected BaseSerie(Enum type, string header, params int[] index)
        {
            Type = type;
            Header = header;
            this.index = index;
        }

        /// <summary>Clear all the previous readed data</summary>
        /// <remarks>Configuration is maintained</remarks>
        public void CleanData()
        {
            Readed = false;
            data.Clear();
        }

        /// <summary>Convert and add a new value to the data collection</summary>
        /// <param name="value">Value to add</param>
        /// <param name="culture">Culture associated to the int and double values types</param>
        /// <param name="cultureDate">Culture associated to the dates</param>
        public abstract void Add(string value, CultureInfo culture, CultureInfo cultureDate);
        /// <summary>Add a new run</summary>
        /// <param name="value">Run number</param>
        public virtual void AddRun(int value) => AddNull();
        /// <summary>Add a new empty value</summary>
        public void AddNull() => AddData(default(T));
        /// <summary>Remove the last value from the data collection</summary>
        public void RemoveLast()
        {
            if (data.Count > 0)
                data.RemoveAt(data.Count - 1);
        }
        /// <summary>Remove a value from the data collection</summary>
        /// <param name="index">Index of the value to remove</param>
        /// <exception cref="ArgumentOutOfRangeException"/>
        public void RemoveAt(int index) => data.RemoveAt(index);
        /// <summary>Data count</summary>
        public int Count => data.Count;

        /// <summary>Correspondance with the associated property index</summary>
        /// <param name="index">Index table to test</param>
        /// <exception cref="ArgumentNullException"/>
        public bool MatchIndex(int[] index) => Enumerable.SequenceEqual(this.index, index);

        /// <summary>Add a new data to the collection</summary>
        /// <param name="value">Value to add</param>
        public void AddData(T value) => data.Add(value);
        /// <summary>Get the data</summary>
        public T[] GetData() => data.ToArray();
        /// <summary>Get the first readed point</summary>
        public virtual T GetFirstData() => GetFirstData(d => d != null);
        /// <summary>Get the first readed point</summary>
        /// <exception cref="ArgumentNullException"/>
        public virtual T GetFirstData(Func<T, bool> predicate) => data.FirstOrDefault(predicate);
        /// <summary>Get the last readed point</summary>
        public virtual T GetLastData() => GetLastData(d => d != null);
        /// <summary>Get the last readed point</summary>
        /// <exception cref="ArgumentNullException"/>
        public virtual T GetLastData(Func<T, bool> predicate) => data.LastOrDefault(predicate);
    }
}
