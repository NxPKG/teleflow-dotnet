using Newtonsoft.Json;

namespace Teleflow.Models.Workflows.Step.Message;

public class MessageCtaData
{
    [JsonProperty("url")] public Uri Url { get; set; }
}