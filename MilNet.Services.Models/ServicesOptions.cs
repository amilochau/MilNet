namespace MilNet.Services.Models
{
    /// <summary>Options to use MilNet Services</summary>
    public abstract class ServicesOptions
    {
        public virtual string ApplicationName { get; set; }

        public virtual LogOptions Log { get; set; } = new LogOptions();

        /// <summary>Contact information</summary>
        public virtual ContactOptions Contact { get; set; } = new ContactOptions();

        /// <summary>Release information</summary>
        public virtual ReleaseOptions Release { get; set; } = new ReleaseOptions();
    }
}
