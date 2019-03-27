namespace MilNet.Services.Models
{
    public class EmailTemplate : BaseEmail
    {
        public EmailTemplateType TemplateType { get; set; }

        public string CallToActionUrl { get; set; }
    }
}
