using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Newtonsoft.Json;
using Teleflow.DTO;
using Teleflow.DTO.Events;
using Teleflow.DTO.Integrations;
using Teleflow.DTO.Notifications;
using Teleflow.DTO.Subscribers;
using Teleflow.DTO.Subscribers.Preferences;
using Teleflow.DTO.Topics;
using Teleflow.DTO.WorkflowGroups;
using Teleflow.DTO.Workflows;
using Teleflow.Interfaces;
using Teleflow.Models;
using Teleflow.Models.Subscribers.Preferences;
using Teleflow.Models.Triggers;
using Teleflow.Models.Workflows;
using Teleflow.Models.Workflows.Step;
using Teleflow.Models.Workflows.Step.Message;
using Teleflow.Tests.IntegrationTests;
using ParkSquare.Testing.Generators;
using Polly;
using Xunit;
using Xunit.Abstractions;
using Xunit.Sdk;
using TopicCreateData = Teleflow.DTO.Events.TopicCreateData;

namespace Teleflow.Tests.AcceptanceTests;

public class NotificationTests : BaseIntegrationTest
{
    public NotificationTests(ITestOutputHelper output) : base(output)
    {
    }

    [RunnableInDebugOnly]
    public async Task E2E_InApp_Event_Test()
    {
        var (workflow, eventName) = await MakeInAppWorkflow();
        var subscriber = await MakeSubscriberOnWorkflow(workflow);

        var trigger = await Event.Trigger(
            new EventCreateData
            {
                EventName = eventName,
                To = { SubscriberId = subscriber.SubscriberId! },
                Payload = new TestMessage(),
            });

        trigger.Data.Acknowledged.Should().BeTrue();

        await VerifyNotifications(subscriber);
    }

    [RunnableInDebugOnly]
    public async Task E2E_InApp_Topic_Test()
    {
        var (workflow, eventName) = await MakeInAppWorkflow();
        var subscriber = await MakeSubscriberOnWorkflow(workflow);
        var subscriber2 = await MakeSubscriberOnWorkflow(workflow);

        var topic = await Make<Topic>(
            subscriber: subscriber,
            additionalSubscribers: new List<Subscriber> { subscriber2 });

        var trigger = await Event.Create(
            new TopicCreateData
            {
                EventName = eventName,
                To = new[] { new TopicTrigger(topic.Key) },
                Payload = new TestMessage(),
            });

        trigger.Data.Acknowledged.Should().BeTrue();

        await VerifyNotifications(subscriber);
        await VerifyNotifications(subscriber2);
    }


    private async Task VerifyNotifications(Subscriber subscriber)
    {
        // WAIT for system to catch up given it is async
        const int maxRetryAttempts = 3;
        var retryPolicy = Policy
            .Handle<XunitException>()
            .WaitAndRetryAsync(maxRetryAttempts, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));

        await retryPolicy.ExecuteAsync(async () =>
        {
            // check, has it arrived?
            var notificationsForSubscriber = await Get<INotificationsClient>()
                .Get(new NotificationQueryParams { SubscriberIds = new[] { subscriber.SubscriberId } });

            notificationsForSubscriber
                .Data
                .Should()
                .HaveCount(1);

            var hasFailed = notificationsForSubscriber
                .Data.Any(x => x.Jobs.Any(y => y.Status.Equals("failed", StringComparison.InvariantCultureIgnoreCase)));

            // it may actually be failing for real and need to introspect reason
            if (hasFailed)
            {
                var message = notificationsForSubscriber
                    .Data
                    // Job has a status with specifics inside the execution details 
                    .Where(x => x.Jobs
                        .Any(y => y.Status.Equals("failed", StringComparison.InvariantCultureIgnoreCase)))
                    .SelectMany(s => s.Jobs)
                    // now find the reason/detail
                    .SelectMany(x => x.ExecutionDetails
                        .Where(z => z.Status.Equals("failed", StringComparison.InvariantCultureIgnoreCase)))
                    .Select(x => x.Detail)
                    .FirstOrDefault();

                // If you get here, it is likely the integration itself is turned off 
                // TODO: check that the integration is turned on
                Assert.Fail(message ?? "Reason unknown");
            }
        });

        await retryPolicy.ExecuteAsync(async () =>
        {
            var dataCount = (await Subscriber.GetInAppUnseen(subscriber.SubscriberId!))
                .Data
                .Count;
            var inAppMessages = await Subscriber.GetInApp(subscriber.SubscriberId);
            dataCount
                .Should()
                .Be(1);

            inAppMessages.Data.Should().NotBeEmpty();

            // var z = inAppMessages.Data.Where(x => x.Channels.Contains(ChannelTypeEnum.InApp));
        });
    }

    private async Task<(Workflow, string)> MakeInAppWorkflow()
    {
        var workflowGroup = await Make<WorkflowGroup>(new WorkflowGroupCreateData
        {
            Name = $"End2EndGroup ({StringGenerator.SequenceOfAlphaNumerics(5)})",
        });
        var workflow = await Make<Workflow>(new WorkflowCreateData
        {
            Name = $"In-App [End2end {StringGenerator.SequenceOfAlphaNumerics(10)}]",
            Description = StringGenerator.LoremIpsum(5),
            WorkflowGroupId = workflowGroup.Id,
            PreferenceSettings = new PreferenceChannels
            {
                InApp = true,
            },
            Steps = new Step[]
            {
                new()
                {
                    Name = $"In-App [End2end ({StringGenerator.SequenceOfAlphaNumerics(5)})]",
                    Template = new InAppMessageTemplate
                    {
                        Content = "Fantastic move! {{message}}",
                        Variables = new[]
                        {
                            new TemplateVariable
                            {
                                Name = "message",
                                Type = TemplateVariableTypeEnum.String,
                                Required = true,
                            },
                        },
                    },
                    Active = true,
                },
            },
            Active = true,
        });


        // Now, ensure that the integration is active. Otherwise, the trigger will not deliver
        // but reports that the subscriber has no active integrations where they problem is that
        // the system has none to offer even though the subscriber has registered
        var existingIntegration = (await Integration.Get())
            .Data
            .SingleOrDefault(x => x.ProviderId == "teleflow");
        if (existingIntegration is not null && !existingIntegration.Active)
        {
            await Integration.Update(
                existingIntegration.Id,
                existingIntegration.ToEditData(x => x.Active = true));
        }
        else if (existingIntegration is null)
        {
            await Make<Integration>(providerId: "teleflow");
        }

        var eventName = workflow.Triggers.FirstOrDefault()?.Identifier;
        return (workflow, eventName);
    }

    private async Task<Subscriber> MakeSubscriberOnWorkflow(Workflow workflow)
    {
        var subscriber = await Make<Subscriber>();

        // update subscriber preferences
        var preferences = await Subscriber.GetPreferences(subscriber.SubscriberId!);

        // enable notification
        var templateId = preferences.Data.Single(x => x.Template.Name == workflow.Name).Template.Id;

        await Subscriber.UpdatePreference(
            subscriber.SubscriberId!,
            templateId,
            new SubscriberPreferenceEditData
            {
                Channel = new Channel
                {
                    Type = ChannelTypeEnum.InApp,
                    Enabled = true,
                },
            });
        return subscriber;
    }
}

public class TestMessage
{
    [JsonProperty(PropertyName = "message")]
    public string Message { get; set; } = "Test Message";
}