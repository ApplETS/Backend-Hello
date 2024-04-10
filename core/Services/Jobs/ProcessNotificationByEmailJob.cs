using api.core.Misc;
using api.core.repositories.abstractions;
using api.core.Services.Abstractions;

using Quartz;

namespace api.tasks.Jobs;

public class ProcessNotificationByEmailJob : IJob
{
    public static readonly JobKey Key = new (nameof(ProcessNotificationByEmailJob), SchedulerSetup.NotificationByEmailKey);

    private readonly IServiceProvider _provider;

    public ProcessNotificationByEmailJob(IServiceProvider serviceProvider)
    {
        _provider = serviceProvider;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        try
        {
            using var serviceScope = _provider.CreateScope();
            var notifService = (serviceScope.ServiceProvider?.GetService<INotificationService>()) ?? throw new SchedulerException("Cannot instantiate NotificationService from the AspNet Core IOC.");
            var logger = (serviceScope.ServiceProvider?.GetService<ILogger<ProcessNotificationByEmailJob>>()) ?? throw new SchedulerException("Cannot instantiate Logger from the AspNet Core IOC.");

            var emailsCount = await notifService.SendNewsForRemainingPublication();
            logger.LogInformation($"[NotificationByEmailJob] {emailsCount} emails were sent out");
        }
        catch (Exception e)
        {
            throw new SchedulerException($"Problem while instantiating job '{context.JobDetail.Key}' from the AspNet Core IOC.", e);
        }
    }
}
