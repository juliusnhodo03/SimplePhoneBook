

using Application.Modules.Common;
using Domain.Logging;
using Domain.Repository;
using Hyphen.Integration.Reconciliation.FileProcessor;
using Infrastructure.Logging;
using Infrastructure.Repository;
using Ninject.Modules;

namespace Hyphen.Integration.Reconciliation.Host
{
    public class Bindings : NinjectModule
    {
        public override void Load()
        {
            Bind<ILookup>().To<Lookup>();
            Bind<IRepository>().To<Repository>();
            Bind<IAsyncRepository>().To<AsyncRepository>();
            Bind<IFileManager>().To<FileManager>();
            Bind<ILogger>().To<Logger>();              
        }
    }
}
