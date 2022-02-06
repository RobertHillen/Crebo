using Crebo.Base.Helpers;
using Crebo.Base.Interfaces;
using SimpleInjector;
using SimpleInjector.Integration.Web.Mvc;
using System.Linq;
using System.Web.Mvc;

namespace Crebo
{
    public static class DepencencyConfig
    {
        public static void RegisterDependencyContainer(Container container)
        {
            var assemblies = ReflectionHelper.GetCreboAssemblies().ToArray();

            container.Register(typeof(IQueryHandler<,>), assemblies, Lifestyle.Scoped);

            DependencyResolver.SetResolver(new SimpleInjectorDependencyResolver(container));
        }
    }
}