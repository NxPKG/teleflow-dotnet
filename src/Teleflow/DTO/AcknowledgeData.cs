using Newtonsoft.Json;

namespace Teleflow.DTO;

public class AcknowledgeData
{
    [JsonProperty("acknowledged")] public bool Acknowledged { get; set; }
    [JsonProperty("status")] public string Status { get; set; }
}