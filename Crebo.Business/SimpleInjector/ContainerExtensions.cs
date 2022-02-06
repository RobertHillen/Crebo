using System.ComponentModel;

namespace Crebo.Business.SimpleInjector
{
    public static class ContainerExtensions
    {
        public static void RegisterQueryHandling(this Container container)
        {
            container.RegisterQueryHandling();
        }
    }
}
