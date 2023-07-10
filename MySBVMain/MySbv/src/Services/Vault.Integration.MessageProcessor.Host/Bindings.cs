using Application.Modules.Common;
using Domain.Logging;
using Domain.Repository;
using Domain.Serializer;
using Infrastructure.Logging;
using Infrastructure.Repository;
using Infrastructure.Serializer;
using Ninject.Modules;
using Vault.Integration.Msmq.Connector;

namespace Vault.Integration.MessageProcessor.Host
{
    public class Bindings : NinjectModule
    {
        public override void Load()
        {
            Bind<IRunner>().To<Runner>();
            Bind<IMessageProcessor>().To<MessageProcessor>();
            Bind<IMsmqConnector>().To<MsmqConnector>();
            Bind<ILookup>().To<Lookup>();
            Bind<IRepository>().To<Repository>();
            Bind<IAsyncRepository>().To<AsyncRepository>();
            Bind<ISerializer>().To<XmlSerializer>();
            Bind<ILogger>().To<Logger>();
        }
    }
}