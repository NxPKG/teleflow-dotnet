using Newtonsoft.Json;

namespace Teleflow.Models.Workflows.Step;

public class DigestBaseMetadata : AmountAndUnit
{
    [JsonProperty("digestKey")] public string DigestKey { get; set; }
}