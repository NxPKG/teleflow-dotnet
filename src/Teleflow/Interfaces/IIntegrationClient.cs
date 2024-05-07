using Teleflow.DTO;
using Teleflow.DTO.Integrations;
using Refit;

namespace Teleflow.Interfaces;

/// <summary>
///     see https://docs.teleflow.khulnasoft.com/api/integration-creation/
/// </summary>
public interface IIntegrationClient
{
    /// <summary>
    ///     Return all the integrations the user has created for that organization. Review v.0.17.0 changelog
    ///     for a breaking change
    ///     see https://docs.teleflow.khulnasoft.com/api/get-integrations/
    /// </summary>
    [Get("/integrations")]
    public Task<TeleflowResponse<IEnumerable<Integration>>> Get();

    /// <summary>
    ///     Get a integration by its ID
    ///     see https://docs.teleflow.khulnasoft.com/api/get-integration/
    /// </summary>
    [Get("/integrations/{id}")]
    public Task<TeleflowResponse<Integration>> Get(string id);

    /// <summary>
    ///     Return all the active integrations the user has created for that organization. Review v.0.17.0 changelog for a
    ///     breaking change
    ///     see https://docs.teleflow.khulnasoft.com/api/get-active-integrations/
    /// </summary>
    /// <returns></returns>
    [Get("/integrations/active")]
    public Task<TeleflowResponse<IEnumerable<Integration>>> GetActive();

    /// <summary>
    ///     Create an integration for the current environment the user is based on the API key provided
    ///     https://docs.teleflow.khulnasoft.com/api/create-integration/
    /// </summary>
    [Post("/integrations")]
    public Task<TeleflowResponse<Integration>> Create([Body] IntegrationCreateData createIntegrationData);

    /// <summary>
    ///     Update the name, content and variables of a integration. Also change it to be default or no.
    ///     see https://docs.teleflow.khulnasoft.com/api/update-integration/
    /// </summary>
    [Put("/integrations/{id}")]
    public Task<TeleflowResponse<Integration>> Update(
        string id,
        [Body] IntegrationEditData request);

    /// <summary>
    ///     Execute a soft delete of a integration given a certain ID.
    ///     see https://docs.teleflow.khulnasoft.com/api/delete-integration/
    /// </summary>
    [Delete("/integrations/{id}")]
    public Task Delete(string id);

    /// <summary>
    ///     Return the status of the webhook for this provider, if it is supported or if it is not based on a boolean value
    ///     https://docs.teleflow.khulnasoft.com/api/get-webhook-support-status-for-provider/
    /// </summary>
    [Get("/integrations/webhook/provider/{id}/status")]
    public Task GetWebHookSupportStatus(string id);
}