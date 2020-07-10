using Autofac;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
using System.Web.Http;
using System.Web.Mvc;

namespace Monolith.Web.App_Start
{
    public static class AutofacConfig
    {
        public static void Register()
        {
            var builder = new ContainerBuilder();
            builder.RegisterControllers(typeof(MvcApplication).Assembly);
            builder.RegisterApiControllers(typeof(MvcApplication).Assembly);

            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
            GlobalConfiguration.Configuration.DependencyResolver = new AutofacWebApiDependencyResolver(container);
        }
    }
}