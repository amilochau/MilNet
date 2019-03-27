using System.Threading.Tasks;

namespace MilNet.Services.AspNetCore.Services.Abstractions
{
    /// <summary>Tokens service</summary>
    public interface ITokenService
    {
        /// <summary>Get token from Identity for a specifiedscope</summary>
        Task<string> GetTokenAsync(string scope);
    }
}
