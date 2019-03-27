using System.Collections.Generic;
using System.Threading.Tasks;

namespace MilNet.Core.Services
{
    public interface IRequestProvider
    {
        Task<TResult> GetAsync<TResult>(string uri);
        Task<TResult> GetAsync<TResult>(string uri, string bearerToken);
        Task<TResult> GetAsync<TResult>(string uri, string bearerToken, IDictionary<string, string> headers);

        Task PostAsync<TResult>(string uri, TResult data);
        Task PostAsync<TResult>(string uri, TResult data, string bearerToken);
        Task PostAsync<TResult>(string uri, TResult data, string bearerToken, IDictionary<string, string> headers);

        Task<TResult> PostAsync<TData, TResult>(string uri, TData data);
        Task<TResult> PostAsync<TData, TResult>(string uri, TData data, string bearerToken);
        Task<TResult> PostAsync<TData, TResult>(string uri, TData data, string bearerToken, IDictionary<string, string> headers);
    }
}
