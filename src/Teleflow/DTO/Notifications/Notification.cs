using Newtonsoft.Json;
using Teleflow.Models.Notifications;
using Teleflow.Models.Subscribers.Preferences;
using Template = Teleflow.Models.Notifications.Template;

namespace Teleflow.DTO.Notifications;

public class Notification
{
    [JsonProperty("_id")] public string Id { get; set; }
    [JsonProperty("_templateId")] public string TemplateId { get; set; }
    [JsonProperty("_environmentId")] public string EnvironmentId { get; set; }
    [JsonProperty("_organizationId")] public string OrganizationId { get; set; }
    [JsonProperty("transactionId")] public string TransactionId { get; set; }
    [JsonProperty("channels")] public ChannelTypeEnum[] Channels { get; set; }
    [JsonProperty("template")] public Template Template { get; set; }

    /// <summary>
    ///     Sometimes this can be null and not sure why
    /// </summary>
    [JsonProperty("subscriber")]
    public Subscriber? Subscriber { get; set; }

    [JsonProperty("jobs")] public Job[] Jobs { get; set; }
}