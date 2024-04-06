using api.tasks.Jobs;

using Quartz;
using Quartz.Logging;

namespace api.core.Misc;

public static class SchedulerSetup
{
    public static string SetPublicationPublishedKey = "_SetPublicationPublished";
    public static string NotificationByEmailKey = "_NotificationByEmail";

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

        // define the first job
        var job = JobBuilder.Create<SetPublicationEventJob>()
            .WithIdentity(SetPublicationPublishedKey)
            .Build();

        var trigger = TriggerBuilder.Create()
            .StartNow()
            .WithSimpleSchedule(x => x
                .WithInterval(new TimeSpan(0, 0, 10, 0, 0, 0)) // every 10 minutes
                .RepeatForever())
            .Build();

        await scheduler.ScheduleJob(job, trigger);

        // define the second job
        job = JobBuilder.Create<NotificationByEmailJob>()
            .WithIdentity(NotificationByEmailKey)
            .Build();

        trigger = TriggerBuilder.Create()
            .WithSimpleSchedule(x => x
                .WithInterval(new TimeSpan(0, 1, 0, 0, 0, 0)) // Every 1 hour
                .RepeatForever())
            .Build();
        await scheduler.ScheduleJob(job, trigger);
    }
}
