using Newtonsoft.Json;

namespace Teleflow.Models.Notifications;

public class Metadata
{
    [JsonProperty("timed")] public Timed Timed { get; set; }
}