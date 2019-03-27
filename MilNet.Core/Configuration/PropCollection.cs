using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;
using System.Globalization;

namespace MilNet.Core.Configuration
{
    /// <summary>Collection of configuration properties</summary>
    [System.Obsolete("Will be removed in version 3. Please use standard .NET Core features instead.")]
    public class PropCollection : ICollection<Prop>
    {
        private List<Prop> properties = new List<Prop>();
        private const string VERSION_CONFIGURATION_FILE = "1.0.0.0";
        
        /// <summary>Get or set the configuration property with the specified <param name="name"/></summary>
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="InvalidOperationException">The property has not been found</exception>
        public Prop this[string name]
        {
            get
            {
                if (string.IsNullOrEmpty(name))
                    throw new ArgumentNullException(nameof(name));
                return properties.First(p => p.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
            }
        }

        /// <summary>Save the properties collection into an XML file</summary>
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="DirectoryNotFoundException"/>
        /// <exception cref="IOException"/>
        /// <exception cref="UnauthorizedAccessException"/>
        /// <exception cref="NotSupportedException"/>
        /// <exception cref="System.Security.SecurityException"/>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1303", MessageId = "System.Xml.XmlWriter.WriteComment(System.String)")]
        internal void Save(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
                throw new ArgumentNullException(nameof(fileName));

            string directoryName = Path.GetDirectoryName(fileName);

            // Create configuration directory, if needed
            if (!Directory.Exists(directoryName))
                Directory.CreateDirectory(directoryName);
            
            // Check if the configuration directory exists
            if (!Directory.Exists(directoryName))
                throw new DirectoryNotFoundException(string.Format(CultureInfo.InvariantCulture, Resources.Resources.FolderNotFoundSettingsCantBeSaved, directoryName));

            using (XmlTextWriter writer = new XmlTextWriter(fileName, Encoding.UTF8))
            {
                writer.Formatting = Formatting.Indented;

                // File start
                writer.WriteStartDocument(false);
                writer.WriteComment("Application settings");

                // Main content
                writer.WriteStartElement("CONFIG");
                writer.WriteAttributeString("description", "List of settings");
                writer.WriteAttributeString("version", VERSION_CONFIGURATION_FILE);

                foreach (Prop property in properties)
                {
                    writer.WriteStartElement("PROP");
                    writer.WriteAttributeString("nom", property.Name);
                    writer.WriteAttributeString("valeur", property.ToString());
                    writer.WriteEndElement();
                }
                
                writer.WriteEndElement();
            }
        }

        /// <summary>Open an XML file to fill the properties collection</summary>
        /// <param name="fileName">File name</param>
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="FileNotFoundException"/>
        /// <exception cref="IOException"/>
        /// <exception cref="NotSupportedException"/>
        public void Open(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
                throw new ArgumentNullException(nameof(fileName));

            if (!File.Exists(fileName))
                throw new FileNotFoundException(string.Format(CultureInfo.InvariantCulture, Resources.Resources.FileNotFoundSettingsCantBeLoaded, fileName));

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
                        case "CONFIG":
                            // TODO Check the file version
                            break;
                        case "PROP":
                            Prop prop = properties.FirstOrDefault(p => p.Name.Equals(reader.GetAttribute("nom"), StringComparison.OrdinalIgnoreCase));
                            if (prop != null)
                                prop.FromString(reader.GetAttribute("valeur"));
                            break;
                        default:
                            break;
                    }
                }
                reader.Close();
            }
        }

        /// <summary>Fill properties values with default values</summary>
        public void UseDefaultValues() => properties.ForEach(p => p.UseDefaultValue());

        #region "ICollection<Prop>"

        /// <summary>Properties count</summary>
        public int Count => properties.Count;

        /// <summary>Read only</summary>
        public bool IsReadOnly => false;

        /// <summary>Add a new property into collection</summary>
        public void Add(Prop item) => properties.Add(item);

        /// <summary>Clear properties collection</summary>
        public void Clear() => properties.Clear();

        /// <summary>Contains</summary>
        public bool Contains(Prop item) => properties.Any(p => p.Equals(item));

        /// <summary>Copy to a properties array</summary>
        /// <exception cref="NotImplementedException"/>
        public void CopyTo(Prop[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }
        /// <summary>Properties enumerator</summary>
        /// <exception cref="NotImplementedException"/>
        public IEnumerator<Prop> GetEnumerator()
        {
            throw new NotImplementedException();
        }
        /// <summary>Remove a property from collection</summary>
        public bool Remove(Prop item) => properties.Remove(item);

        /// <summary>Enumerator</summary>
        IEnumerator IEnumerable.GetEnumerator() => properties.GetEnumerator();

        #endregion
    }
}
