using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Xml;
using MilNet.Core.Application.Dependencies;
using MilNet.Core.Application.Versions;

namespace MilNet.Core.Application
{
    /// <summary>File containing application details</summary>
    [System.Obsolete("Will be removed in version 3. Please use standard .NET Core features instead.")]
    public class ApplicationFile : IApplicationFile
    {
        /// <summary>List of versions</summary>
        public Collection<Version> Versions { get; } = new Collection<Version>();

        /// <summary>List of dependencies</summary>
        public Collection<Dependency> Dependencies { get; } = new Collection<Dependency>();

        /// <summary>Read a file containing application details</summary>
        /// <param name="fileName">File to read</param>
        /// <exception cref="System.ArgumentNullException"/>
        /// <exception cref="FileNotFoundException"/>
        /// <exception cref="IOException"/>
        public void Read(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
                throw new System.ArgumentNullException(nameof(fileName));

            // Clear dependencies, versions list
            Dependencies.Clear();
            Versions.Clear();

            // Test if the file exists
            if (!File.Exists(fileName))
                throw new FileNotFoundException(null, fileName);

            // TODO use another read method (one-time)

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
                        case "DEPENDENCY":
                            Dependencies.Add(new Dependency()
                            {
                                Name = reader.GetAttribute("name"),
                                Version = reader.GetAttribute("version"),
                                Authors = reader.GetAttribute("authors"),
                                License = reader.GetAttribute("license")
                            });
                            break;
                    }
                }
            }
        }
    }
}
