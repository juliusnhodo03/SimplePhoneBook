using System;
using System.Collections.Generic;
using System.Linq;
using Infrastructure.Logging;
using Quartz;
using Quartz.Spi;

namespace Nedbank.Integration.Service
{
    public class QuartzMefJobFactory : IJobFactory
    {
        public IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
        {
            try
            {
                IEnumerable<Lazy<IJob>> jobs = Bootstrapper.Instance.Container.GetExports<IJob>();
                return jobs.First(job => job.Value.GetType() == bundle.JobDetail.JobType).Value;
            }
            catch (Exception exception)
            {
                this.Log()
                    .Fatal(string.Format("Problem instantiating class - [{0}]", bundle.JobDetail.JobType), exception);
                throw new SchedulerException("Problem instantiating class " + bundle.JobDetail.JobType, exception);
            }
        }

        public void ReturnJob(IJob job)
        {
            throw new NotImplementedException();
        }
    }
}