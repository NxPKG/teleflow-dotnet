using Teleflow.Interfaces;

namespace Teleflow.Models;

public class TeleflowClientConfiguration : ITeleflowClientConfiguration
{
    public string Url { get; set; } = "https://api.teleflow.co/v1";

    public string ApiKey { get; set; }
}