using Microsoft.Extensions.Options;
using MilNet.Services.AspNetCore.Configuration;
using MilNet.Services.AspNetCore.Services.Abstractions;
using MilNet.Services.Models;
using System.Threading.Tasks;

namespace MilNet.Services.AspNetCore.Services
{
    public class ApplicationService<TOptions> : IApplicationService
        where TOptions : ServicesOptions, new()
    {
        private readonly ServicesOptions options;
        private readonly IFeedbackService feedbackService;
        private readonly IIdentityService identityService;

        public ApplicationService(IFeedbackService feedbackService,
            IIdentityService identityService,
            IOptions<TOptions> options)
        {
            this.options = options.Value;
            this.feedbackService = feedbackService;
            this.identityService = identityService;
        }

        public ReleaseOptions GetReleaseOptions()
        {
            return options.Release;
        }

        public Task SendFeedbackAsync(Feedback feedback)
        {
            return feedbackService.CreateAsync(feedback);
        }

        public Task<Contacts> GetContactsAsync()
        {
            return identityService.GetContactsAsync();
        }
    }
}
