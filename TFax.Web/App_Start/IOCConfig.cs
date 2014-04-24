using System.Data.Entity;
using System.Web.Mvc;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.StaticFactory;

using TFax.Web.CORE.BLL.Infrastructure;
using TFax.Web.CORE.BLL.Security;
using TFax.Web.CORE.COMMON.Helpers;
using TFax.Web.CORE.DAL;

using Unity.Mvc4;

namespace TFax.Web
{
    public static class IOCConfig
    {
        public static IUnityContainer Initialize()
        {
            var container = BuildUnityContainer();

            DependencyResolver.SetResolver(new UnityDependencyResolver(container));

            return container;
        }

        private static IUnityContainer BuildUnityContainer()
        {
            var container = new UnityContainer();

            // register all your components with the container here
            // it is NOT necessary to register your controllers

            // e.g. container.RegisterType<ITestService, TestService>();    
            RegisterTypes(container);

            return container;
        }

        private static void RegisterTypes(IUnityContainer container)
        {

            //db context
            container.RegisterType<DbContext, TFaxEntities>(new TransientLifetimeManager());

            //unit of work
            container.RegisterType<IUnitOfWork, UnitOfWork>(new TransientLifetimeManager());

            //membership
            container.RegisterType<IMembershipHelper, MembershipHelper>(new TransientLifetimeManager());
             
        }
    }
}