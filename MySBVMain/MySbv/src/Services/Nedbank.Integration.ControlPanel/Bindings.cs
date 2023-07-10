using Application.Modules.Common;
using Domain.Notifications;
using Domain.Repository;
using Domain.Security;
using Infrastructure.Notifications;
using Infrastructure.Repository;
using Infrastructure.Security;
using Nedbank.Integration.FileCreator;
using Nedbank.Integration.FileManager;
using Nedbank.Integration.FileUtilities;
using Nedbank.Integration.Request.Generator;
using Nedbank.Integration.Response.Reader;
using Nedbank.Integration.SettlementHub;
using Ninject.Modules;
using Vault.Integration.Msmq.Connector;
using Vault.Integration.DataContracts;

namespace Nedbank.Integration.ControlPanel
{
    public class Bindings : NinjectModule
    {
        public override void Load()
        {
            Bind<ILookup>().To<Lookup>();
            Bind<IAsyncRepository>().To<AsyncRepository>();
            Bind<IRepository>().To<Repository>();
            Bind<IFileCreator>().To<FileCreator.FileCreator>();
            Bind<IResponseReader>().To<ResponseReader>();
            Bind<IFileManager>().To<FileManager.FileManager>();
            Bind<INotification>().To<Notification>();
            Bind<IRequestWriter>().To<RequestWriter>();
            Bind<IFileUtility>().To<FileUtility>();
            Bind<IMsmqConnector>().To<MsmqConnector>();
            Bind<ISettlement>().To<Settlement>();
            Bind<ISecurity>().To<Security>();
        }
    }
}