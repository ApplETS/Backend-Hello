using api.core.Misc;
using api.core.services.abstractions;

using Quartz;

namespace api.tasks.Jobs;

public class NotificationByEmailJob : IJob
{
    public static readonly JobKey Key = new (nameof(NotificationByEmailJob), SchedulerSetup.BackgroundTaskKey);

    private readonly IServiceProvider _provider;

    public NotificationByEmailJob(IServiceProvider serviceProvider)
    {
        _provider = serviceProvider;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        try
        {
            using var serviceScope = _provider.CreateScope();
            var eventService = (serviceScope.ServiceProvider?.GetService<IEventService>()) ?? throw new SchedulerException("Cannot instantiate EventService from the AspNet Core IOC.");
            
            var publishedEventCount = eventService.PublishedIfApprovedPassedDue();

            await Console.Out.WriteLineAsync($"[NotificationByEmailJob] was trigger and transform {publishedEventCount} events from APPROVED->PUBLISHED");
        }
        catch (Exception e)
        {
            throw new SchedulerException($"Problem while instantiating job '{context.JobDetail.Key}' from the AspNet Core IOC.", e);
        }
    }
}
