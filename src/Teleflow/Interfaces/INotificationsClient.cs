using Teleflow.DTO;
using Teleflow.DTO.Notifications;
using Refit;

namespace Teleflow.Interfaces;

public interface INotificationsClient
{
    [Get("/notifications")]
    public Task<TeleflowPaginatedResponse<Notification>> Get([Query] NotificationQueryParams? queryParam = null);

    [Get("/notifications/stats")]
    public Task<TeleflowResponse<NotificationStats>> GetStats();

    [Get("/notifications/graph/stats")]
    public Task<TeleflowResponse<NotificationGraphStats[]>> GetGraphStats([Query] int days = 7);

    [Get("/notifications/{id}")]
    public Task Get(string id);
}