
using Application.Modules.Common;
using Domain.Notifications;
using Domain.Repository;
using Domain.Security;
using Hyphen.Integration.Reconciliation.FileProcessor;
using Infrastructure.Notifications;
using Infrastructure.Repository;
using Infrastructure.Security;
using Ninject.Modules;

namespace Hyphen.Integration.Reconciliation.Controller
{
    public class Bindings : NinjectModule
    {
        public override void Load()
        {
            Bind<ILookup>().To<Lookup>();
            Bind<IAsyncRepository>().To<AsyncRepository>();
            Bind<IRepository>().To<Repository>();
            Bind<IFileManager>().To<FileManager>();
            Bind<INotification>().To<Notification>();
            Bind<ISecurity>().To<Security>();
        }
    }
}