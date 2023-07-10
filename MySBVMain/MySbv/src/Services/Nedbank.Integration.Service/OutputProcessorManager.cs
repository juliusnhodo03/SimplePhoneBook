using System.ComponentModel.Composition;
using Quartz;
using Quartz.Impl;

namespace Nedbank.Integration.Service
{
    /// <summary>
    ///     Handles the Scheduled job that handles the Output Processor
    /// </summary>
    [Export(typeof (OutputProcessorManager))]
    public class OutputProcessorManager
    {
        /// <summary>
        ///     Instantiates and Runs the output processor job
        /// </summary>
        public void Run(IJob outputProcessorJob)
        {
            var configuration = new SchedulerConfiguration();
            configuration.Initialize();
            // construct a scheduler factory
            ISchedulerFactory factory = new StdSchedulerFactory();

            // get a scheduler
            IScheduler scheduler = factory.GetScheduler();
            scheduler.Start();

            // define the job and tie it to our OutputProcessorJob class
            IJobDetail job = new JobDetailImpl("OutputProcessorJob", outputProcessorJob.GetType());

            // Trigger the job to run now, and then every [configuration.Interval] seconds
            ITrigger trigger = TriggerBuilder.Create()
                .WithIdentity("OutputProcessorJobTrigger", "OutputProcessorGroup")
                .StartNow()
                .WithSimpleSchedule(x => x
                    .WithIntervalInMinutes(configuration.Interval)
                    .RepeatForever())
                .Build();

            scheduler.ScheduleJob(job, trigger);
        }
    }
}