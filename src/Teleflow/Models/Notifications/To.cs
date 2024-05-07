using Newtonsoft.Json;

namespace Teleflow.Models.Notifications;

public class To
{
    [JsonProperty("subscriberId")] public string SubscriberId { get; set; }
}