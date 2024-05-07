using Teleflow.Interfaces;

namespace Teleflow.Models;

public class ApiClientConfiguration : IApiConfiguration
{
    public ITeleflowClientConfiguration TeleflowClientConfiguration { get; set; }
}