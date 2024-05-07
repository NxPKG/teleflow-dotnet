using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using Teleflow.DTO;
using Teleflow.DTO.Integrations;
using Teleflow.Interfaces;
using Teleflow.Models.Integrations;
using Teleflow.Models.Subscribers.Preferences;
using Teleflow.Sync.Models;
using Teleflow.Sync.Services;
using Teleflow.Tests.IntegrationTests;
using Teleflow.Utils;
using Xunit;
using Xunit.Abstractions;

namespace Teleflow.Tests.MicroTests.Sync;

public class SyncIntegrationTests : BaseIntegrationTest
{
    private static readonly TemplateIntegration TeleflowInAppTemplateIntegration = new()
    {
        Identifier = "teleflow-in-app-e2e-tests-XJJHG",
        ProviderId = "teleflow",
        Channel = ChannelTypeEnum.InApp,
        Name = "Teleflow In-App",
        Active = true,
        Credentials = new Credentials
        {
            Hmac = true,
        },
    };

    private readonly Mock<IIntegrationClient> _integrationClient;

    public SyncIntegrationTests(ITestOutputHelper output) : base(output)
    {
        _integrationClient = new Mock<IIntegrationClient>();

        // looks to be a need to registered the swapped out implementations before any are
        // instantiated. Expectations are set in the tests.
        Register(
            services => { services.SwapTransient(_ => _integrationClient.Object); });
    }

    public static IEnumerable<object[]> Data => new List<object[]>
    {
        new object[]
        {
            "both sides empty",
            Array.Empty<TemplateIntegration>(),
            Array.Empty<Integration>(),
            (Func<Times>)Times.Once,
            (Func<Times>)Times.Never,
            (Func<Times>)Times.Never,
            (Func<Times>)Times.Never,
        },
        new object[]
        {
            "new template - create at dest",
            new List<TemplateIntegration>
            {
                TeleflowInAppTemplateIntegration,
            },
            Array.Empty<Integration>(),
            (Func<Times>)Times.Once,
            (Func<Times>)Times.Once,
            (Func<Times>)Times.Never,
            (Func<Times>)Times.Never,
        },
        new object[]
        {
            "same on both sides - no changes",
            new List<TemplateIntegration>
            {
                TeleflowInAppTemplateIntegration,
            },
            new List<Integration>
            {
                new()
                {
                    Identifier = TeleflowInAppTemplateIntegration.Identifier,
                    Channel = TeleflowInAppTemplateIntegration.Channel.ToEnumString(),
                    ProviderId = TeleflowInAppTemplateIntegration.ProviderId,
                    Active = TeleflowInAppTemplateIntegration.Active,
                    Credentials = TeleflowInAppTemplateIntegration.Credentials,
                    Name = TeleflowInAppTemplateIntegration.Name,
                },
            },
            (Func<Times>)Times.Once,
            (Func<Times>)Times.Never,
            (Func<Times>)Times.Never,
            (Func<Times>)Times.Never,
        },
        new object[]
        {
            "template changed - update dest",
            new List<TemplateIntegration>
            {
                TeleflowInAppTemplateIntegration,
            },
            new List<Integration>
            {
                new()
                {
                    Identifier = TeleflowInAppTemplateIntegration.Identifier,
                    Channel = TeleflowInAppTemplateIntegration.Channel.ToEnumString(),
                    ProviderId = TeleflowInAppTemplateIntegration.ProviderId,
                    Active = !TeleflowInAppTemplateIntegration.Active, // changed
                    Credentials = new Credentials
                    {
                        Hmac = false, // changed
                    },
                    Name = TeleflowInAppTemplateIntegration.Name,
                },
            },
            (Func<Times>)Times.Once,
            (Func<Times>)Times.Never,
            (Func<Times>)Times.Once,
            (Func<Times>)Times.Never,
        },
        new object[]
        {
            "template removed - delete at dest",
            Array.Empty<TemplateIntegration>(),
            new List<Integration>
            {
                new()
                {
                    Identifier = TeleflowInAppTemplateIntegration.Identifier,
                    Channel = TeleflowInAppTemplateIntegration.Channel.ToEnumString(),
                    ProviderId = TeleflowInAppTemplateIntegration.ProviderId,
                    Active = TeleflowInAppTemplateIntegration.Active,
                    Credentials = TeleflowInAppTemplateIntegration.Credentials,
                    Name = TeleflowInAppTemplateIntegration.Name,
                },
            },
            (Func<Times>)Times.Once,
            (Func<Times>)Times.Never,
            (Func<Times>)Times.Never,
            (Func<Times>)Times.Once,
        },
    };


    [Theory]
    [MemberData(nameof(Data))]
    public async Task Tests(
        string test,
        IList<TemplateIntegration> templateIntegrations,
        IList<Integration> currentSetAtDestination,
        Func<Times> getCalls,
        Func<Times> createCalls,
        Func<Times> updateCalls,
        Func<Times> deleteCalls
    )
    {
        Output.WriteLine(test);

        _integrationClient
            .Setup(x => x.Get())
            .ReturnsAsync(new TeleflowResponse<IEnumerable<Integration>>(currentSetAtDestination));

        var syncClient = Get<ITeleflowSync<TemplateIntegration>>();

        await syncClient.Sync(templateIntegrations);

        _integrationClient.Verify(x => x.Get(), getCalls);
        _integrationClient.Verify(x => x.Create(It.IsAny<IntegrationCreateData>()), createCalls);
        _integrationClient.Verify(x => x.Update(It.IsAny<string>(), It.IsAny<IntegrationEditData>()), updateCalls);
        _integrationClient.Verify(x => x.Delete(It.IsAny<string>()), deleteCalls);
    }
}