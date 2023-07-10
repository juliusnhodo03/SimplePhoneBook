
using Application.Modules.Common;
using Domain.Logging;
using Domain.Repository;
using Domain.Serializer;
using Infrastructure.Logging;
using Infrastructure.Repository;
using Infrastructure.Serializer;
using Ninject.Modules;
using Vault.Integration.MessageController;
using Vault.Integration.MessageValidator;
using Vault.Integration.Msmq.Connector;
using Vault.Integration.Request.Data;

namespace Vault.Integration.FailedTransactions.MessageProcessor.Host
{
    public class Bindings : NinjectModule
    {
        public override void Load()
        {
            Bind<IRunner>().To<Runner>();
            Bind<IMsmqConnector>().To<MsmqConnector>();
            Bind<ILookup>().To<Lookup>();
            Bind<IRepository>().To<Repository>();
            Bind<IAsyncRepository>().To<AsyncRepository>();
            Bind<ISerializer>().To<XmlSerializer>();
            Bind<IMessageController>().To<MessageController.MessageController>();
            Bind<IFailedMessageProcessorValidation>().To<FailedMessageProcessorValidation>();
            Bind<IMessageValidator>().To<MessageValidator.MessageValidator>();
            Bind<ILogger>().To<Logger>();
            Bind<IRequest>().To<Request.Data.Request>();              
        }
    }
}
