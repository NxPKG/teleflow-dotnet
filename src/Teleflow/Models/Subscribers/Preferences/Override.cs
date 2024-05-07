using Newtonsoft.Json;

namespace Teleflow.Models.Subscribers.Preferences;

public class Override
{
    [JsonProperty("channel")] public string Channel { get; set; }

    [JsonProperty("source")] public string Source { get; set; }
}