using System.Threading.Tasks;
using SubModels.Models;

namespace RDAP.Api.Services.Contract
{
    public interface IRdapService
    {
        Task<HttpProcessResults<RdapModel>> GetRdapDetails(string ipAddress);
    }
}
