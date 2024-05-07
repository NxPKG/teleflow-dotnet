namespace Teleflow.Interfaces;

public interface IApiConfiguration
{
    public ITeleflowClientConfiguration TeleflowClientConfiguration { get; set; }
}