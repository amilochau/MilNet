using System.Collections.Generic;

namespace MilNet.Services.Models
{
    public class ContactCategory
    {
        public List<ContactUser> Users { get; set; }
        public string Email { get; set; }
        public string Place { get; set; }
        public string Url { get; set; }
    }
}
