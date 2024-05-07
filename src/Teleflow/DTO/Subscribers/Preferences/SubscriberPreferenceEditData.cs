using Newtonsoft.Json;
using Teleflow.Models.Subscribers.Preferences;

namespace Teleflow.DTO.Subscribers.Preferences;

public class SubscriberPreferenceEditData
{
    [JsonProperty("channel")] public Channel Channel { get; set; }
}