using System.ComponentModel.Composition;
using Quartz;
using Quartz.Impl;

namespace Hyphen.Integration.Reconciliation.Host
{
    /// <summary>
    ///     Handles the Scheduled job that handles the Input Processor
    /// </summary>
    [Export(typeof (ProcessorManager))]
    public class ProcessorManager
    {
        /// <summary>
        ///     Instantiates and Runs the output processor job
        /// </summary>
        public void Run(IJob inputProcessorJob)
        {
            var configuration = new SchedulerConfiguration();
            configuration.Initialize();

            // construct a scheduler factory
            var factory = new StdSchedulerFactory();

            // get a scheduler
            IScheduler scheduler = factory.GetScheduler();
            scheduler.Start();

            // define the job and tie it to our HelloJob class
            var job = new JobDetailImpl("InputProcessorJob", inputProcessorJob.GetType());

            // Trigger the job to run now, and then every [configuration.Interval] seconds
            ITrigger trigger = TriggerBuilder.Create()
                .WithIdentity("InputProcessorJobTrigger", "InputProcessorGroup")
                .StartNow()
                .WithSimpleSchedule(x => x
                    .WithIntervalInMinutes(configuration.Interval)
                    .RepeatForever())
                .Build();

            scheduler.ScheduleJob(job, trigger);
        }
    }
}