using Teleflow.DTO;
using Teleflow.DTO.WorkflowGroups;
using Refit;

namespace Teleflow.Interfaces;

public interface IWorkflowGroupClient
{
    /// <summary>
    ///     Create a Workflow Group
    /// </summary>
    /// <param name="requestBody"></param>
    [Post("/notification-groups")]
    public Task<TeleflowResponse<WorkflowGroup>> Create([Body] WorkflowGroupCreateData requestBody);

    /// <summary>
    ///     Get All Workflow Groups
    /// </summary>
    [Get("/notification-groups")]
    public Task<TeleflowResponse<IEnumerable<WorkflowGroup>>> Get();

    [Delete("/notification-groups/{id}")]
    public Task Delete(string id);
}