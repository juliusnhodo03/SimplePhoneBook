using System;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.Reflection;
using System.ServiceProcess;
using Hyphen.Integration.Reconciliation.Controller.MetaData;
using Hyphen.Integration.Reconciliation.Host.Framework;
using Infrastructure.Logging;
using Quartz;

namespace Hyphen.Integration.Reconciliation.Host
{
    /// <summary>
    ///     The actual implementation of the windows service goes here...
    /// </summary>
    [WindowsService("Hyphen.Integration.Reconciliation.Host",
        DisplayName = "Hyphen.Integration.Reconciliation.Host",
        Description = "The description of the Hyphen.Integration.Reconciliation.Host service.",
        EventLogSource = "Hyphen.Integration.Reconciliation.Host",
        StartMode = ServiceStartMode.Automatic)]
    public class ServiceImplementation : IWindowsService
    {
        /// <summary>
        ///     MEF Composition Container
        /// </summary>
        private CompositionContainer _container;

        /// <summary>
        ///     Service Implementation Constructor
        /// </summary>
        public ServiceImplementation()
        {
            this.Log().Debug(() =>
            {
                try
                {
                    var catalog = new AggregateCatalog(new ComposablePartCatalog[]
                    {
                        new AssemblyCatalog(Assembly.GetExecutingAssembly()),
                        new DirectoryCatalog(@".")
                    });

                    //var catalog = new AggregateCatalog();
                    _container = new CompositionContainer(catalog);
                    _container.SatisfyImportsOnce(this);
                    Bootstrapper.SetContainer(_container);
                }
                catch (CompositionException compositionException)
                {
                    this.Log().Fatal("Failed to Compose MEF Parts... !", compositionException);
                    this.Log().Info(compositionException.ToString());
                }
                catch (Exception ex)
                {
                    this.Log().Fatal("Failed to Compose MEF Parts... !", ex);
                }
            }, "Initializing MEF Catalog");
        }

        [ImportMany(AllowRecomposition = true)]
        public Lazy<IJob, IProcessorTypeMetadata>[] Parts { get; set; }

        /// <summary>
        ///     Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <filterpriority>2</filterpriority>
        public void Dispose()
        {
            this.Log().Info(() => _container.Dispose(), "Disposing Container");
        }


        /// <summary>
        ///     This method is called when the service gets a request to start.
        /// </summary>
        /// <param name="args">Any command line arguments</param>
        public void OnStart(string[] args)
        {
            foreach (var part in Parts)
            {
                switch (part.Metadata.ProcessorType)
                {
                    case ProcessorType.Output:
                        Lazy<IJob, IProcessorTypeMetadata> inputJobPartLazy = part;
                        this.Log()
                            .Info(
                                () => _container.GetExportedValue<ProcessorManager>().Run(inputJobPartLazy.Value),
                                "Input Processor Manager Running");
                        break;
                    case ProcessorType.Input:                       
                        break;
                    default:
                        this.Log()
                            .Fatal(string.Format("Invalid processor type supplied [{0}]", part.Metadata.ProcessorType),
                                new ArgumentException("Invalid Processor type"));
                        break;
                }
            }
        }

        /// <summary>
        ///     This method is called when the service gets a request to stop.
        /// </summary>
        public void OnStop()
        {
            this.Log().Info(() => { }, "Hyphen.Integration.Reconciliation.Host Stopping");
        }

        /// <summary>
        ///     This method is called when a service gets a request to pause,
        ///     but not stop completely.
        /// </summary>
        public void OnPause()
        {
            this.Log().Info(() => { }, "Hyphen.Integration.Reconciliation.Host Pausing");
        }

        /// <summary>
        ///     This method is called when a service gets a request to resume
        ///     after a pause is issued.
        /// </summary>
        public void OnContinue()
        {
            this.Log().Info(() => { }, "Hyphen.Integration.Reconciliation.Host Continuing");
        }

        /// <summary>
        ///     This method is called when the machine the service is running on
        ///     is being shutdown.
        /// </summary>
        public void OnShutdown()
        {
            this.Log().Info(() => { }, "Hyphen.Integration.Reconciliation.Host Shutting Down");
        }

        /// <summary>
        ///     This method is called when a custom command is issued to the service.
        /// </summary>
        /// <param name="command">The command identifier to execute.</param>
        public void OnCustomCommand(int command)
        {
        }
    }
}