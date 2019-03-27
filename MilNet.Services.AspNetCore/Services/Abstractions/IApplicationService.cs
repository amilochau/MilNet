using MilNet.Services.AspNetCore.Configuration;
using MilNet.Services.Models;
using System.Threading.Tasks;

namespace MilNet.Services.AspNetCore.Services.Abstractions
{
    public interface IApplicationService
    {
        ReleaseOptions GetReleaseOptions();

        Task SendFeedbackAsync(Feedback feedback);

        Task<Contacts> GetContactsAsync();
    }
}
