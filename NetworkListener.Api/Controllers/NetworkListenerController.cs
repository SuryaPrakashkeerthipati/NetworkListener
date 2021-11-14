using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SubModels.Utilities;
using System;
using System.Net;
using NetworkListener.Api.Services.Contracts;

namespace NetworkListener.Api.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    [ApiVersion("2.0")]
    public class NetworkListenerController : ControllerBase
    {
        private readonly ILogger<NetworkListenerController> _logger;
        private readonly INetworkListenerService _networkListenerService;
        public NetworkListenerController(ILogger<NetworkListenerController> logger, INetworkListenerService networkListenerService)
        {
            _logger = logger;
            _networkListenerService = networkListenerService;
        }
        /// <summary>
        /// Fetch the GeoIp,RDAP,RDNS and Ping information based on ip address and services(Pass service typs as comma separate string i.e GEOIP,RDAP,RDNS,PING ).
        /// </summary>
        /// <param name="ipAddress">8.8.8.8</param>
        /// <param name="services">GEOIP,RDAP,RDNS,PING"</param>
        /// <returns>NetworkListener</returns>
        [HttpGet]
        public IActionResult ExecuteServices(string ipAddress, string services)
        {
            try
            {
                if (string.IsNullOrEmpty(ipAddress) || !_networkListenerService.CheckIPv4andIPv6(ipAddress))
                    return BadRequest($"Given {ipAddress} IpAddress is not valid");

                var response = _networkListenerService.InvokeParallelServices(ipAddress, services);
                return Ok(response);
            }
            catch (AggregateException e)
            {
                _logger.LogError($"Unexpected error in {nameof(ExecuteServices)}", e.InnerException);
                throw new CustomAggregateException { StatusCode = (int)HttpStatusCode.InternalServerError, Content = $"An Error occurred while requesting {nameof(ExecuteServices)}", Exception = e };
            }
            catch (Exception ex)
            {
                var apiException = (HttpResponseException)ex;
                _logger.LogError($"Error when executing {nameof(ExecuteServices)} method, {apiException.Content}");
                throw new HttpResponseException { StatusCode = apiException.StatusCode, Content = $"Error when executing {nameof(ExecuteServices)} method, {apiException.Content}" };
            }
        }
    }
}
