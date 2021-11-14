using Newtonsoft.Json;

namespace SubModels.Models
{
    public class PingModel
    {
        [JsonProperty("status")]
        public string Status { get; set; }
        [JsonProperty("executionTime")]
        public long ExecutionTime { get; set; }
    }
}
