namespace Teleflow.Interfaces;

public interface ITeleflowClientConfiguration
{
    /// <summary>
    ///     Full URL to where the Teleflow API is housed. Defaults to
    ///     https://api.teleflow.co/v1
    /// </summary>
    public string Url { get; set; }

    /// <summary>
    ///     Api Key used for authentication. Found on the Settings
    ///     page > API Keys > API Key.
    /// </summary>
    public string? ApiKey { get; set; }
}