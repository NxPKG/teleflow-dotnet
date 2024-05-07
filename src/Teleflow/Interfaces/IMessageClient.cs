using Teleflow.DTO;
using Teleflow.DTO.Layouts;
using Teleflow.DTO.Messages;
using Refit;

namespace Teleflow.Interfaces;

/// <summary>
///     see https://docs.teleflow.khulnasoft.com/api/get-messages/
/// </summary>
public interface IMessageClient
{
    /// <summary>
    ///     see https://docs.teleflow.khulnasoft.com/api/get-messages/
    /// </summary>
    /// <param name="page"></param>
    /// <param name="limit"></param>
    /// <param name="channel"></param>
    /// <param name="subscriberId"></param>
    /// <param name="transactionId"></param>
    [Get("/messages")]
    public Task<TeleflowPaginatedResponse<Message>> Get(
        [Query] int page = 0,
        [Query] int limit = 100,
        [Query] string? channel = default,
        [Query] string? subscriberId = default,
        [Query] string? transactionId = default);

    /// <summary>
    ///     see https://docs.teleflow.khulnasoft.com/api/get-messages/
    /// </summary>
    [Get("/messages")]
    public Task<TeleflowPaginatedResponse<Message>> Get([Query] MessageQueryParams queryParams);

    /// <summary>
    ///     Deletes a message entity from the Teleflow platform
    ///     see https://docs.teleflow.khulnasoft.com/api/delete-message/
    /// </summary>
    [Delete("/messages/{id}")]
    public Task<TeleflowResponse<AcknowledgeData>> Delete(string id);
}