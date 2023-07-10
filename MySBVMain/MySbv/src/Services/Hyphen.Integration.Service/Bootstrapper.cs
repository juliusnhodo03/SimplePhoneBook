using System.ComponentModel.Composition.Hosting;

namespace Hyphen.Integration.Service
{
    public class Bootstrapper
    {
        private static readonly Bootstrapper _instance = new Bootstrapper();
        private static CompositionContainer _container;

        private Bootstrapper()
        {
        }

        public static Bootstrapper Instance
        {
            get { return _instance; }
        }

        public CompositionContainer Container
        {
            get { return _container; }
        }

        public static void SetContainer(CompositionContainer container)
        {
            _container = container;
        }
    }
}