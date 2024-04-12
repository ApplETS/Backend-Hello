using api.tasks.Jobs;

using Microsoft.AspNetCore.Http;

using Quartz;
using Quartz.Logging;

namespace api.core.Misc;

public static class SchedulerSetup
{
    public const string SetPublicationPublishedKey = "_SetPublicationPublished";
    public const string NotificationByEmailKey = "_NotificationByEmail";

    public static void SetupScheduler(this IServiceCollection services)
    {
        LogProvider.SetCurrentLogProvider(new ConsoleLogProvider());

        services.AddQuartz();

        services.AddQuartzHostedService(opt =>
        {
            opt.WaitForJobsToComplete = true;
            opt.AwaitApplicationStarted = true;
        });
    }

    public static async Task AddSchedulerAsync(this IServiceProvider services)
    {
        var schedulerFactory = services.GetRequiredService<ISchedulerFactory>();
        var scheduler = await schedulerFactory.GetScheduler();

        // define the job and tie it to our HelloJob class
        var job = JobBuilder.Create<SetPublicationEventJob>()
            .WithIdentity(TriggerEveryMinutes, BackgroundTaskKey)
            .Build();

        // Trigger the job to run now, and then every 40 seconds
        var trigger = TriggerBuilder.Create()
            .WithIdentity(TriggerEveryMinutes, BackgroundTaskKey)
            .StartNow()
            .WithSimpleSchedule(x => x
                .WithInterval(new TimeSpan(0, 1, 0, 0, 0, 0))
                .RepeatForever())
            .Build();

        await scheduler.ScheduleJob(job, trigger);
    }
}
