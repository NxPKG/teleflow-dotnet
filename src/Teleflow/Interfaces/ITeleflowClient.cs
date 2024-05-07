namespace Teleflow.Interfaces;

public interface ITeleflowClient
{
    ISubscriberClient Subscriber { get; }
    IEventClient Event { get; }
    ITopicClient Topic { get; }
    IWorkflowGroupClient WorkflowGroup { get; }
    IWorkflowClient Workflow { get; }
    ILayoutClient Layout { get; }
    IIntegrationClient Integration { get; }
    INotificationsClient Notifications { get; }
    IMessageClient Message { get; }
    IExecutionDetailsClient ExecutionDetails { get; }
}