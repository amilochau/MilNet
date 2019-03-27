using Microsoft.Extensions.Options;
using MilNet.Core.Services;
using MilNet.Services.AspNetCore.Configuration;
using MilNet.Services.AspNetCore.Services.Abstractions;
using MilNet.Services.Models;
using System.Threading.Tasks;

namespace MilNet.Services.AspNetCore.Services
{
    public class IdentityService<TOptions> : IIdentityService
        where TOptions : ServicesOptions, new()
    {
        private readonly IRequestProvider requestProvider;
        private readonly ServicesOptions options;

        public IdentityService(IRequestProvider requestProvider,
            IOptions<TOptions> options)
        {
            this.requestProvider = requestProvider;
            this.options = options.Value;
        }

        public async Task<Contacts> GetContactsAsync()
        {
            string uri = $"{options.IdentityUrl}/api/contacts/contacts/{options.Contact.Name}";
            var contacts = await requestProvider.GetAsync<Contacts>(uri);

            contacts.Business.Email = options.Contact.Business.Email;
            contacts.Business.Place = options.Contact.Business.Place;
            contacts.Business.Url = options.Contact.Business.Url;
            contacts.Technical.Email = options.Contact.Technical.Email;
            contacts.Technical.Place = options.Contact.Technical.Place;
            contacts.Technical.Url = options.Contact.Technical.Url;

            return contacts;
        }
    }
}
