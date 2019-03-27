namespace MilNet.Services.AspNetCore.Configuration
{
    public class ContactOptions
    {
        /// <summary>Name for current service to get contacts list</summary>
        public string Name { get; set; }
        public ContactCategoryOptions Business { get; set; }
        public ContactCategoryOptions Technical { get; set; }
    }
}
