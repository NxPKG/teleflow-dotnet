    using Newtonsoft.Json;

    namespace Teleflow.DTO.WorkflowGroups;

public class WorkflowGroupCreateData
{
    [JsonProperty("name")] public string Name { get; set; }
}