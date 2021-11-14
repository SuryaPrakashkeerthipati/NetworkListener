using System.Threading.Tasks;
using SubModels.Models;

namespace GeoIP.Api.Services.Contract
{
    public interface IGeoIpService
    {
        Task<HttpProcessResults<GeoIpModel>> GetIpDetails(string ipAddress);
    }
}
