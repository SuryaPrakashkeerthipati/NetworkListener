using Newtonsoft.Json;

namespace SubModels.Models
{
    public class NetworkListenerModel
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public HttpProcessResults<GeoIpModel> GeoIp { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public HttpProcessResults<RdapModel> Rdap { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public HttpProcessResults<RdnsModel> Rdns { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public HttpProcessResults<PingModel> Ping { get; set; }
    }
}
