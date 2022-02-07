using Crebo.Base.Helpers;
using Crebo.Base.Interfaces;
using SimpleInjector;
using SimpleInjector.Integration.Web.Mvc;
using System.Linq;
using System.Web.Mvc;
using Crebo.Business.SimpleInjector;
using SimpleInjector.Integration.Wcf;

namespace Crebo
{
    public static class DepencencyConfig
    {
        public static void RegisterDependencyContainer(Container container)
        {
            container.RegisterQueryHandling();

            DependencyResolver.SetResolver(new SimpleInjectorDependencyResolver(container));
            SimpleInjectorServiceHostFactory.SetContainer(container);
        }
    }
}