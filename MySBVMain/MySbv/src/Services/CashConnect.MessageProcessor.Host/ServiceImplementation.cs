using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Reflection;
using System.ServiceProcess;
using System.Threading.Tasks;
using CashConnect.MessageProcessor.Host.Framework;
using Infrastructure.Logging;

namespace Vault.Integration.MessageProcessor.Host
{
    /// <summary>
    ///     The actual implementation of the windows service goes here...
    /// </summary>
    [WindowsService("CashConnect.MessageProcessor.Host",
        DisplayName = "CashConnect.MessageProcessor.Host",
        Description = "The description of the CashConnect.MessageProcessor.Host service.",
        EventLogSource = "CashConnect.MessageProcessor.Host",
        StartMode = ServiceStartMode.Automatic)]
    public class ServiceImplementation : IWindowsService
    {
        /// <summary>
        ///     MEF Composition Container
        /// </summary>
        private CompositionContainer _container;
        
        /// <summary>
        /// Constructor
        /// </summary>
        public ServiceImplementation()
        {
            this.Log().Debug(() =>
            {
                var catalog = new AggregateCatalog(new DirectoryCatalog(@"."),
                new AssemblyCatalog(Assembly.GetExecutingAssembly()));
                _container = new CompositionContainer(catalog);
                _container.SatisfyImportsOnce(this);
            }, "Initializing MEF Catalog for Cash Connect Message Processor Host ");
        }
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
            Task.Factory.StartNew(() =>
            {
                this.Log().Info(() => _container.GetExportedValue<IMessageProcessor>().Run(), "Running");
            });
        }

        /// <summary>
        ///     This method is called when the service gets a request to stop.
        /// </summary>
        public void OnStop()
        {
            this.Log().Info(() => { }, "Cash Connect Message Processor Host Stopping");
        }

        /// <summary>
        ///     This method is called when a service gets a request to pause,
        ///     but not stop completely.
        /// </summary>
        public void OnPause()
        {
            this.Log().Info(() => { }, "Cash Connect Message Processor Host Pausing");
        }

        /// <summary>
        ///     This method is called when a service gets a request to resume
        ///     after a pause is issued.
        /// </summary>
        public void OnContinue()
        {
            this.Log().Info(() => { }, "Cash Connect Message Processor Host Continuing");
        }

        /// <summary>
        ///     This method is called when the machine the service is running on
        ///     is being shutdown.
        /// </summary>
        public void OnShutdown()
        {
            this.Log().Info(() => { }, "Cash Connect Message Processor Host Shutting Down");
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