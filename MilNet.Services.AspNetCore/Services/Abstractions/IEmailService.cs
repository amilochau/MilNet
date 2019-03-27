using MilNet.Services.Models;
using System.Threading.Tasks;

namespace MilNet.Services.AspNetCore.Services.Abstractions
{
    public interface IEmailService
    {
        Task SendEmailAsync(Email email);
        Task SendEmailAsync(Email email, string culture);

        Task SendEmailWithTemplateAsync(EmailTemplate emailTemplate);
        Task SendEmailWithTemplateAsync(EmailTemplate emailTemplate, string culture);

        Task<bool> CheckDomainAsync(string email);
    }
}
