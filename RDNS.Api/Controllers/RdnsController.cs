using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SubModels.Utilities;
using System;
using System.Threading.Tasks;
using RDNS.Api.Services.Contract;

namespace RDNS.Api.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class RdnsController : BaseController
    {
        private readonly IRdnsService _rdnsService;
        private readonly ILogger<RdnsController> _logger;
        public RdnsController(IRdnsService rdnsService, ILogger<RdnsController> logger)
        {
            _rdnsService = rdnsService;
            _logger = logger;
        }
        /// <summary>
        /// Get the RDNS information based on ip address.
        /// </summary>
        /// <param name="ipAddress"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetRdnsInformation(string ipAddress)
        {
            try
            {
                if (string.IsNullOrEmpty(ipAddress))
                    return BadRequest();

                var response = await _rdnsService.GetRdnsInformation(ipAddress);

                return CustomActionResult(response);
            }
            catch (Exception ex)
            {
                var apiException = (HttpResponseException)ex;
                _logger.LogError($"Error when executing {nameof(GetRdnsInformation)} method, {apiException.Content}");
                throw new HttpResponseException { StatusCode = apiException.StatusCode, Content = $"Error when executing {nameof(GetRdnsInformation)} method, {apiException.Content}" };
            }
        }
    }
}
