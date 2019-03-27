using System;
using System.Globalization;

namespace MilNet.Core.Traces.Csv
{
    /// <summary>Trace file data serie</summary>
    /// <remarks>Data will be read as integers</remarks>
    public class SerieInt : BaseSerie<int?>
    {
        /// <summary>Constructor</summary>
        /// <param name="type">Serie type</param>
        /// <param name="header">File serie header</param>
        /// <param name="index">Associated property index</param>
        public SerieInt(Enum type, string header, params int[] index) : base(type, header, index) { }

        /// <summary>Convert and add a new value to the data collection</summary>
        /// <param name="value">Value to add</param>
        /// <param name="culture">Culture associated to the int and double values types</param>
        /// <param name="cultureDate">Culture associated to the dates</param>
        public override void Add(string value, CultureInfo culture, CultureInfo cultureDate)
        {
            int temp;
            if (int.TryParse(value, NumberStyles.Float, culture, out temp))
                AddData(temp);
            else
                AddNull(); // On ajoute par défaut une donnée vide
        }
    }
}
