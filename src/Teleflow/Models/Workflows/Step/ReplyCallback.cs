using Newtonsoft.Json;

namespace Teleflow.Models.Workflows.Step;

public class ReplyCallback
{
    [JsonProperty("active")] public bool Active { get; set; }

    [JsonProperty("url")] public Uri Url { get; set; }
}