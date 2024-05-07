using Newtonsoft.Json;

namespace Teleflow.DTO.Subscribers;

public class SubscriberOnlineEditData
{
    [JsonProperty("isOnline")] public bool IsOnline { get; set; }
}