using Microsoft.Extensions.DependencyInjection;
using Teleflow.Sync.Models;
using Teleflow.Sync.Services;

namespace Teleflow.Sync;

public static class IocRegistrationExtensions
{
    public static IServiceCollection RegisterTeleflowSync(this IServiceCollection services)
    {
        services.AddTransient<ITeleflowSync<TemplateLayout>, LayoutSync>();
        services.AddTransient<ITeleflowSync<TemplateWorkflowGroup>, WorkflowGroupSync>();
        services.AddTransient<ITeleflowSync<TemplateIntegration>, IntegrationSync>();
        services.AddTransient<ITeleflowSync<TemplateWorkflow>, WorkflowSync>();
        return services;
    }
}