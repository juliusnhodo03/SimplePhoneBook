using Application.Modules.Common;
using Domain.Notifications;
using Domain.Repository;
using Hyphen.Integration.Facs;
using Hyphen.Integration.FileManager;
using Hyphen.Integration.Request.Data;
using Hyphen.Integration.Response.Data;
using Infrastructure.Notifications;
using Infrastructure.Repository;
using Ninject.Modules;
using Vault.Integration.Msmq.Connector;
using Vault.Integration.DataContracts;

namespace Hyphen.Integration.Manager
{
    public class Bindings : NinjectModule
    {
        public override void Load()
        {
            Bind<ILookup>().To<Lookup>();
            Bind<IAsyncRepository>().To<AsyncRepository>();
            Bind<IRepository>().To<Repository>();
            Bind<IFacs>().To<Facs.Facs>();
            Bind<IResponseData>().To<ResponseData>();
            Bind<IFileManager>().To<FileManager.FileManager>();
            Bind<INotification>().To<Notification>();
            Bind<IRequestData>().To<RequestData>();
            Bind<IMsmqConnector>().To<MsmqConnector>();            
        }
    }
}