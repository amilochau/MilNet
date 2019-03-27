using System.Collections.ObjectModel;
using System.IO;
using System.Xml;

namespace MilNet.Core.Application.Dependencies
{
    /// <summary>Files containing details of dependencies</summary>
    [System.Obsolete("Will be removed in version 3. Please use standard .NET Core features instead.")]
    public class DependenciesFile : IDependenciesFile
    {
        /// <summary>List of dependencies</summary>
        public Collection<Dependency> Dependencies { get; } = new Collection<Dependency>();

        /// <summary>Read a file containing dependencies details</summary>
        /// <param name="fileName">File name</param>
        /// <exception cref="System.ArgumentNullException"/>
        /// <exception cref="FileNotFoundException"/>
        /// <exception cref="IOException"/>
        public void Read(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
                throw new System.ArgumentNullException(fileName);

            // Clear dependencies list
            Dependencies.Clear();

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
