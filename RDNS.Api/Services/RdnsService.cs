using Microsoft.Extensions.Configuration;
using SubModels.Helper;
using SubModels.Models;
using System;
using System.Threading.Tasks;
using RDNS.Api.Services.Contract;

namespace RDNS.Api.Services
{
    public class RdnsService : IRdnsService
    {
        private readonly IRestService _restService;
        private readonly IConfiguration _configuration;

        public RdnsService(IRestService restService, IConfiguration configuration)
        {
            _restService = restService;
            _configuration = configuration;
        }
        /// <summary>
        /// Fetch the Rdns information from external service provider
        /// </summary>
        /// <param name="ipAddress"></param>
        /// <returns></returns>
        public async Task<HttpProcessResults<RdnsModel>> GetRdnsInformation(string ipAddress)
        {
            HttpProcessResults<RdnsModel> rdnsModel = null;
            var rdnsBaseUrl = _configuration["RDNSBaseURL"];

            if (string.IsNullOrEmpty(rdnsBaseUrl) || string.IsNullOrEmpty(ipAddress))
                throw new ArgumentException($"Required argument not passed");

            var uri = $"{rdnsBaseUrl}?q={ipAddress}";

            var result = await _restService.GetAsync(uri);
            if (result != null)
            {
                rdnsModel = new HttpProcessResults<RdnsModel>
                {
                    Result = new RdnsModel { DnsName = result, IpAddress = ipAddress },
                    HttpStatusCode = System.Net.HttpStatusCode.OK,
                    IsSuccess = true
                };
            };

            return rdnsModel;
        }
    }

}


