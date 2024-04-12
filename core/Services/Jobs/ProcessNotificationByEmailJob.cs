using api.core.Misc;
using api.core.repositories.abstractions;
using api.core.Services.Abstractions;

using Quartz;

namespace api.core.Services.Jobs;

public class ProcessNotificationByEmailJob(IServiceProvider provider) : IJob
{
    public static readonly JobKey Key = new(nameof(ProcessNotificationByEmailJob), SchedulerSetup.NotificationByEmailKey);

    public async Task Execute(IJobExecutionContext context)
    {
        try
        {
            using var serviceScope = provider.CreateScope();
            var notifService = (serviceScope.ServiceProvider?.GetService<INotificationService>()) ?? throw new SchedulerException("Cannot instantiate NotificationService from the AspNet Core IOC.");
            var logger = (serviceScope.ServiceProvider?.GetService<ILogger<ProcessNotificationByEmailJob>>()) ?? throw new SchedulerException("Cannot instantiate Logger from the AspNet Core IOC.");

            var emailsCount = await notifService.SendNewsForRemainingPublication();
            logger.LogInformation("[NotificationByEmailJob] {emailsCount} emails were sent out", emailsCount);
        }
        catch (Exception e)
        {
            throw new SchedulerException($"Problem while instantiating job '{context.JobDetail.Key}' from the AspNet Core IOC.", e);
        }
    }
}
