using Quartz;

namespace api.tasks.Jobs;

public class HelloJob : IJob
{
    public static readonly JobKey Key = new JobKey("HelloJob", SchedulerSetup.BackgroundTaskKey);

    public Task Execute(IJobExecutionContext context)
    {
        Console.WriteLine("HelloJob is executing.");
        return Task.CompletedTask;
    }
}
