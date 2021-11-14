using SubModels.Models;

namespace NetworkListener.Api.Services.Contracts
{
    public interface INetworkListenerService
    {
        NetworkListenerModel InvokeParallelServices(string ipAddress, string services);
        bool CheckIPv4andIPv6(string ipAddress);

    }
}
