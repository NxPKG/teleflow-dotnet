using System.Runtime.Serialization;

namespace Teleflow.Models.Workflows.Step.Message;

public enum ChannelCtaTypeEnum
{
    [EnumMember(Value = "redirect")] Redirect = 10,
}