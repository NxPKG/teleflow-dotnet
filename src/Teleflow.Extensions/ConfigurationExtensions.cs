using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using Teleflow.Models;

namespace Teleflow.Extensions;

public static class ConfigurationExtensions
{
    /// <summary>
    ///     Load up the configuration from (based on the directory) the running process
    ///     - appsettings.json
    ///     - appsettings.[env].json
    ///     - appsettings.[dev_user].json
    ///     - environment variables
    /// </summary>
    public static IConfigurationRoot CreateConfigurationRoot(this string jsonFileName)
    {
        return new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile(jsonFileName, true)
            .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", true)
            .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("USER")}.json", true)
            .AddEnvironmentVariables()
            .Build();
    }

    /// <summary>
    ///     Get a configuration based on a string key and throw an exception if null or empty
    /// </summary>
    public static string GetConfiguration(this IConfiguration configuration, string section)
    {
        return configuration.GetSection(section)?
            .Value
            .ThrowConfigurationErrorsExceptionIfNullOrEmpty($"Error: Configuration variable '{section}' must be set");
    }


    /// <summary>
    ///     Return the <see cref="TeleflowClientConfiguration" /> from the settings file
    /// </summary>
    public static TeleflowClientConfiguration GetTeleflowClientConfiguration(this IConfiguration configuration)
    {
        var teleflowConfiguration = new TeleflowClientConfiguration();
        configuration.GetSection(Settings.TeleflowSection).Bind(teleflowConfiguration);

        var teleflowConfigurationUrl = Environment.GetEnvironmentVariable("TELEFLOW_API_URL");
        if (teleflowConfigurationUrl is not null)
        {
            teleflowConfiguration.Url = teleflowConfigurationUrl;
        }

        var teleflowConfigurationApiKey = Environment.GetEnvironmentVariable("TELEFLOW_API_KEY");
        if (teleflowConfigurationApiKey is not null)
        {
            teleflowConfiguration.ApiKey = teleflowConfigurationApiKey;
        }

        return teleflowConfiguration;
    }
}