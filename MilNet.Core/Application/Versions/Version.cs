using System.Collections.ObjectModel;

namespace MilNet.Core.Application.Versions
{
    /// <summary>Version details</summary>
    [System.Obsolete("Will be removed in version 3. Please use standard .NET Core features instead.")]
    public class Version
    {
        /// <summary>Version number</summary>
        public System.Version Number { get; protected internal set; }
        /// <summary>List of version details item</summary>
        public Collection<Item> Items { get; } = new Collection<Item>();
    }
}
