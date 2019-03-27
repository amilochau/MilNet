using Microsoft.Extensions.Options;
using MilNet.Core.Services;
using MilNet.Services.AspNetCore.Configuration;
using MilNet.Services.AspNetCore.Services.Abstractions;
using MilNet.Services.Models;
using System.Collections.Generic;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace MilNet.Services.AspNetCore.Services
{
    public class EmailService<TOptions> : IEmailService
        where TOptions : ServicesOptions, new()
    {
        private readonly ITokenService tokenService;
        private readonly IRequestProvider requestProvider;
        private readonly ServicesOptions options;

        public EmailService(ITokenService tokenService,
            IRequestProvider requestProvider,
            IOptions<TOptions> options)
        {
            this.tokenService = tokenService;
            this.requestProvider = requestProvider;
            this.options = options.Value;
        }

        public Task SendEmailAsync(Email email)
            => SendEmailAsync(email, culture: "en");

        public async Task SendEmailAsync(Email email, string culture)
        {
            string token = await tokenService.GetTokenAsync(options.EmailsName);
            await requestProvider.PostAsync($"{options.EmailsUrl}/api/emails", email, token, new Dictionary<string, string>
            {
                { "Accept-Language", culture }
            });
        }

        public Task SendEmailWithTemplateAsync(EmailTemplate emailTemplate)
            => SendEmailWithTemplateAsync(emailTemplate, culture: "en");

        public async Task SendEmailWithTemplateAsync(EmailTemplate emailTemplate, string culture)
        {
            string token = await tokenService.GetTokenAsync(options.EmailsName);
            await requestProvider.PostAsync($"{options.EmailsUrl}/api/emails/WithTemplate", emailTemplate, token, new Dictionary<string, string>
            {
                { "Accept-Language", culture }
            });
        }

        public async Task<bool> CheckDomainAsync(string email)
        {
            var uri = $"{options.EmailsUrl}/api/emails/checkDomain?email={UrlEncoder.Default.Encode(email)}";

            string token = await tokenService.GetTokenAsync(options.EmailsName);
            return await requestProvider.GetAsync<bool>(uri, token);
        }
    }
}
