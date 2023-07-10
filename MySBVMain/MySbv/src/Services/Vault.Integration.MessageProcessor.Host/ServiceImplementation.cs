using System.Reflection;
using System.ServiceProcess;
using Infrastructure.Logging;
using Ninject;
using Vault.Integration.MessageProcessor.Host.Framework;

namespace Vault.Integration.MessageProcessor.Host
{
    /// <summary>
    ///     The actual implementation of the windows service goes here...
    /// </summary>
    [WindowsService("Vault.Integration.MessageProcessor.Host",
        DisplayName = "Vault.Integration.MessageProcessor.Host",
        Description = "The description of the Vault.Integration.MessageProcessor.Host service.",
        EventLogSource = "Vault.Integration.MessageProcessor.Host",
        StartMode = ServiceStartMode.Automatic)]
    public class ServiceImplementation : IWindowsService
    {
        /// <summary>
        ///     MEF Composition Container
        /// </summary>
        //private CompositionContainer _container;
        private IRunner _runner;

        /// <summary>
        ///     Service Implementation Constructor
        /// </summary>
        public ServiceImplementation()
        {
            this.Log().Debug(() =>
            {
                // create a Ninject kernel to resolve dependancies in our code automatically
                IKernel kernel = new StandardKernel();
                kernel.Load(Assembly.GetExecutingAssembly());

                _runner = kernel.Get<IRunner>();

                //var catalog = new AggregateCatalog(new DirectoryCatalog(@"."),
                //    new AssemblyCatalog(Assembly.GetExecutingAssembly()));
                //_container = new CompositionContainer(catalog);
                //_container.SatisfyImportsOnce(this);
            }, "Initializing NINJECT Catalog for Message Processor Host ");
        }

        /// <summary>
        ///     Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <filterpriority>2</filterpriority>
        public void Dispose()
        {
            //this.Log().Info(() => _container.Dispose(), "Disposing Container");
        }

        /// <summary>
        ///     This method is called when the service gets a request to start.
        /// </summary>
        /// <param name="args">Any command line arguments</param>
        public void OnStart(string[] args)
        {
            // ReSharper disable once ConvertToLambdaExpression
            //this.Log().Info(() => _container.GetExportedValue<IRunner>().Run(), "Running");
            this.Log().Info(() => _runner.Run(), "Running");
        }

        /// <summary>
        ///     This method is called when the service gets a request to stop.
        /// </summary>
        public void OnStop()
        {
            this.Log().Info(() => { }, "Response Processor Host Stopping");
        }

        /// <summary>
        ///     This method is called when a service gets a request to pause,
        ///     but not stop completely.
        /// </summary>
        public void OnPause()
        {
            this.Log().Info(() => { }, "Response Processor Host Pausing");
        }

        /// <summary>
        ///     This method is called when a service gets a request to resume
        ///     after a pause is issued.
        /// </summary>
        public void OnContinue()
        {
            this.Log().Info(() => { }, "Response Processor Host Continuing");
        }

        /// <summary>
        ///     This method is called when the machine the service is running on
        ///     is being shutdown.
        /// </summary>
        public void OnShutdown()
        {
            this.Log().Info(() => { }, "Response Processor Host Shutting Down");
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