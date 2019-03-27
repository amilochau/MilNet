using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Xml;

namespace MilNet.Core.Application.Versions
{
    /// <summary>File containing versions details</summary>
    [System.Obsolete("Will be removed in version 3. Please use standard .NET Core features instead.")]
    public class VersionsFile : IVersionsFile
    {
        /// <summary>List of versions</summary>
        public Collection<Version> Versions { get; } = new Collection<Version>();

        /// <summary>Read a file containing versions details</summary>
        /// <param name="fileName">Filename</param>
        /// <exception cref="System.ArgumentNullException"/>
        /// <exception cref="FileNotFoundException"/>
        /// <exception cref="IOException"/>
        public void Read(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
                throw new System.ArgumentNullException(nameof(fileName));

            // Clear versions list
            Versions.Clear();

            // Test if the file exists
            if (!File.Exists(fileName))
                throw new FileNotFoundException(null, fileName);

            // Read file content
            using (XmlTextReader reader = new XmlTextReader(fileName))
            {
                while (reader.Read())
	            {
	                if (reader.NodeType != XmlNodeType.Element)
                        continue;

                    if (string.IsNullOrEmpty(reader.Name))
                        continue;

                    switch (reader.Name.Trim().ToUpperInvariant())
                    {
                        case "VERSION":
                            System.Version versionNumber;
                            System.Version.TryParse(reader.GetAttribute("num"), out versionNumber);
                            Versions.Add(new Version()
                            {
                                Number = versionNumber
                            });
                            break;
                        case "ITEM":
                            Versions.Last().Items.Add(new Item()
                            {
                                Text = reader.GetAttribute("type"),
                                Description = reader.GetAttribute("desc")
                            });
                            break;
                    }
	            }
            }
        }
    }
}
