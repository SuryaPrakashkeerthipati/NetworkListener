using Microsoft.Extensions.Configuration;
using SubModels.Helper;
using SubModels.Models;
using System;
using System.Threading.Tasks;
using GeoIP.Api.Services.Contract;

namespace GeoIP.Api.Services
{
    public class GeoIpService : IGeoIpService
    {
        private readonly IRestService _restService;
        private readonly IConfiguration _configuration;

        public GeoIpService(IRestService restService, IConfiguration configuration)
        {
            _restService = restService;
            _configuration = configuration;
        }
        /// <summary>
        /// Fetch the GeoIP information from external service provider
        /// </summary>
        /// <param name="ipAddress"></param>
        /// <returns></returns>
        public async Task<HttpProcessResults<GeoIpModel>> GetIpDetails(string ipAddress)
        {
            var ipBaseUrl = _configuration["IPBaseURL"];

            if (string.IsNullOrEmpty(ipBaseUrl) || string.IsNullOrEmpty(ipAddress))
                throw new ArgumentException($"Required argument not passed");

            var uri = $"{ipBaseUrl}{ipAddress}";

            return await _restService.GetExternalServiceAsync<GeoIpModel>(uri);
        }
    }
}
