using Newtonsoft.Json;

namespace SubModels.Models
{
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class NetworkListenerModel
    {
        public HttpProcessResults<GeoIpModel> GeoIp { get; set; }
        public HttpProcessResults<RdapModel> Rdap { get; set; }
        public HttpProcessResults<RdnsModel> Rdns { get; set; }
        public HttpProcessResults<PingModel> Ping { get; set; }
    }
}
