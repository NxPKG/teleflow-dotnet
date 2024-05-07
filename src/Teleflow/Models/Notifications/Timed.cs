using Newtonsoft.Json;

namespace Teleflow.Models.Notifications;

public class Timed
{
    [JsonProperty("weekDays")] public List<object> WeekDays { get; set; }

    [JsonProperty("monthDays")] public List<object> MonthDays { get; set; }
}