namespace MilNet.Services.Models
{
    /// <summary>Options to use MilNet Services</summary>
    public abstract class ServicesOptions
    {
        public virtual string ApplicationName { get; set; }

        public virtual LogOptions Log { get; set; } = new LogOptions();

        public virtual ContactOptions Contact { get; set; } = new ContactOptions();

        public virtual ReleaseOptions Release { get; set; } = new ReleaseOptions();

        public virtual DocumentationOptions Documentation { get; set; } = new DocumentationOptions();
    }
}
