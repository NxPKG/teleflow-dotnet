using Newtonsoft.Json;

namespace Teleflow.DTO;

public class SucceedData
{
    [JsonProperty("succeeded")] public string[] Succeeded { get; set; }
}