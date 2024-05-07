using Newtonsoft.Json;

namespace Teleflow.Models.Subscribers.Preferences;

public class Template
{
    [JsonProperty("_id")] public string Id { get; set; }

    [JsonProperty("name")] public string Name { get; set; }

    [JsonProperty("critical")] public bool Critical { get; set; }
}