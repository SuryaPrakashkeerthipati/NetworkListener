using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SubModels.Models;
using System;
using System.Threading.Tasks;
using NetworkListener.Api.Services.Contracts;
using System.Net;
using System.Net.Sockets;
using System.Linq;
using SubModels.Helper;

namespace NetworkListener.Api.Services
{
    public class NetworkListenerService : INetworkListenerService
    {
        private readonly ILogger<NetworkListenerService> _logger;
        private readonly IConfiguration _configuration;
        private readonly IRestService _restService;

        public NetworkListenerService(ILogger<NetworkListenerService> logger, IConfiguration configuration,IRestService restService)
        {
            _logger = logger;
            _configuration = configuration;
            _restService = restService;
        }

        /// <summary>
        ///Ability to Process multiple services using parllel.foreach approach
        /// </summary>
        /// <param name="ipAddress"></param>
        /// <param name="services"></param>
        /// <returns></returns>
        public NetworkListenerModel InvokeParallelServices(string ipAddress, string services)
        {
            _logger.LogInformation($"Begin {nameof(InvokeParallelServices)} method");

            var networkListenerModel = new NetworkListenerModel();
            
            if (string.IsNullOrEmpty(services))
                services = _configuration.GetSection("DefaultServices").Get<string>();

            var networkServices = services.Split(',');

            Parallel.ForEach(networkServices, service =>
            {
                lock (service.ToLower())
                {
                    if (Enum.TryParse(service.ToLower(), out ServiceTypes result))
                    {
                        switch (result)
                        {
                            case ServiceTypes.geoip:
                                var ipServiceBaseURL = $"{_configuration["IPServiceBaseURL"]}?ipAddress={ipAddress}";
                                networkListenerModel.GeoIp = _restService.GetInternalServiceAsync<GeoIpModel>(ipServiceBaseURL).Result;
                                _logger.LogInformation($"Invoked {nameof(ServiceTypes.geoip)} service for IpAddress :{ipAddress}");
                                break;

                            case ServiceTypes.rdap:
                                var rdapServiceBaseUrl = $"{_configuration["RDAPServiceBaseURL"]}?ipAddress={ipAddress}";
                                networkListenerModel.Rdap = _restService.GetInternalServiceAsync<RdapModel>(rdapServiceBaseUrl).Result;
                                _logger.LogInformation($"Invoked {nameof(ServiceTypes.rdap)} service for IpAddress :{ipAddress}");
                                break;

                            case ServiceTypes.rdns:
                                var rdnsServiceBaseUrl = $"{_configuration["RDNSServiceBaseURL"]}?ipAddress={ipAddress}";
                                networkListenerModel.Rdns = _restService.GetInternalServiceAsync<RdnsModel>(rdnsServiceBaseUrl).Result;
                                _logger.LogInformation($"Invoked {nameof(ServiceTypes.rdns)} service for IpAddress : {ipAddress}");
                                break;

                            case ServiceTypes.ping:
                                var pingServiceBaseUrl = $"{_configuration["pingServiceBaseUrl"]}?ipAddress={ipAddress}";
                                networkListenerModel.Ping = _restService.GetInternalServiceAsync<PingModel>(pingServiceBaseUrl).Result;
                                _logger.LogInformation($"Invoked {nameof(ServiceTypes.ping)} service for IpAddress : {ipAddress}");
                                break;

                            default:

                                _logger.LogInformation($"Service is not listed : {ipAddress}");
                                break;
                        }
                    }
                }
            });

            return networkListenerModel;
        }
        /// <summary>
        /// Validates given IpAddress is compatible to IPv4/IPv6 protocol standards
        /// </summary>
        /// <param name="ipAddress"></param>
        /// <returns></returns>
        public bool CheckIPv4andIPv6(string ipAddress)
        {
            bool isValid = false;

            IPAddress address;
            if (!string.IsNullOrEmpty(ipAddress))
            {
                if (ipAddress.Count(c => c == '.') == 3)
                {
                    isValid = IPAddress.TryParse(ipAddress, out address);
                }
                else if (ipAddress.Contains(':'))
                {
                    if (IPAddress.TryParse(ipAddress, out address))
                    {
                        isValid = address.AddressFamily == AddressFamily.InterNetworkV6;
                    }
                }

            }
            return isValid;
        }

    }
}

