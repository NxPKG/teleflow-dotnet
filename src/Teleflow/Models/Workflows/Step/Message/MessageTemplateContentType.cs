using System.Runtime.Serialization;

namespace Teleflow.Models.Workflows.Step.Message;

public enum MessageTemplateContentType
{
    [EnumMember(Value = "editor")] Editor = 10,
    [EnumMember(Value = "customHtml")] CustomHtml = 20,
}