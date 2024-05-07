using Newtonsoft.Json;
using Teleflow.Models.Workflows.Trigger;

namespace Teleflow.Models.Notifications;

public class Trigger
{
    [JsonProperty("type")] public TriggerTypeEnum Type { get; set; }

    [JsonProperty("identifier")] public string Identifier { get; set; }

    [JsonProperty("variables")] public List<object> Variables { get; set; }

    [JsonProperty("subscriberVariables")] public List<object> SubscriberVariables { get; set; }
}