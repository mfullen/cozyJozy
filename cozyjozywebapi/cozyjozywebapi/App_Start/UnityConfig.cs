using System.Data.Entity;
using cozyjozywebapi.Entity;
using cozyjozywebapi.Entity.Repositories;
using cozyjozywebapi.Infrastructure;
using cozyjozywebapi.Infrastructure.Core;
using cozyjozywebapi.Models;
using cozyjozywebapi.Services;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Practices.Unity;
using System.Web.Http;
using Unity.WebApi;

namespace cozyjozywebapi
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();
            
            // register all your components with the container here
            // it is NOT necessary to register your controllers
            
            // e.g. container.RegisterType<ITestService, TestService>();

            container.RegisterType<IUnitOfWork, CozyJozyUnitOfWork>();
            container.RegisterType<DbContext, CozyJozyContext>(new PerResolveLifetimeManager());
            container.RegisterType<IChildRepository, ChildRepository>();
            container.RegisterType<IFeedingRepository, FeedingRepository>();
            container.RegisterType<IChildPermissionsRepository, ChildPermissionsRepository>();
            container.RegisterType<IDiaperChangesRepository, DiaperChangesRepository>();
            container.RegisterType<IRoleRepository, IdentityRoleRepository>();
            container.RegisterType<IUserRepository, UserRepository>();
            container.RegisterType<ITitleRepository, TitleRepository>();
            container.RegisterType<ISleepRepository, SleepRepository>();
            container.RegisterType<IUserService, UserService>();
            container.RegisterType<IEmailService, EmailService>();
            container.RegisterType<ITokenRepository, InMemoryTokenRepository>(new ContainerControlledLifetimeManager());

            container.RegisterType<IUserStore<User>, UserStore<User>>(new HierarchicalLifetimeManager());
      
            GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container);
        }
    }
}