using Microsoft.Extensions.Options;
using MilNet.Core.Services;
using MilNet.Services.AspNetCore.Configuration;
using MilNet.Services.AspNetCore.Services.Abstractions;
using MilNet.Services.Models;
using System.Threading.Tasks;

namespace MilNet.Services.AspNetCore.Services
{
    /// <summary>Feedback service</summary>
    public class FeedbackService<TOptions> : IFeedbackService
        where TOptions : ServicesOptions, new()
    {
        private readonly ServicesOptions options;
        private readonly ITokenService tokenService;
        private readonly IRequestProvider requestProvider;

        public FeedbackService(IOptions<TOptions> options,
            ITokenService tokenService,
            IRequestProvider requestProvider)
        {
            this.options = options.Value;
            this.tokenService = tokenService;
            this.requestProvider = requestProvider;
        }

        /// <summary>Create new feedback</summary>
        public async Task CreateAsync(Feedback feedback)
        {
            // Normalize rating
            feedback.Rating /= 5; // 5 stars = 100%

            string token = await tokenService.GetTokenAsync(options.FeedbacksName);
            await requestProvider.PostAsync($"{options.FeedbacksUrl}/api/feedbacks", feedback, token);
        }
    }
}
