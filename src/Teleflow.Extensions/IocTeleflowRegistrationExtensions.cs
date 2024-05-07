using System;
using System.Net.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Teleflow.Interfaces;
using Refit;

namespace Teleflow.Extensions;

public static class IocTeleflowRegistrationExtensions
{
    /// <summary>
    ///     Register Api client for Refit
    /// </summary>
    public static IServiceCollection RegisterTeleflowClients(
        this IServiceCollection services,
        IConfiguration configuration,
        RefitSettings refitSettings = null)
    {
        var teleflowConfiguration = configuration.GetTeleflowClientConfiguration();

        Action<HttpClient> configureClient = client =>
        {
            client.BaseAddress = new Uri(teleflowConfiguration.Url);

            // allow for multiple registrations—authorization cannot have multiple entries
            if (client.DefaultRequestHeaders.Contains("Authorization"))
            {
                client.DefaultRequestHeaders.Remove("Authorization");
            }

            client.DefaultRequestHeaders.Add("Authorization", $"ApiKey {teleflowConfiguration.ApiKey}");
        };
        var settings = RefitSettings(refitSettings);

        services.AddRefitClient<ISubscriberClient>(settings).ConfigureHttpClient(configureClient);
        services.AddRefitClient<IEventClient>(settings).ConfigureHttpClient(configureClient);
        services.AddRefitClient<ITopicClient>(settings).ConfigureHttpClient(configureClient);
        services.AddRefitClient<IWorkflowGroupClient>(settings).ConfigureHttpClient(configureClient);
        services.AddRefitClient<IWorkflowClient>(settings).ConfigureHttpClient(configureClient);
        services.AddRefitClient<ILayoutClient>(settings).ConfigureHttpClient(configureClient);
        services.AddRefitClient<IIntegrationClient>(settings).ConfigureHttpClient(configureClient);
        services.AddRefitClient<INotificationsClient>(settings).ConfigureHttpClient(configureClient);
        services.AddRefitClient<IMessageClient>(settings).ConfigureHttpClient(configureClient);
        services.AddRefitClient<IExecutionDetailsClient>(settings).ConfigureHttpClient(configureClient);

        return services
            .AddTransient<ITeleflowClientConfiguration>(_ => teleflowConfiguration);
    }


    public static IServiceCollection RegisterTeleflowClient(
        this IServiceCollection services,
        IConfiguration configuration,
        RefitSettings refitSettings = null)
    {
        return services
            .AddTransient<ITeleflowClient>(_ => new TeleflowClient(
                configuration.GetTeleflowClientConfiguration(),
                refitSettings: RefitSettings(refitSettings)));
    }
    
    private static RefitSettings RefitSettings(RefitSettings refitSettings)
    {
        return refitSettings ?? new RefitSettings
        {
            ContentSerializer = new NewtonsoftJsonContentSerializer(TeleflowClient.DefaultSerializerSettings),
        };
    }
}