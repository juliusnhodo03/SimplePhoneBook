using System;
using Ninject;
using Quartz;
using Quartz.Impl;

namespace Hyphen.Integration.Service
{
    public class ScheduledJobs
    {
        public static void RegisterJobs()
        {
            // Create a new kernel and create the necessary bindings
            IKernel kernel = new StandardKernel();
            kernel.Bind<IJobUsingService>().To<JobUsingService>();

            // Create a scheduler and give it the Ninject job factory created earlier
            IScheduler scheduler = new StdSchedulerFactory().GetScheduler();
            scheduler.JobFactory = new NinjectJobFactory(kernel);

            // Create the job with the interface which will be injected
            IJobDetail job = JobBuilder.Create<IJobUsingService>()
                .WithIdentity("IJobUsingService")
                .Build();

            // Create the trigger
            ITrigger trigger = TriggerBuilder.Create()
                .WithDailyTimeIntervalSchedule(s =>
                    s.WithIntervalInMinutes(30)                    
                        .OnEveryDay()
                        .StartingDailyAt(new TimeOfDay(DateTime.Now.Hour, DateTime.Now.Minute)))
                .Build();

            scheduler.ScheduleJob(job, trigger);
        }
    }
}