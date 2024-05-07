using System.Runtime.Serialization;

namespace Teleflow.Models.Workflows.Step.Message;

public enum MessageActionStatusEnum
{
    [EnumMember(Value = "pending")] Pending = 10,
    [EnumMember(Value = "done")] Done = 20,
}