using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SubModels.Utilities;
using System;
using System.Threading.Tasks;
using GeoIP.Api.Services.Contract;

namespace GeoIP.Api.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class GeoIpController : BaseController
    {
        private readonly IGeoIpService _geoIpService;
        private readonly ILogger<GeoIpController> _logger;
        public GeoIpController(IGeoIpService geoIpService, ILogger<GeoIpController> logger)
        {
            _geoIpService = geoIpService;
            _logger = logger;
        }
        /// <summary>
        /// Get the GeoIp information based on ip address.
        /// </summary>
        /// <param name="ipAddress"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetIpDetails(string ipAddress)
        {
            try
            {
                if (string.IsNullOrEmpty(ipAddress))
                    return BadRequest();

                var response = await _geoIpService.GetIpDetails(ipAddress);

                return CustomActionResult(response);
            }
            catch (Exception ex)
            {
                var apiException = (HttpResponseException)ex;
                _logger.LogError($"Error when executing {nameof(GetIpDetails)} method, {apiException.Content}");
                throw new HttpResponseException { StatusCode = apiException.StatusCode, Content = $"Error when executing {nameof(GetIpDetails)} method, {apiException.Content}" };
            }
        }
    }
}
