namespace MilNet.Services.Models
{
    /// <summary>Application release options</summary>
    /// <remarks>These settings are typically set up in a Release pipeline</remarks>
    public class ReleaseOptions
    {
        /// <summary>Release definition</summary>
        public string Definition { get; set; }

        /// <summary>Release name</summary>
        public string Name { get; set; }
    }
}
