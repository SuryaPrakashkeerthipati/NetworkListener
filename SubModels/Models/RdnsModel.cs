using System.Text.Json.Serialization;

namespace SubModels.Models
{
    public class RdnsModel
    {
        [JsonPropertyName("ipAddress")]
        public string IpAddress { get; set; }
        [JsonPropertyName("dnsName")]
        public string DnsName { get; set; }
    }
}
