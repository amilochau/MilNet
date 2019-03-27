namespace MilNet.Services.Models
{
    public class ContactOptions
    {
        public ContactCategoryOptions Business { get; set; } = new ContactCategoryOptions();
        public ContactCategoryOptions Technical { get; set; } = new ContactCategoryOptions();
    }
}
