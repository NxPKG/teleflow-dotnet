using Newtonsoft.Json;

namespace Teleflow.Models.Triggers;

public class Actor
{
    [JsonProperty("subscriberId")] public string SubscriberId { get; set; }
}