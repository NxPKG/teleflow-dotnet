using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Teleflow.Models.Workflows.Trigger;

namespace Teleflow.Models.Triggers;

public class TopicTrigger
{
    public TopicTrigger(string topicKey)
    {
        TopicKey = topicKey;
    }

    public TopicTrigger()
    {
    }

    [JsonConverter(typeof(StringEnumConverter))]
    [JsonProperty("type")]
    public TriggerRecipientsTypeEnum Type { get; } = TriggerRecipientsTypeEnum.Topic;

    [JsonProperty("topicKey")] public string TopicKey { get; set; }
}