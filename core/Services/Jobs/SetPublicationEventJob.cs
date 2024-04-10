using api.core.Misc;
using api.core.services.abstractions;

using Quartz;

namespace api.tasks.Jobs;

public class SetPublicationEventJob : IJob
{
    public static readonly JobKey Key = new (nameof(SetPublicationEventJob), SchedulerSetup.SetPublicationPublishedKey);

    private readonly IServiceProvider _provider;

    public SetPublicationEventJob(IServiceProvider serviceProvider)
    {
        _provider = serviceProvider;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        try
        {
            using var serviceScope = _provider.CreateScope();
            var eventService = (serviceScope.ServiceProvider?.GetService<IEventService>()) ?? throw new SchedulerException("Cannot instantiate EventService from the AspNet Core IOC.");
            var logger = (serviceScope.ServiceProvider?.GetService<ILogger<SetPublicationEventJob>>()) ?? throw new SchedulerException("Cannot instantiate Logger from the AspNet Core IOC.");

            var publishedEventCount = eventService.PublishedIfApprovedPassedDue();
            logger.LogInformation("[SetPublicationEventJob] was trigger and transform {publishedEventCount} events from APPROVED->PUBLISHED", publishedEventCount);
        }
        catch (Exception e)
        {
            throw new SchedulerException($"Problem while instantiating job '{context.JobDetail.Key}' from the AspNet Core IOC.", e);
        }
    }
}
