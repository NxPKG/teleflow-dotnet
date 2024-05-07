using Newtonsoft.Json;

namespace Teleflow.DTO.Notifications;

public class NotificationStats
{
    [JsonProperty("weeklySent")] public int WeeklySent { get; set; }

    [JsonProperty("monthlySent")] public int MonthlySent { get; set; }
}