using System.Threading.Tasks;
using SubModels.Models;

namespace RDNS.Api.Services.Contract
{
    public interface IRdnsService
    {
        Task<HttpProcessResults<RdnsModel>> GetRdnsInformation(string ipAddress);
    }
}
