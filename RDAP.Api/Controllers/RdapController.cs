using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using RDAP.Api.Services.Contract;
using SubModels.Utilities;

namespace RDAP.Api.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class RdapController : BaseController
    {
        private readonly IRdapService _rdapService;
        private readonly ILogger<RdapController> _logger;
        public RdapController(IRdapService rdapService, ILogger<RdapController> logger)
        {
            _rdapService = rdapService;
            _logger = logger;
        }
        /// <summary>
        /// Get the RDAP information based on ip address.
        /// </summary>
        /// <param name="ipAddress"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetRdapDetails(string ipAddress)
        {
            try
            {
                if (string.IsNullOrEmpty(ipAddress))
                    return BadRequest();

                var response = await _rdapService.GetRdapDetails(ipAddress);

                return CustomActionResult(response);
            }
            catch (Exception ex)
            {
                var apiException = (HttpResponseException)ex;
                _logger.LogError($"Error when executing {nameof(GetRdapDetails)} method, {apiException.Content}");
                throw new HttpResponseException { StatusCode = apiException.StatusCode, Content = $"Error when executing {nameof(GetRdapDetails)} method, {apiException.Content}" };
            }
        }
    }
}
