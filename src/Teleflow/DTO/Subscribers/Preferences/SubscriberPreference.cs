using Newtonsoft.Json;
using Teleflow.Models.Subscribers.Preferences;

namespace Teleflow.DTO.Subscribers.Preferences;

public class SubscriberPreference
{
    [JsonProperty("template")] public Template Template { get; set; }
    [JsonProperty("preference")] public Preference Preference { get; set; }
}