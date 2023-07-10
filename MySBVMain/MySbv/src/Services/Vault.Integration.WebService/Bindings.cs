using Application.Modules.CashHandling.CashProcessing.VaultProcessor;
using Application.Modules.Common;
using Domain.Logging;
using Domain.Notifications;
using Domain.Repository;
using Domain.Serializer;
using Infrastructure.Logging;
using Infrastructure.Notifications;
using Infrastructure.Repository;
using Infrastructure.Serializer;
using Ninject.Modules;
using Vault.Integration.MessageController;
using Vault.Integration.MessageValidator;
using Vault.Integration.Msmq.Connector;
using Vault.Integration.Request.Data;

namespace Vault.Integration.WebService
{
    public class Bindings : NinjectModule
    {
        public override void Load()
        {
            Bind<IMsmqConnector>().To<MsmqConnector>();
            Bind<IMessageValidator>().To<MessageValidator.MessageValidator>();
            Bind<ILookup>().To<Lookup>();
            Bind<IRepository>().To<Repository>();
            Bind<IAsyncRepository>().To<AsyncRepository>();
            Bind<ILogger>().To<Logger>();
            Bind<IMessageController>().To<MessageController.MessageController>();
            Bind<IRequest>().To<Request.Data.Request>();
            Bind<ISerializer>().To<XmlSerializer>();
            Bind<ICashDepositVaultProcessingValidation>().To<CashDepositVaultProcessingValidation>();
            Bind<INotification>().To<Notification>();
            Bind<IEmailManager>().To<EmailManager>();              
        }
    }
}