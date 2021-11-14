using SubModels.Models;
using System.Threading.Tasks;

namespace SubModels.Helper
{
    public interface IRestService
    {
        Task<HttpProcessResults<T>> GetExternalServiceAsync<T>(string uri);
        Task<HttpProcessResults<T>> GetInternalServiceAsync<T>(string uri);
        Task<string> GetAsync(string uri);
    }
}
