using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics.Contracts;
using System.Web.Http;
using System.Web.Http.Dependencies;
using cozyjozywebapi.Entity;
using cozyjozywebapi.Entity.Repositories;
using cozyjozywebapi.Infrastructure;
using cozyjozywebapi.Infrastructure.Core;
using Ninject.Syntax;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(cozyjozywebapi.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(cozyjozywebapi.App_Start.NinjectWebCommon), "Stop")]

namespace cozyjozywebapi.App_Start
{
    using System;
    using System.Web;

    using Microsoft.Web.Infrastructure.DynamicModuleHelper;

    using Ninject;
    using Ninject.Web.Common;

    public static class NinjectWebCommon 
    {
        private static readonly Bootstrapper bootstrapper = new Bootstrapper();

        /// <summary>
        /// Starts the application
        /// </summary>
        public static void Start() 
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            bootstrapper.Initialize(CreateKernel);
        }
        
        /// <summary>
        /// Stops the application.
        /// </summary>
        public static void Stop()
        {
            bootstrapper.ShutDown();
        }
        
        /// <summary>
        /// Creates the kernel that will manage your application.
        /// </summary>
        /// <returns>The created kernel.</returns>
        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            try
            {
                kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
                kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();

                RegisterServices(kernel);
                GlobalConfiguration.Configuration.DependencyResolver = new NinjectDependencyResolver(kernel);

                return kernel;
            }
            catch
            {
                kernel.Dispose();
                throw;
            }
        }

        /// <summary>
        /// Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel)
        {
            kernel.Bind<IUnitOfWork>().To<CozyJozyUnitOfWork>().InRequestScope();
            kernel.Bind<DbContext>().To<CozyJozyContext>().InRequestScope();
            kernel.Bind<IChildRepository>().To<ChildRepository>().InRequestScope();
            kernel.Bind<IFeedingRepository>().To<FeedingRepository>().InRequestScope();
            kernel.Bind<IChildPermissionsRepository>().To<ChildPermissionsRepository>().InRequestScope();
            kernel.Bind<IDiaperChangesRepository>().To<DiaperChangesRepository>().InRequestScope();
            kernel.Bind<IRoleRepository>().To<IdentityRoleRepository>().InRequestScope();
            kernel.Bind<IUserRepository>().To<UserRepository>().InRequestScope();
            kernel.Bind<ITitleRepository>().To<TitleRepository>().InRequestScope();
            kernel.Bind<ISleepRepository>().To<SleepRepository>().InRequestScope();
        }        
    }

    public class NinjectDependencyScope : IDependencyScope
    {
        private IResolutionRoot resolver;

        internal NinjectDependencyScope(IResolutionRoot resolver)
        {
            Contract.Assert(resolver != null);
            this.resolver = resolver;
        }

        public void Dispose()
        {
            IDisposable disposable = resolver as IDisposable;
            if (disposable != null)
                disposable.Dispose();
            resolver = null;
        }

        public object GetService(Type serviceType)
        {
            if (resolver == null)
                throw new ObjectDisposedException("this", "This scope has already been disposed");

            return resolver.TryGet(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            if (resolver == null)
                throw new ObjectDisposedException("this", "This scope has already been disposed");

            return resolver.GetAll(serviceType);
        }
    }

    public class NinjectDependencyResolver : NinjectDependencyScope, IDependencyResolver
    {
        private IKernel kernel;
        public NinjectDependencyResolver(IKernel kernel)
            : base(kernel)
        {
            this.kernel = kernel;
        }

        public IDependencyScope BeginScope()
        {
            return new NinjectDependencyScope(kernel.BeginBlock());
        }
    } 
}
