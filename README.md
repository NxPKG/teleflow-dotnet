<div align="center"> 
    <a href="https://teleflow.co" target="_blank"> 
        <picture>
            <source media="(prefers-color-scheme: dark)" srcset="https://user-images.githubusercontent.com/2233092/213641039-220ac15f-f367-4d13-9eaf-56e79433b8c1.png"> <img src="https://user-images.githubusercontent.com/2233092/213641043-3bbb3f21-3c53-4e67-afe5-755aeb222159.png" width="280" alt="Logo"/> 
        </picture> 
    </a> 
</div>

# teleflow-dotnet

[![NuGet](https://img.shields.io/nuget/v/Teleflow.svg)](https://www.nuget.org/packages/Teleflow/)
[![NuGet](https://img.shields.io/nuget/dt/Teleflow.svg)](https://www.nuget.org/packages/Teleflow/)
[![Deploy to Nuget](https://github.com/nxpkg/teleflow-dotnet/actions/workflows/dotnet-deploy.yaml/badge.svg)](https://github.com/nxpkg/teleflow-dotnet/actions/workflows/dotnet-deploy.yaml)

.NET SDK for Teleflow - The open-source notification infrastructure for engineers. 🚀

teleflow-dotnet targets .NET Standard 2.0 and is compatible with .NET Core 2.0+ and .NET Framework 4.6.1+.

## Features

- Bindings against most [API endpoints](https://docs.teleflow.co/api/overview/)
  - Events, subscribers, notifications, integrations, layouts, topics, workflows, workflow groups, messages, execution details
  - Not Implemented: [environments](https://docs.teleflow.co/api/get-current-environment/), [inbound parse](https://docs.teleflow.co/api/validate-the-mx-record-setup-for-the-inbound-parse-functionality/), [changes](https://docs.teleflow.co/api/get-changes/)
- Bootstrap each services as part of services provider or directly as a singleton class (setting injectable)
- A Sync service that will mirror an environment based a set of templates (layouts, integrations, workflow groups, workflows)

**WARNING**: 0.3.0 has breaking changes and the tests should be relied on for understanding the client libraries

## Dependencies

| dotnet teleflow | teleflow api [package](https://github.com/nxpkg/teleflow/pkgs/container/teleflow%2Fapi) | Notes                                                                                                                                                                                                                                   |
| ----------- | ---------------------------------------------------------------------------- | --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| 0.2.2       | <= 0.17                                                                      | Singleton client with Refit's use of RestService                                                                                                                                                                                        |
| 0.3.0       | >= 0.18                                                                      | 0.3.0 is not compatible with 0.2.2 and requires upgrade to code. Also 0.18 introduced a breaking change only found in 0.3.0. All 0.2.2 must be upgraded if used against the production system. HttpClient can now be used and injected. |
| 0.3.1       | >= 0.18                                                                      | Failed release. You will not find this release on Nuget.                                                                                                                                                                                |
| 0.3.2       | >= 0.18                                                                      | [BREAKING} Obsolete Notification Templates has been removed. Service registration separation of single client and each client. Teleflow.Extension and Teleflow.Sync released as packages.                                                       |

## Installation

```bash
dotnet add package Teleflow
```

## Configuration

### Direct instantiation

```csharp
using Teleflow.DTO;
using Teleflow.Models;
using Teleflow;

 var teleflowConfiguration = new TeleflowClientConfiguration
{
    // Defaults to https://api.teleflow.co/v1
    Url = "https://teleflow-api.my-domain.com/v1",
    ApiKey = "12345",
};

var teleflow = new TeleflowClient(teleflowConfiguration);

// Note: this client exposes all endpoints as methods but uses RestService
var subscribers = await teleflow.Subscriber.Get();
```

### Dependency Injection

Configure via settings

```appsettings.json
{
  "Teleflow": {
    "Url": "http://localhost:3000/v1",
    "ApiKey": "e36b820fcc9a68a83db6c79c30f1a461"
  }
}

```

Setup Injection via extension methods

```csharp

public static IServiceCollection RegisterNotificationSetupServices(
    this IServiceCollection services,
    IConfiguration configuration)
{
    // registers all clients with teleflow config from appsetting.json
    // the services inject HttpClient
    return services
        .RegisterTeleflowClients(configuration)
        // here as an example that the registered services are injected into local service
        .AddTransient<TeleflowNotificationService>();
}
```

Write your consuming code with the injected clients

```csharp
// then instantiate via injection
public class TeleflowNotificationService
{
    private readonly IEventClient _event;

    public TeleflowSyncService(IEventClient @event)
    {
        _event = @event;
    }

    public async Task Trigger(string subscriberId){
       var trigger = await Event.Trigger(
            new EventCreateData
            {
                EventName = 'event-name',
                To = { SubscriberId =subscriberId },
                Payload = new Payload("Bogus payload"),
            });
    }

    public record Payload(string Message)
    {
        [JsonProperty("message")] public string Message { get; set; }
    }
}
```

## Examples

Usage of the library is best understood by looking at the tests.

- [Integration Tests](https://github.com/nxpkg/teleflow-dotnet/tree/main/src/Teleflow.Tests/IntegrationTests): these show the minimal dependencies required to do one primary request (create, update, delete)
- [Acceptance Tests](https://github.com/nxpkg/teleflow-dotnet/tree/main/src/Teleflow.Tests/AcceptanceTests): these show a sequence of actions to complete a business process in an environment

## Repository Overview

[Teleflow](https://github.com/nxpkg/teleflow-dotnet/tree/main/src/Teleflow) is the main SDK with [Teleflow.Tests](https://github.com/nxpkg/teleflow-dotnet/tree/main/src/Teleflow.Tests) housing all unit tests. [Teleflow.Extensions](https://github.com/nxpkg/teleflow-dotnet/tree/main/src/Teleflow.Extensions) is required for DI and [Teleflow.Sync](https://github.com/nxpkg/teleflow-dotnet/tree/main/src/Teleflow.Sync)
if your are looking for mirroring environments.

### teleflow-dotnet

The key folders to look into:

- [DTO](https://github.com/nxpkg/teleflow-dotnet/tree/main/src/Teleflow/DTO) directory holds all objects needed to use the clients
- [Interfaces](https://github.com/nxpkg/teleflow-dotnet/tree/main/src/Teleflow/Interfaces) directory holds all interfaces that are intended to outline how a class should be structured
- [Models](https://github.com/nxpkg/teleflow-dotnet/tree/main/src/Teleflow/Models) directory holds various models that are sub-resources inside the DTOs

### Major changes

Github issues closed

#### 0.3.1

- #57
- #58
- #59
- #60
- #55

#### 0.3.0

- #19
- #20
- #21
- #34
- #48
- #49
- #50
- #47
- #45
