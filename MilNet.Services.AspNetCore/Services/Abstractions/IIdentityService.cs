using MilNet.Services.Models;
using System.Threading.Tasks;

namespace MilNet.Services.AspNetCore.Services.Abstractions
{
    public interface IIdentityService
    {
        Task<Contacts> GetContactsAsync();
    }
}
