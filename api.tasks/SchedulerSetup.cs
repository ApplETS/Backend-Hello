using api.tasks.Jobs;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace api.tasks;

public static class SchedulerSetup
{
    public static string BackgroundTaskKey = "_BackgroundTask";
    private static string TriggerEveryDay = "_TriggerEveryDay";

    public static void SetupScheduler(this IServiceCollection services)
    {
        services.AddQuartz(opt =>
        {
        });

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
        var job = JobBuilder.Create<HelloJob>()
            .WithIdentity(TriggerEveryDay, BackgroundTaskKey)
            .Build();

        // Trigger the job to run now, and then every 40 seconds
        var trigger = TriggerBuilder.Create()
            .WithIdentity(TriggerEveryDay, BackgroundTaskKey)
            .StartNow()
            .WithSimpleSchedule(x => x
                .WithInterval(new TimeSpan(1,0,0,0,0,0))
                .RepeatForever())
            .Build();

        await scheduler.ScheduleJob(job, trigger);
    }
}
