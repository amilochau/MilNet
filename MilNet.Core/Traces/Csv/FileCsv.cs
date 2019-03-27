using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace MilNet.Core.Traces.Csv
{
    /// <summary>Data trace file from a CSV formatted file</summary>
    public class FileCsv : TraceFile
    {
        /// <summary>File type</summary>
        public override TypeFile Type => TypeFile.Csv;

        /// <summary>File is readed</summary>
        public bool FileReaded { get; private set; } = false;
        /// <summary>Series are readed</summary>
        public bool SeriesReaded { get; private set; } = false;

        /// <summary>Series associated to the files</summary>
        public Collection<ISerie> Series { get; } = new Collection<ISerie>();

        /// <summary>Serie of runs</summary>
        public SerieRun SerieRun { get; }

        /// <summary>File lines</summary>
        public Collection<string> Lines { get; private set; }

        /// <summary>Constructor</summary>
        /// <param name="ext">File extension</param>
        /// <param name="serieRun">Serie of runs</param>
        /// <exception cref="ArgumentNullException">Argument empty or null</exception>
        public FileCsv(string ext, SerieRun serieRun) : base(ext)
        {
            if (string.IsNullOrEmpty(ext))
                throw new ArgumentNullException(nameof(ext));
            if (serieRun == null)
                throw new ArgumentNullException(nameof(serieRun));

            SerieRun = serieRun;
            Series.Add(serieRun);
        }

        /// <summary>Read the CSV file</summary>
        /// <param name="baseFileName">Absolute path of the file to read, without extension</param>
        /// <param name="init">Clear all previous readed data before reading the CSV file</param>
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="FileLoadException">No valid line found into the file</exception>
        /// <exception cref="IOException">I/O Exception</exception>
        /// <exception cref="UnauthorizedAccessException"/>
        /// <exception cref="NotSupportedException"/>
        public void ReadFile(string baseFileName, bool init)
        {
            if (string.IsNullOrEmpty(baseFileName))
                throw new ArgumentNullException(nameof(baseFileName));

            FileReaded = false;

            // Initialisation
            if (init)
                CleanData();

            // Vérification de l'existence du fichier
            BaseFileName = baseFileName;
            if (!File.Exists(FileName))
                return; // Aucun fichier n'est obligatoire

            string[] lines = File.ReadAllLines(FileName);

            // Vérification de fichier vide
            if (lines.Count() == 0)
                throw new FileLoadException(null, FileName);

            // Mise en mémoire du fichier
            Lines = new Collection<string>(lines);

            FileReaded = true;
        }

        /// <summary>Interpret the CSB file lines</summary>
        /// <remarks>Data of the whole lines will be saved</remarks>
        /// <exception cref="FileLoadException">Read problem</exception>
        /// <exception cref="RegexMatchTimeoutException"/>
        public void ReadSeries()
        {
            SeriesReaded = false;
            int columnsCount = 0;
            CultureInfo culture = CultureInfo.CreateSpecificCulture("en-US");
            CultureInfo cultureDate = CultureInfo.CreateSpecificCulture("fr-FR");

            // Check the files read
            if (!FileReaded)
                return; // TODO Pas de problème aujourd'hui, à voir ensuite

            // Vérification de la présence d'une série de numéros de passage, détermination du dernier passage lu le cas échéant
            int lastRunsCount = SerieRun.Count > 0 ? SerieRun.Count + 1 : 0;

            string firstLine = Lines[0];
            string[] data;
            
            data = firstLine.Split(';'); // Données du header
            columnsCount = data.Count();
            foreach (ISerie serie in Series) // Attribution des numéros de colonnes à chaque série
            {
                if (serie.IsRegexHeader)
                    serie.ColumnIndex = Array.FindLastIndex(data, s => new Regex(serie.Header, RegexOptions.IgnoreCase).Match(s).Success);
                else
                    serie.ColumnIndex = Array.LastIndexOf(data, serie.Header);
            }

            // Vérification que la série de numéro de passages a bien été trouvée
            if (!SerieRun.Readed)
                throw new FileLoadException(string.Format(CultureInfo.InvariantCulture, Resources.Resources.SerieOfRunsNotFoundIntoFile, FileName));

            // Lecture des lignes suivantes
            for (int i = 1; i < Lines.Count; i++)
            {
                string line = Lines[i];

                data = line.Split(';'); // Données de la ligne

                // Vérification du nombre de colonnes
                if (data.Count() != columnsCount)
                    continue; // La ligne ne peut pas être lue : on continue (on ajoutera des données vides au prochain passage, le cas échéant

                // Vérification de la cohérence des passages
                int currentRun;
                if (!int.TryParse(data[SerieRun.ColumnIndex], NumberStyles.Float, culture, out currentRun))
                    break; // Si on ne peut pas lire la série des passages, on ne peut pas lire le reste du fichier
                currentRun += lastRunsCount;
                if (currentRun > 0 && SerieRun.Count == 0)
                {
                    // Les premiers passages (dont le passage 0) n'ont pas été lus :
                    // On ajoute des données vides pour les premiers passages
                    while (currentRun > SerieRun.Count)
                    {
                        foreach (ISerie serie in Series)
                            serie.AddRun(SerieRun.Count); // La ligne intermédiaire n'a pas été lue : on incrémente le numéro de passage
                    }
                }

                if (currentRun < SerieRun.Count - 1)
                    continue; // La ligne a déjà été lue : on continue

                while (currentRun > SerieRun.Count)
                {
                    foreach (ISerie serie in Series)
                        serie.AddRun(serie.Count); // La ligne intermédiaire n'a pas été lue : on incrémente le numéro de passage
                }
                if (currentRun != SerieRun.Count)
                    throw new FileLoadException(string.Format(CultureInfo.InvariantCulture, Resources.Resources.RunsCantBeProcessedIntoFile, FileName)); // On ne peut vraiment plus rien faire pour réconcilier les lignes

                // Attribution des données à chaque série
                foreach (ISerie serie in Series.Where(s => s.Readed))
                    serie.Add(data[serie.ColumnIndex], culture, cultureDate);
            }

            // Vérification de la cohérence du nombre de données dans chaque série
            int lastRun = SerieRun.GetLastData() ?? 0;
            int runsCount = lastRun + 1;
            foreach (ISerie serie in Series.Where(s => s.Readed))
            {
                while (serie.Count < runsCount)
                    serie.AddNull(); // On ajoute des données vides jusqu'à atteindre le nombre voulu
                while (serie.Count > runsCount)
                    serie.RemoveLast(); // On supprime des données jusqu'à atteindre le nombre voulu
            }

            SeriesReaded = true; // On arrive ici si aucune exception n'a été levée pendant toute la lecture
        }
        
        /// <summary>Clear all previous readed data</summary>
        /// <remarks>Configuration is maintained</remarks>
        public override void CleanData()
        {
            FileReaded = false;
            SeriesReaded = false;
            // Nettoyage des lignes du fichier
            BaseFileName = null;
            Lines?.Clear();
            // Nettoyage des données de séries
            foreach (ISerie serie in Series)
                serie.CleanData();
        }
    }
}
