using IdentityModel.Client;
using Microsoft.Extensions.Options;
using MilNet.Services.AspNetCore.Configuration;
using MilNet.Services.AspNetCore.Services.Abstractions;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace MilNet.Services.AspNetCore.Services
{
    public class TokenService<TOptions> : ITokenService
        where TOptions : ServicesOptions, new()
    {
        private readonly ServicesOptions options;
        private readonly IHttpClientFactory httpClientFactory;

        public TokenService(IOptions<TOptions> options,
            IHttpClientFactory httpClientFactory)
        {
            this.options = options.Value;
            this.httpClientFactory = httpClientFactory;
        }

        /// <summary>Get token from Identity</summary>
        /// <param name="scope">Service name</param>
        public async Task<string> GetTokenAsync(string scope)
        {
            var httpClient = httpClientFactory.CreateClient();
            var discoveryResponse = await httpClient.GetDiscoveryDocumentAsync(new DiscoveryDocumentRequest
            {
                Address = options.IdentityUrl,
                Policy =
                {
                    RequireHttps = false
                }
            });
            
            if (discoveryResponse.IsError)
            {
                throw new Exception(discoveryResponse.Error);
            }

            var tokenResponse = await httpClient.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
            {
                Address = discoveryResponse.TokenEndpoint,
                ClientId = options.ApplicationName,
                ClientSecret = options.IdentitySecret
            });
            if (tokenResponse.IsError)
            {
                throw new Exception(tokenResponse.Error);
            }

            return tokenResponse.AccessToken;
        }
    }
}
