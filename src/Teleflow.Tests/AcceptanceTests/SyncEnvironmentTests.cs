using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Teleflow.DTO.Layouts;
using Teleflow.Interfaces;
using Teleflow.Models.Integrations;
using Teleflow.Models.Subscribers.Preferences;
using Teleflow.Models.Workflows;
using Teleflow.Models.Workflows.Step;
using Teleflow.Sync;
using Teleflow.Sync.Models;
using Teleflow.Sync.Services;
using Teleflow.Tests.IntegrationTests;
using Xunit.Abstractions;

namespace Teleflow.Tests.AcceptanceTests;

public class SyncEnvironmentTests : BaseIntegrationTest
{
    private static readonly TemplateIntegration TeleflowInApp = new()
    {
        Identifier = "teleflow-in-app-e2e-tests-ZJHGY",
        ProviderId = "teleflow",
        Channel = ChannelTypeEnum.InApp,
        Name = "Teleflow In-App",
        Active = true,
        Credentials = new Credentials
        {
            Hmac = true,
        },
    };

    private static readonly TemplateIntegration CustomEmail = new()
    {
        Identifier = "custom-email-e2e-tests-ZJHGY",
        ProviderId = "nodemailer",
        Channel = ChannelTypeEnum.Email,
        // Name = "Teleflow In-App",
        Active = true,
        Credentials = new Credentials
        {
            User = "AKIA3XI.....NJ4JPUOG",
            Password = "BDmxG...................rTphx",
            Host = "email-smtp.us-east-1.amazonaws.com",
            Port = "587",
            Secure = false,
            RequireTls = true,
            IgnoreTls = false,
            From = "hello@example.com",
            SenderName = "Hello",
        },
    };


    private static readonly TemplateIntegration AmazonSnsSms = new()
    {
        Identifier = "sns-sms-e2e-tests-ZJHGY",
        ProviderId = "sns",
        Channel = ChannelTypeEnum.Sms,
        Name = "Amazon SNS SMS",
        Active = true,
        Credentials = new Credentials
        {
            ApiKey = "AKIA3..........X4M",
            SecretKey = "/uAP61+RS.............E2yQ9Z",
            Region = "ap-southeast-2",
        },
    };

    public SyncEnvironmentTests(ITestOutputHelper output) : base(output)
    {
    }

    /// <summary>
    ///     WARNING: this test will synchronise your entire environment (that is it will delete existing resources)
    /// </summary>
    [RunnableInDebugOnly]
    public async Task WorkflowTest()
    {
        DeRegisterExceptionHandler();

        await Get<ITeleflowSync<TemplateIntegration>>().Sync(
            new[]
            {
                TeleflowInApp,
                CustomEmail,
                AmazonSnsSms,
            });

        // depending on environment there can hidden workflow attached to layouts that this
        // test will continue on if it experiences a 409
        try
        {
            await Get<ITeleflowSync<TemplateLayout>>().Sync(
                new TemplateLayout
                {
                    Name = "Default Layout",
                    Description = "Updated/created from acceptance tests",
                    Content = LayoutCreateData.BodyExpression,
                    IsDefault = true,
                });
        }
        catch
        {
            // ignored 
        }

        await Get<ITeleflowSync<TemplateWorkflowGroup>>().Sync(
            new List<TemplateWorkflowGroup>
            {
                new() { Name = "Default WorkflowGroup [e2e test]" },
                new() { Name = "Other WorkflowGroup [e2e test]" },
            });

        var workflowGroups = await Get<IWorkflowGroupClient>().Get();
        var workflowGroup =
            workflowGroups?.Data?.SingleOrDefault(x => x.Name.Equals("Other WorkflowGroup [e2e test]"));

        workflowGroup.Should().NotBeNull();

        await Get<ITeleflowSync<TemplateWorkflow>>().Sync(
            new TemplateWorkflow
            {
                Name = "Invite (e2e)",
                Description = "Users are notified of an invitation (e2e)",
                WorkflowGroupId = workflowGroup!.Id,
                PreferenceSettings = new PreferenceChannels
                {
                    InApp = true,
                    Sms = true,
                    Email = true,
                },
                Steps = new[]
                {
                    StepFactory.InApp(true),
                    StepFactory.Sms(true),
                    StepFactory.DigestRegular(true),
                    new Step
                    {
                        Name = "Email",
                        Template = MessageTemplateFactory.EmailMessageTemplate(),
                        Active = true,
                    },
                },
                Active = true,
            });
    }
}