using MilNet.Core.Exceptions;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace MilNet.Core.Services
{
    public class RequestProvider : IRequestProvider
    {
        private readonly JsonSerializerSettings serializerSettings;
        private readonly IHttpClientFactory httpClientFactory;

        public RequestProvider(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;

            serializerSettings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                DateTimeZoneHandling = DateTimeZoneHandling.Utc,
                NullValueHandling = NullValueHandling.Ignore
            };
            serializerSettings.Converters.Add(new StringEnumConverter());
        }


        public Task<TResult> GetAsync<TResult>(string uri)
            => GetAsync<TResult>(uri, null, null);

        public Task<TResult> GetAsync<TResult>(string uri, string bearerToken)
            => GetAsync<TResult>(uri, bearerToken, null);

        public async Task<TResult> GetAsync<TResult>(string uri, string bearerToken, IDictionary<string, string> headers)
        {
            var httpClient = CreateHttpClient(bearerToken, headers);
            HttpResponseMessage response = await httpClient.GetAsync(uri);

            string serialized = await HandleResponse(response);
            TResult result = await Task.Run(() => JsonConvert.DeserializeObject<TResult>(serialized, serializerSettings));

            return result;
        }

        public Task PostAsync<TData>(string uri, TData data)
            => PostAsync(uri, data, null, null);

        public Task PostAsync<TData>(string uri, TData data, string bearerToken)
            => PostAsync(uri, data, bearerToken, null);

        public async Task PostAsync<TData>(string uri, TData data, string bearerToken, IDictionary<string, string> headers)
        {
            var httpClient = CreateHttpClient(bearerToken, headers);
            var content = new StringContent(JsonConvert.SerializeObject(data));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpResponseMessage response = await httpClient.PostAsync(uri, content);

            await HandleResponse(response);
        }

        public Task<TResult> PostAsync<TData, TResult>(string uri, TData data)
            => PostAsync<TData, TResult>(uri, data, null, null);

        public Task<TResult> PostAsync<TData, TResult>(string uri, TData data, string bearerToken)
            => PostAsync<TData, TResult>(uri, data, bearerToken, null);

        public async Task<TResult> PostAsync<TData, TResult>(string uri, TData data, string bearerToken, IDictionary<string, string> headers)
        {
            var httpClient = CreateHttpClient(bearerToken, headers);
            var content = new StringContent(JsonConvert.SerializeObject(data));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpResponseMessage response = await httpClient.PostAsync(uri, content);

            string serialized = await HandleResponse(response);
            TResult result = await Task.Run(() => JsonConvert.DeserializeObject<TResult>(serialized, serializerSettings));

            return result;
        }
        
        private HttpClient CreateHttpClient(string bearerToken, IDictionary<string, string> headers)
        {
            var httpClient = httpClientFactory.CreateClient();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            if (!string.IsNullOrEmpty(bearerToken))
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", bearerToken);
            }

            if (headers != null)
            {
                foreach (var header in headers)
                {
                    httpClient.DefaultRequestHeaders.Add(header.Key, header.Value);
                }
            }

            return httpClient;
        }

        private async Task<string> HandleResponse(HttpResponseMessage response)
        {
            var content = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                if (response.StatusCode == HttpStatusCode.Forbidden || response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    throw new ForbiddenException(content);
                }
                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    throw new NotFoundException(content);
                }

                throw new HttpRequestExceptionEx(response.StatusCode, content);
            }
            return content;
        }
    }
}
