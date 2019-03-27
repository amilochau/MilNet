using System.Collections.Generic;

namespace MilNet.Services.Models
{
    public abstract class BaseEmail
    {
        public List<EmailAddress> Tos { get; set; }

        public List<EmailAddress> Ccs { get; set; }

        public EmailAddress ReplyTo { get; set; }

        public EmailContext Context { get; set; }
    }
}
