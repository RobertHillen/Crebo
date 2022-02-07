using Crebo.Base.Helpers;
using Crebo.Base.Interfaces;
using SimpleInjector;
using System.Linq;

namespace Crebo.Business.SimpleInjector
{
    public static class ContainerExtensions
    {
        public static void RegisterQueryHandling(this Container container)
        {
            var assemblies = ReflectionHelper.GetCreboAssemblies().ToArray();

            container.Register(typeof(IQueryHandler<,>), assemblies, Lifestyle.Scoped);
        }
    }
}
