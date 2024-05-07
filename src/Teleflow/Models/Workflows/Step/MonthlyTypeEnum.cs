using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Teleflow.Models.Workflows.Step;

[JsonConverter(typeof(StringEnumConverter))]
public enum MonthlyTypeEnum
{
    [EnumMember(Value = "each")] Each = 10,
    [EnumMember(Value = "on")] On = 20,
}