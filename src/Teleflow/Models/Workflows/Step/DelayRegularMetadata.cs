using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace Teleflow.Models.Workflows.Step;

public class DelayRegularMetadata : AmountAndUnit, IWorkflowMetaData
{
    [Required] [JsonProperty("type")] public DelayTypeEnum Type { get; private set; } = DelayTypeEnum.Regular;
}