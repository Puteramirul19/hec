using Autofac;
using Autofac.Integration.WebApi;
using Autofac.Integration.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Hec.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Hangfire;
using Microsoft.Extensions.Configuration;
using Autofac.Configuration;
using System.Threading;
using System.Globalization;

namespace Hec.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        public static bool IsDemo
        {
            get
            {
                var isDemoCfg = System.Configuration.ConfigurationManager.AppSettings["IsDemo"];
                return (isDemoCfg != null && isDemoCfg.ToLower() == "true");
            }
        }

        protected void Application_BeginRequest()
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("en-GB");
        }

        protected void Application_AuthenticateRequest()
        {
            if (Request.Cookies.AllKeys.Contains("PublicUser") && Context.User == null)
            {
                Response.Cookies["PublicUser"].Expires = DateTime.Now.AddDays(-1);
                Response.Redirect("/Session/End?t=1", true);
            }
        }

        protected void Application_Start()
        {
            // Register Telerik Reporting Web API (must register before MVC routes)
            Telerik.Reporting.Services.WebApi.ReportsControllerConfiguration.RegisterRoutes(System.Web.Http.GlobalConfiguration.Configuration);
            
            AreaRegistration.RegisterAllAreas();
            //System.Web.Http.GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            
            SetupIoC();

            HangfireBootstrapper.Instance.Start();

            //
            //  Replace default MVC's JSON serialization/deserialization with Newtonsoft Json.NET
            //  See http://labs.bjfocus.co.uk/2014/06/using-json-net-as-default-json-serializer-in-mvc/
            //      http://www.dalsoft.co.uk/blog/index.php/2012/01/10/asp-net-mvc-3-improved-jsonvalueproviderfactory-using-json-net/
            //
            GlobalFilters.Filters.Add(new JsonNetActionFilter());
            ValueProviderFactories.Factories.Remove(ValueProviderFactories.Factories.OfType<JsonValueProviderFactory>().FirstOrDefault());
            ValueProviderFactories.Factories.Add(new JsonNetValueProviderFactory());


            //
            // Remove "X-AspNetMvc-Version" in HTTP Header
            //
            MvcHandler.DisableMvcResponseHeader = true;

            //
            // Initialize file store
            //
            Container.Resolve<FileStorage.IFileStore>().Initialize();
        }

        protected void Application_End()
        {
            HangfireBootstrapper.Instance.Stop();
        }

        public static IContainer Container { get; set; }

        private void SetupIoC()
        {
            var builder = new ContainerBuilder();

            //
            // Setup Autofac integration 
            //  http://docs.autofac.org/en/latest/integration/mvc.html
            //  http://docs.autofac.org/en/latest/integration/webapi.html 
            //
            builder.RegisterControllers(typeof(MvcApplication).Assembly);
            builder.RegisterApiControllers(System.Reflection.Assembly.GetExecutingAssembly());

            //
            // Registration
            //
            builder.RegisterType<HecContext>()
                .As<HecContext>()
                .As<System.Data.Entity.DbContext>();

            builder.RegisterType<BackgroundJobClient>()
                .As<IBackgroundJobClient>();

            builder.Register(x => System.Web.HttpContext.Current.GetOwinContext().Authentication)
                .As<IAuthenticationManager>();

            builder.RegisterType<Hec.Web.ApplicationUserManager>()
                .As<Hec.Web.ApplicationUserManager>()
                .As<UserManager<User>>();

            builder.RegisterType<UserStore<User>>()
                .As<IUserStore<User>>();

            builder.RegisterType<Hec.Web.ApplicationSignInManager>()
                .As<Hec.Web.ApplicationSignInManager>()
                .As<SignInManager<User, string>>();

            //
            // Read registration from web.config
            //
            //builder.RegisterModule(new Autofac.Configuration.ConfigurationSettingsReader("autofac"));

            // Add the configuration to the ConfigurationBuilder.
            var config = new ConfigurationBuilder();
            config.AddJsonFile("autofac.json");

            // Register the ConfigurationModule with Autofac.
            var module = new ConfigurationModule(config.Build());
            builder.RegisterModule(module);

            //
            // Build the container
            //
            Container = builder.Build();

            //
            // Set the dependency resolver to be Autofac.
            //
            DependencyResolver.SetResolver(new AutofacDependencyResolver(Container));
            //System.Web.Http.GlobalConfiguration.Configuration.DependencyResolver = new AutofacWebApiDependencyResolver(Container);
        }
    }
}
