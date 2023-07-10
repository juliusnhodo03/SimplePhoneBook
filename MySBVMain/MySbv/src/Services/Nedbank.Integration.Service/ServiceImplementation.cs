using System;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.Reflection;
using System.ServiceProcess;
using Infrastructure.Logging;
using Nedbank.Integration.ControlPanel.MetaData;
using Nedbank.Integration.Service.Framework;
using Quartz;

namespace Nedbank.Integration.Service
{
    /// <summary>
    ///     The actual implementation of the windows service goes here...
    /// </summary>
    [Export(typeof (IWindowsService))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    [WindowsService("Nedbank.Integration.Service",
        DisplayName = "Nedbank.Integration.Service",
        Description = "Integrates Deposits with Nedbank",
        EventLogSource = "Nedbank.Integration.Service",
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
                    case ProcessorType.Input:
                        Lazy<IJob, IProcessorTypeMetadata> inputJobPartLazy = part;
                        this.Log()
                            .Info(
                                () => _container.GetExportedValue<InputProcessorManager>().Run(inputJobPartLazy.Value),
                                "Input Processor Manager Running");
                        break;
                    case ProcessorType.Output:
                        Lazy<IJob, IProcessorTypeMetadata> outputJobPartLazy = part;
                        this.Log()
                            .Info(
                                () => _container.GetExportedValue<OutputProcessorManager>().Run(outputJobPartLazy.Value),
                                "Output Processor Manager Running");
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
            this.Log().Info(() => { }, "Nedbank Integration Service Stopping");
        }

        /// <summary>
        ///     This method is called when a service gets a request to pause,
        ///     but not stop completely.
        /// </summary>
        public void OnPause()
        {
            this.Log().Info(() => { }, "Nedbank Integration Service Pausing");
        }

        /// <summary>
        ///     This method is called when a service gets a request to resume
        ///     after a pause is issued.
        /// </summary>
        public void OnContinue()
        {
            this.Log().Info(() => { }, "Nedbank Integration Service Continuing");
        }

        /// <summary>
        ///     This method is called when the machine the service is running on
        ///     is being shutdown.
        /// </summary>
        public void OnShutdown()
        {
            this.Log().Info(() => { }, "Nedbank Integration Service Shutting Down");
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