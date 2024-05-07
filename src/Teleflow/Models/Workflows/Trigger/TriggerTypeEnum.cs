using System.Runtime.Serialization;

namespace Teleflow.Models.Workflows.Trigger;

public enum TriggerTypeEnum
{
    [EnumMember(Value = "event")] Event = 10,
}