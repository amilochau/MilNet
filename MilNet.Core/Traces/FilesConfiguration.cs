using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using MilNet.Core.Traces.Csv;

namespace MilNet.Core.Traces
{
    /// <summary>Used trace files configuration</summary>
    public abstract class FilesConfiguration
    {
        /// <summary>Configuration file name</summary>
        public string FileName { get; set; }

        /// <summary>Collection of trace files needed for a blend</summary>
        protected Collection<TraceFile> Files { get; } = new Collection<TraceFile>();

        /// <summary>Configured trace file</summary>
        /// <param name="ext">Extension</param>
        public TraceFile this[string ext] => Files.FirstOrDefault(f => f.Ext.Equals(ext, StringComparison.OrdinalIgnoreCase));

        #region Data read

        /// <summary>Read the last valid value from the files configuration</summary>
        /// <param name="type">Serie type</param>
        /// <param name="defaultValue">Default value to use</param>
        /// <param name="index">Serie index</param>
        protected TTypeSerie ReadLastValue<TTypeSerie>(Enum type, TTypeSerie defaultValue, params int[] index) where TTypeSerie : struct
            => GetLastData<TTypeSerie>(type, index) ?? defaultValue;

        /// <summary>Read the last valid value from the files configuration</summary>
        /// <param name="type">Serie type</param>
        /// <param name="defaultValue">Default value to use</param>
        /// <param name="index">Serie index</param>
        protected TTypeSerie? ReadLastValue<TTypeSerie>(Enum type, TTypeSerie? defaultValue, params int[] index) where TTypeSerie : struct
            => GetLastData<TTypeSerie>(type, index) ?? defaultValue;

        /// <summary>Read the last valid value from the files configuration</summary>
        /// <param name="type">Serie type</param>
        /// <param name="defaultValue">Default value to use</param>
        /// <param name="function">Function to transform the read value</param>
        /// <param name="index">Serie index</param>
        /// <exception cref="ArgumentNullException"/>
        protected TTypeReturn ReadLastValue<TTypeReturn>(Enum type, TTypeReturn defaultValue, Func<string, TTypeReturn> function, params int[] index)
        {
            if (function == null)
                throw new ArgumentNullException(nameof(function));

            string temp = GetLastData(type, index);
            return temp != null ? function(temp) : defaultValue;
        }
        /// <summary>Read the last valid value from the files configuration</summary>
        /// <param name="type">Serie type</param>
        /// <param name="defaultValue">Default value to use</param>
        /// <param name="function">Function to transform the read value</param>
        /// <param name="index">Serie index</param>
        /// <exception cref="ArgumentNullException"/>
        protected TTypeReturn ReadLastValue<TTypeSerie, TTypeReturn>(Enum type, TTypeReturn defaultValue, Func<TTypeSerie, TTypeReturn> function, params int[] index) where TTypeSerie : struct
        {
            if (function == null)
                throw new ArgumentNullException(nameof(function));

            TTypeSerie? temp = GetLastData<TTypeSerie>(type, index);
            return temp.HasValue ? function(temp.Value) : defaultValue;
        }

        /// <summary>Read the first valid value from the files configuration</summary>
        /// <param name="type">Serie type</param>
        /// <param name="defaultValue">Default value to use</param>
        /// <param name="index">Serie index</param>
        protected TTypeSerie ReadFirstValue<TTypeSerie>(Enum type, TTypeSerie defaultValue, params int[] index) where TTypeSerie : struct
            => GetFirstData<TTypeSerie>(type, index) ?? defaultValue;

        /// <summary>Read the first valid value from the files configuration</summary>
        /// <param name="type">Serie type</param>
        /// <param name="defaultValue">Default value to use</param>
        /// <param name="index">Serie index</param>
        protected TTypeSerie? ReadFirstValue<TTypeSerie>(Enum type, TTypeSerie? defaultValue, params int[] index) where TTypeSerie : struct
            => GetFirstData<TTypeSerie>(type, index) ?? defaultValue;

        /// <summary>Read the first valid value from the files configuration</summary>
        /// <param name="type">Serie type</param>
        /// <param name="defaultValue">Default value to use</param>
        /// <param name="index">Serie index</param>
        protected string ReadFirstValue(Enum type, string defaultValue, params int[] index)
            => GetFirstData(type, index) ?? defaultValue;

        /// <summary>Read the first valid value from the files configuration</summary>
        /// <param name="type">Serie type</param>
        /// <param name="defaultValue">Default value to use</param>
        /// <param name="function">Function to transform the read value</param>
        /// <param name="index">Serie index</param>
        /// <exception cref="ArgumentNullException"/>
        protected TTypeReturn ReadFirstValue<TTypeReturn>(Enum type, TTypeReturn defaultValue, Func<string, TTypeReturn> function, params int[] index)
        {
            if (function == null)
                throw new ArgumentNullException(nameof(function));

            string temp = GetFirstData(type, index);
            return temp != null ? function(temp) : defaultValue;
        }

        /// <summary>Get the readed points table, for an information type</summary>
        /// <param name="type">Information type</param>
        /// <param name="index">Index</param>
        public TTypeSerie[] GetData<TTypeSerie>(Enum type, params int[] index)
        {
            BaseSerie<TTypeSerie> serie = GetSerie<TTypeSerie>(type, index);
            if (serie != null && serie.Readed)
                return serie.GetData();
            else
                return new TTypeSerie[] { };
        }

        #endregion
        #region Files and series read

        /// <summary>Read all the CSV files of the configuration</summary>
        /// <param name="baseFileName">Base of the file name</param>
        /// <remarks>All previous readed data will be cleared</remarks>
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="IOException">File alreaded opened</exception>
        /// <exception cref="UnauthorizedAccessException"/>
        /// <exception cref="NotSupportedException"/>
        public void ReadFiles(string baseFileName) => ReadFiles(baseFileName, true);

        /// <summary>Read all the CSV files of the configuration</summary>
        /// <param name="baseFileName">Base of the file name</param>
        /// <param name="init">Clear all previous readed data</param>
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="IOException">File alreaded opened</exception>
        /// <exception cref="UnauthorizedAccessException"/>
        /// <exception cref="NotSupportedException"/>
        public void ReadFiles(string baseFileName, bool init)
        {
            if (string.IsNullOrEmpty(baseFileName))
                throw new ArgumentNullException(nameof(baseFileName));

            foreach (FileCsv file in Files.OfType<FileCsv>())
            {
                try
                {
                    file.ReadFile(baseFileName, init);
                }
                catch (FileLoadException) { }
            }
        }

        /// <summary>Read the series of all the CSV files of the configuration</summary>
        /// <remarks>Match data among the files; be sure all file has a run serie defined</remarks>
        /// <exception cref="FileLoadException">Read problem</exception>
        /// <exception cref="RegexMatchTimeoutException"/>
        public void ReadSeries()
        {
            // Read each configured file
            foreach (FileCsv file in Files.OfType<FileCsv>())
            {
                try
                {
                    file.ReadSeries();
                }
                catch (FileLoadException) { } // No mandatory file
            }

            // Check the coherence of the runs count in each file
            SerieRun serieRunRef = Files.OfType<FileCsv>().Select(f => f.SerieRun).FirstOrDefault();

            int lastRunRef = serieRunRef.GetLastData() ?? 0;
            foreach (FileCsv file in Files.OfType<FileCsv>().Where(f => f.SeriesReaded))
            {
                // Vérification, et mise en cohérence le cas échéant, du nombre de données dans chaque série dans chaque fichier
                foreach (var serie in file.Series.Where(s => s.Readed))
                {
                    while (serie.Count < lastRunRef + 1)
                        serie.AddRun(serie.Count); // On ajoute des données vides jusqu'à atteindre le nombre voulu
                    while (serie.Count > lastRunRef + 1)
                        serie.RemoveLast(); // On supprime des données jusqu'à atteindre le nombre voulu
                }

                // Vérification de la taille de la série de numéro de passage
                if (!file.SerieRun.GetLastData().HasValue || file.SerieRun.GetLastData().Value != lastRunRef)
                    throw new FileLoadException(string.Format(CultureInfo.InvariantCulture, Resources.Resources.RunsCantBeProcessedIntoFile, file.FileName));
            }
        }

        #endregion
        #region Private methods

        /// <summary>Get the serie, for an information type</summary>
        /// <param name="type">Information type</param>
        /// <param name="index">Index</param>
        /// <exception cref="ArgumentNullException"/>
        private BaseSerie<TTypeSerie> GetSerie<TTypeSerie>(Enum type, int[] index)
            => Files
                .OfType<FileCsv>()
                .SelectMany(f => f.Series)
                .OfType<BaseSerie<TTypeSerie>>()
                .Where(s => s.Readed && s.Type.Equals(type) && s.MatchIndex(index))
                .FirstOrDefault();

        /// <summary>Get the first readed point, for an information type</summary>
        /// <param name="type">Information type</param>
        /// <param name="index">Index</param>
        /// <exception cref="ArgumentNullException"/>
        private TTypeSerie? GetFirstData<TTypeSerie>(Enum type, int[] index) where TTypeSerie : struct
            => GetSerie<TTypeSerie?>(type, index)?.GetFirstData();

        /// <summary>Get the first readed point, for an information type</summary>
        /// <param name="type">Information type</param>
        /// <param name="index">Index</param>
        /// <exception cref="ArgumentNullException"/>
        private string GetFirstData(Enum type, int[] index)
            => GetSerie<string>(type, index)?.GetFirstData();

        /// <summary>Get the last readed point, for an information type</summary>
        /// <param name="type">Information type</param>
        /// <param name="index">Index</param>
        /// <exception cref="ArgumentNullException"/>
        private TTypeSerie? GetLastData<TTypeSerie>(Enum type, int[] index) where TTypeSerie : struct
            => GetSerie<TTypeSerie?>(type, index)?.GetLastData();

        /// <summary>Get the last readed point, for an information type</summary>
        /// <param name="type">Information type</param>
        /// <param name="index">Index</param>
        /// <exception cref="ArgumentNullException"/>
        private string GetLastData(Enum type, int[] index)
            => GetSerie<string>(type, index)?.GetLastData();
        
        #endregion
    }
}
