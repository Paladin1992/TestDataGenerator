using Autofac;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
using AutoMapper;
using LiteDB;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Serilog;
using System;
using System.Reflection;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using TestDataGenerator.Services;
using TestDataGenerator.Web.App_Start;
using TestDataGenerator.Web.Controllers;

namespace TestDataGenerator.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            var builder = new ContainerBuilder();
            builder.RegisterType<DataService>().As<IDataService>().InstancePerDependency();
            builder.RegisterType<AccountService>().As<IAccountService>().InstancePerDependency();
            builder.RegisterType<EmailService>().As<IEmailService>().InstancePerDependency();
            builder.RegisterType<SetupService>().As<ISetupService>().InstancePerDependency();
            builder.RegisterInstance(new DataGeneratorService());
            builder.RegisterInstance(new LiteRepository(@"C:\Temp\TDG.db"));

            var mapper = AutoMapperConfig.Configure();
            builder.RegisterInstance(mapper).As<IMapper>();

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.AppSettings()
                .CreateLogger();

            // Get your HttpConfiguration.
            var config = GlobalConfiguration.Configuration;

            // Register your MVC controllers. (MvcApplication is the name of
            // the class in Global.asax.)
            builder.RegisterControllers(typeof(MvcApplication).Assembly);

            // Register your Web API controllers.
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            // OPTIONAL: Register the Autofac filter provider.
            builder.RegisterWebApiFilterProvider(config);

            // OPTIONAL: Register the Autofac model binder provider.
            builder.RegisterWebApiModelBinderProvider();

            // Set the dependency resolver to be Autofac.
            var container = builder.Build();
            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);

            // Set the dependency resolver to be Autofac.
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));


            // JSON
            JsonConvert.DefaultSettings = () => new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                TypeNameHandling = TypeNameHandling.Objects,
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };

            GlobalConfiguration.Configuration.Formatters
                .JsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();

            ViewEngines.Engines.Clear();
            var customEngine = new RazorViewEngine
            {
                PartialViewLocationFormats = new string[]
                {
                    "~/Views/{1}/{0}.cshtml",
                    "~/Views/Shared/{0}.cshtml",
                    "~/Views/Partial/{0}.cshtml",
                    "~/Views/Partial/{1}/{0}.cshtml"
                },

                ViewLocationFormats = new string[]
                {
                    "~/Views/{1}/{0}.cshtml",
                    "~/Views/Shared/{0}.cshtml"
                },

                MasterLocationFormats = new string[]
                {
                    "~/Views/Shared/{0}.cshtml",
                    "~/Views/Layout/{0}.cshtml"
                }
            };

            ViewEngines.Engines.Add(customEngine);
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            var httpContext = ((MvcApplication)sender).Context;
            var exception = Server.GetLastError();

            if (exception != null)
            {
                Log.Error(exception, string.Empty);
            }

            var controller = new ErrorController();
            var routeData = new RouteData();

            httpContext.ClearError();
            httpContext.Response.Clear();

            routeData.Values["controller"] = "Error";
            routeData.Values["action"] = "Index";

            controller.ViewData.Model = exception;
            ((IController)controller).Execute(new RequestContext(new HttpContextWrapper(httpContext), routeData));
        }
    }
}