using Newtonsoft.Json;

namespace Teleflow.Models.Workflows.Step.Message;

public class EmailBlockStyle
{
    [JsonProperty("textAlign")] public TextAlignEnum Type { get; set; }
}