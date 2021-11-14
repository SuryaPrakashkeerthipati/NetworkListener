using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SubModels.Models;
using System.Net;

namespace RDNS.Api.Controllers
{
    
    public class BaseController : ControllerBase
    {
        protected IActionResult CustomActionResult<T>(HttpProcessResults<T> httpProcessResults) where T : class
        {
            switch (httpProcessResults)
            {
                case null:
                    return NoContent();
                default:
                    {
                        switch (httpProcessResults.HttpStatusCode)
                        {
                            case HttpStatusCode.OK:
                            case HttpStatusCode.Created:
                                return Ok(httpProcessResults);
                            case HttpStatusCode.NotFound:
                                return NotFound(httpProcessResults);
                            case HttpStatusCode.NoContent:
                                return NoContent();
                            case HttpStatusCode.BadRequest:
                                return BadRequest();
                            case HttpStatusCode.Conflict:
                                return Conflict(httpProcessResults);
                            case HttpStatusCode.InternalServerError:
                                return StatusCode(StatusCodes.Status500InternalServerError, httpProcessResults);
                            default:
                                return NoContent();
                        }
                    }
            }
        }
    }
}
