using Teleflow.DTO;
using Teleflow.DTO.Events;
using Refit;

namespace Teleflow.Interfaces;

public interface IEventClient
{
    [Post("/events/trigger")]
    Task<TeleflowResponse<EventAcknowledgeData>> Trigger([Body] EventCreateData data);

    /// <summary>
    ///     Trigger a notification to all the subscribers assigned to a topic, which helps to have
    ///     to avoid to list all the subscriber identifiers in the to field of the notification trigger
    /// 
    ///     see https://docs.teleflow.co/platform/topics#sending-a-notification-to-a-topic
    /// </summary>
    [Post("/events/trigger")]
    Task<TeleflowResponse<EventAcknowledgeData>> Create([Body] TopicCreateData data);
    
    [Post("/events/trigger/bulk")]
    Task<TeleflowResponse<IEnumerable<EventAcknowledgeData>>> CreateBulk([Body] BulkEventCreateData data);

    /// <summary>
    ///     Trigger a broadcast event to all existing subscribers, could be used to send announcements, etc. In the
    ///     future could be used to trigger events to a subset of subscribers based on defined filter
    /// </summary>
    /// <remarks>
    ///     A broadcast with fail with a 500 error if the workflow and at least one step is enabled.
    /// </remarks>
    [Post("/events/trigger/broadcast")]
    Task<TeleflowResponse<EventAcknowledgeData>> CreateBroadcast([Body] BroadcastEventCreateData dto);

    [Delete("/events/trigger/{transactionId}")]
    Task Cancel(string transactionId);
}