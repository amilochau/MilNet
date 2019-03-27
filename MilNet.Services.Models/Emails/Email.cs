namespace MilNet.Services.Models
{
    public class Email : BaseEmail
    {
        public string Subject { get; set; }

        public string Message { get; set; }

        public bool IsHtml { get; set; }
    }
}
