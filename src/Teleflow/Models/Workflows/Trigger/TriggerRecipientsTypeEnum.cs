using System.Runtime.Serialization;

namespace Teleflow.Models.Workflows.Trigger;

/// <summary>
///     see https://github.com/nxpkg/teleflow/blob/next/libs/shared/src/types/events/index.ts#L40
/// </summary>
public enum TriggerRecipientsTypeEnum
{
    [EnumMember(Value = "Subscriber")] Subscriber = 10,
    [EnumMember(Value = "Topic")] Topic = 20,
}