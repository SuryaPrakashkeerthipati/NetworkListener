using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using Network = System.Net.NetworkInformation;
using System.Net;
using SubModels.Utilities;
using SubModels.Extensions;
using SubModels.Models;
using System.Threading.Tasks;

namespace Ping.Api.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class PingController : BaseController
    {
        private readonly ILogger<PingController> _logger;
        public PingController(ILogger<PingController> logger)
        {
            _logger = logger;
        }
        /// <summary>
        /// Get the ping  object which includes IPStatus,IPAddress and ExecutionTime
        /// </summary>
        /// <param name="ipAddress"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetPingInfo(string ipAddress)
        {
            HttpProcessResults<PingModel> result = null;
            try
            {
                if (string.IsNullOrEmpty(ipAddress))
                    return BadRequest();

                Network.Ping myPing = new Network.Ping();
                var response = await myPing.SendPingAsync(ipAddress, 1000);
                var pingModel = response.ToPingModel();

                result = new HttpProcessResults<PingModel>
                {
                    IsSuccess = true,
                    Result = pingModel,
                    HttpStatusCode = HttpStatusCode.OK
                };

                return CustomActionResult(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Unexpected error in {nameof(GetPingInfo)}", ex);
                throw new HttpResponseException { StatusCode = (int)HttpStatusCode.InternalServerError, Content = $"An Error occurred while requesting {nameof(GetPingInfo)}" };
            }
        }

    }
}
