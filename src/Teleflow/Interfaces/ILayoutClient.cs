using Teleflow.DTO;
using Teleflow.DTO.Layouts;
using Refit;

namespace Teleflow.Interfaces;

/// <summary>
///     see https://docs.teleflow.khulnasoft.com/api/layout-creation/
/// </summary>
public interface ILayoutClient
{
    /// <summary>
    ///     Returns a list of layouts that can be paginated using the `page` query parameter and filtered by the
    ///     environment where it is executed from the organization the user belongs to.
    ///     see https://docs.teleflow.khulnasoft.com/api/filter-layouts/
    /// </summary>
    /// <param name="page">Number of page for the pagination</param>
    /// <param name="pageSize">Size of page for the pagination</param>
    /// <param name="sortBy">Sort field. Currently only supported `createdAt`</param>
    /// <param name="orderBy">Direction of the sorting query param. Either ascending (1) or descending (-1)</param>
    /// <returns></returns>
    [Get("/layouts")]
    public Task<TeleflowPaginatedResponse<Layout>> Get(
        [Query] int page = 0,
        [Query] int pageSize = 10,
        [Query] string? sortBy = default,
        [Query] int orderBy = default);

    [Get("/layouts")]
    public Task<TeleflowPaginatedResponse<Layout>> Get([Query] PaginationQueryParams queryParams);

    /// <summary>
    ///     Get a layout by its ID
    ///     see https://docs.teleflow.khulnasoft.com/api/get-layout/
    /// </summary>
    [Get("/layouts/{id}")]
    public Task<TeleflowResponse<Layout>> Get(string id);

    /// <summary>
    ///     Create a layout
    ///     https://docs.teleflow.khulnasoft.com/api/layout-creation/
    /// </summary>
    [Post("/layouts")]
    public Task<TeleflowResponse<Layout>> Create([Body] LayoutCreateData createLayoutData);

    /// <summary>
    ///     Update the name, content and variables of a layout. Also change it to be default or no.
    ///     see https://docs.teleflow.khulnasoft.com/api/update-layout/
    /// </summary>
    [Patch("/layouts/{id}")]
    public Task<TeleflowResponse<Layout>> Update(
        string id,
        [Body] LayoutEditData request);

    /// <summary>
    ///     Execute a soft delete of a layout given a certain ID.
    ///     see https://docs.teleflow.khulnasoft.com/api/delete-layout/
    /// </summary>
    [Delete("/layouts/{id}")]
    public Task Delete(string id);

    /// <summary>
    ///     https://docs.teleflow.khulnasoft.com/api/set-default-layout/
    /// </summary>
    [Post("/layouts/{id}/default")]
    public Task SetAsDefault(string id);
}