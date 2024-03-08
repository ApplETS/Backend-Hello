using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
