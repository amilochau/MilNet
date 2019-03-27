namespace MilNet.Services.AspNetCore.Configuration
{
    /// <summary>
    /// Options to use MilNet Services.
    /// 
    /// <list type="bullet">
    /// <item>
    /// <description>To use Feedbacks, define <see cref="FeedbacksName"/> and <see cref="FeedbacksUrl"/>; <see cref="IdentityUrl"/>, <see cref="IdentitySecret"/> and <see cref="ApplicationName"/> are used to get token</description>
    /// </item>
    /// </list>
    /// </summary>
    public abstract class ServicesOptions
    {
        /// <summary>Name for Identity service</summary>
        public virtual string IdentityName { get; set; }

        /// <summary>URL for Identity service</summary>
        public virtual string IdentityUrl { get; set; }

        /// <summary>Secret for Identity service to get tokens</summary>
        public string IdentitySecret { get; set; }

        /// <summary>Name for Feedbacks service</summary>
        public virtual string FeedbacksName { get; set; }

        /// <summary>URL for Feedbacks service</summary>
        public virtual string FeedbacksUrl { get; set; }

        /// <summary>Name for Emails service</summary>
        public virtual string EmailsName { get; set; }

        /// <summary>URL for Emails service</summary>
        public virtual string EmailsUrl { get; set; }
        
        /// <summary>Name for current service</summary>
        public string ApplicationName { get; set; }

        /// <summary>URL for current service</summary>
        public string ApplicationUrl { get; set; }

        public LogOptions Log { get; set; } = new LogOptions(); 
        
        /// <summary>Contact information</summary>
        public ContactOptions Contact { get; set; } = new ContactOptions();

        /// <summary>Release information</summary>
        public ReleaseOptions Release { get; set; } = new ReleaseOptions();
    }
}
