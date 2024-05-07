using Teleflow.DTO;
using Teleflow.DTO.Workflows;
using Refit;

namespace Teleflow.Interfaces;

public interface IWorkflowClient
{
    /// <summary>
    ///     see https://docs.teleflow.co/api/get-workflows/
    /// </summary>
    [Get("/workflows")]
    public Task<TeleflowPaginatedResponse<Workflow>> Get([Query] int page = 0, [Query] int limit = 10);

    [Get("/workflows")]
    public Task<TeleflowPaginatedResponse<Workflow>> Get([Query] PaginationQueryParams queryParams);

    /// <summary>
    ///     see https://docs.teleflow.co/api/get-workflow/
    /// </summary>
    [Get("/workflows/{id}")]
    public Task<TeleflowResponse<Workflow>> Get(string id);

    /// <summary>
    ///     https://docs.teleflow.co/api/create-workflow/
    /// </summary>
    [Post("/workflows")]
    public Task<TeleflowResponse<Workflow>> Create([Body] WorkflowCreateData createWorkflowData);

    /// <summary>
    ///     see https://docs.teleflow.co/api/update-workflow/
    /// </summary>
    [Put("/workflows/{id}")]
    public Task<TeleflowResponse<Workflow>> Update(
        string id,
        [Body] WorkflowEditData request);

    /// <summary>
    ///     see https://docs.teleflow.co/api/delete-workflow/
    /// </summary>
    [Delete("/workflows/{id}")]
    public Task<TeleflowResponse<bool>> Delete(string id);

    /// <summary>
    ///     see https://docs.teleflow.co/api/update-workflow-status/
    /// </summary>
    [Put("/workflows/{id}/status")]
    public Task<TeleflowResponse<Workflow>> UpdateStatus(
        string id,
        [Body] WorkflowStatusEditData request);
}