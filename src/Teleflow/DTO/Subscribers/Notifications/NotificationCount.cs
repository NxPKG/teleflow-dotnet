using Newtonsoft.Json;

namespace Teleflow.DTO.Subscribers.Notifications;

public class NotificationCount
{
    [JsonProperty("count")] public int Count { get; set; }
}