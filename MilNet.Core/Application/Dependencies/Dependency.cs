namespace MilNet.Core.Application.Dependencies
{
    /// <summary>Dependency</summary>
    [System.Obsolete("Will be removed in version 3. Please use standard .NET Core features instead.")]
    public class Dependency
    {
        /// <summary>Name</summary>
        public string Name { get; protected internal set; }
        /// <summary>Version</summary>
        public string Version { get; protected internal set; }
        /// <summary>Authors</summary>
        public string Authors { get; protected internal set; }
        /// <summary>License</summary>
        public string License { get; protected internal set; }
        /// <summary>Existing license</summary>
        public bool HasLicense => !string.IsNullOrEmpty(License);
    }
}
