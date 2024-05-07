using Teleflow.DTO;
using Teleflow.DTO.Messages;
using Teleflow.Models.Notifications;
using Refit;

namespace Teleflow.Interfaces;

public interface IExecutionDetailsClient
{
    /// <summary>
    ///     see https://docs.teleflow.co/api/get-execution-details/
    /// </summary>
    /// <param name="notificationId"></param>
    /// <param name="subscriberId"></param>
    /// <returns></returns>
    [Get("/execution-details")]
    public Task<TeleflowResponse<ExecutionDetail[]>> Get([Query] string notificationId, [Query] string subscriberId);
}