using System.Collections.Generic;

namespace MilNet.Services.Models
{
    public class ContactCategoryOptions
    {
        public List<ContactUserOptions> Users { get; set; } = new List<ContactUserOptions>();
        public string Email { get; set; }
        public string Place { get; set; }
        public string Url { get; set; }
    }
}
