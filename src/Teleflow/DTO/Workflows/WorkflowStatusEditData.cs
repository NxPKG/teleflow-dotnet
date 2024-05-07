using Newtonsoft.Json;

namespace Teleflow.DTO.Workflows;

public class WorkflowStatusEditData
{
    [JsonProperty("active")] public bool Active { get; set; }
}