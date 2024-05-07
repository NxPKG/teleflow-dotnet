using Newtonsoft.Json;

namespace Teleflow.Models.Workflows.Step.Message;

public class MessageCta
{
    [JsonProperty("type")] public ChannelCtaTypeEnum Type { get; set; }
    [JsonProperty("data")] public MessageCtaData Data { get; set; }
    [JsonProperty("action")] public MessageAction Action { get; set; }
}