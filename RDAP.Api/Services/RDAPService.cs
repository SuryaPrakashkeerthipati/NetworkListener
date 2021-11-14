using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;
using RDAP.Api.Services.Contract;
using SubModels.Helper;
using SubModels.Models;

namespace RDAP.Api.Services
{
    public class RdapService : IRdapService
    {
        private readonly IRestService _requestService;
        private readonly IConfiguration _configuration;

        public RdapService(IRestService requestService, IConfiguration configuration)
        {
            _requestService = requestService;
            _configuration = configuration;
        }
        /// <summary>
        /// Fetch the Rdap information from external service provider
        /// </summary>
        /// <param name="ipAddress"></param>
        /// <returns></returns>
        public async Task<HttpProcessResults<RdapModel>> GetRdapDetails(string ipAddress)
        {
            var rdapBaseUrl = _configuration["RDAPBaseURL"];

            if (string.IsNullOrEmpty(rdapBaseUrl) || string.IsNullOrEmpty(ipAddress))
                throw new ArgumentException($"Required argument not passed");

            var uri = $"{rdapBaseUrl}{ipAddress}";

            return await _requestService.GetExternalServiceAsync<RdapModel>(uri);
        }
    }
}
