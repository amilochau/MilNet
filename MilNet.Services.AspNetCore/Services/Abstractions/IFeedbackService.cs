using MilNet.Services.Models;
using System.Threading.Tasks;

namespace MilNet.Services.AspNetCore.Services.Abstractions
{
    /// <summary>Feedback service</summary>
    public interface IFeedbackService
    {
        /// <summary>Create new feedback</summary>
        Task CreateAsync(Feedback feedback);
    }
}
