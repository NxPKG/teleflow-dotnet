using Newtonsoft.Json;

namespace Teleflow.DTO.Events;

public class EventAcknowledgeData : AcknowledgeData
{
    [JsonProperty("transactionId")] public string TransactionId { get; set; }
}