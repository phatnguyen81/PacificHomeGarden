using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using Telerik.Web.Mvc.Infrastructure;
using pCMS.Core;
using pCMS.Core.Caching;
using pCMS.Core.Infrastructure;
using pCMS.Data;
using pCMS.Framework;
using pCMS.Services;
using Autofac;
using Autofac.Core;
using Autofac.Integration.Mvc;
using pCMS.Utils;

namespace pCMS
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional }, // Parameter defaults
                new []{"pCMS.Controllers"}
            );
           
        }

        protected void Session_Start()
        {
            WorkContext.CurrentLanguage = AppSettings.DefaultLanguage;
            SessionManager.FirstVisit = true;
        }
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);

            //ViewEngines.Engines.Add(new CustomViewEngine());
            //WorkContext.CurrentLanguage = AppSettings.DefaultLanguage;

            //Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(AppSettings.DefaultLanguage);
            //Thread.CurrentThread.CurrentUICulture = new CultureInfo(AppSettings.DefaultLanguage);
            //HttpContext.Current.Response.SetCookie(new HttpCookie("lang", AppSettings.DefaultLanguage));

            RegisterUnityContainer();

        }

        private void RegisterUnityContainer()
        {

            var builder = new ContainerBuilder();
            builder.RegisterModule<AutofacWebTypesModule>();
            builder.RegisterControllers(Assembly.GetExecutingAssembly());
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                if(assembly.FullName.Contains("pCMS.Admin"))
                {
                    builder.RegisterControllers(assembly);
                }
            }

            builder.RegisterModelBinders(Assembly.GetExecutingAssembly());

            builder.Register(c => new DalContext()).As<IDalContext>().InstancePerHttpRequest();
            builder.Register(c => new WebHelper(Context)).As<IWebHelper>().InstancePerHttpRequest();
            builder.Register(c => new MemoryCacheManager()).As<ICacheManager>().InstancePerHttpRequest();
            builder.Register(c => new AlbumService(c.Resolve<IDalContext>(), c.Resolve<IPictureService>())).As<IAlbumService>().InstancePerHttpRequest();
            builder.Register(c => new ArticleService(c.Resolve<IDalContext>())).As<IArticleService>().InstancePerHttpRequest();
            builder.Register(c => new CategoryService(c.Resolve<IDalContext>())).As<ICategoryService>().InstancePerHttpRequest();
            builder.Register(c => new ChannelService(c.Resolve<IDalContext>())).As<IChannelService>().InstancePerHttpRequest();
            builder.Register(c => new EventService(c.Resolve<IDalContext>())).As<IEventService>().InstancePerHttpRequest();
            builder.Register(c => new LanguageService(c.Resolve<IDalContext>())).As<ILanguageService>().InstancePerHttpRequest();
            builder.Register(c => new LocalizationService(c.Resolve<IDalContext>(), c.Resolve<ICacheManager>())).As<Services.ILocalizationService>().InstancePerHttpRequest();
            builder.Register(c => new ManufacturerService(c.Resolve<IDalContext>())).As<IManufacturerService>().InstancePerHttpRequest();
            builder.Register(c => new PictureService(c.Resolve<IDalContext>(),c.Resolve<IWebHelper>())).As<IPictureService>().InstancePerHttpRequest();
            builder.Register(c => new DownloadService(c.Resolve<IDalContext>(), c.Resolve<IWebHelper>())).As<IDownloadService>().InstancePerHttpRequest();
            builder.Register(c => new PollService(c.Resolve<IDalContext>())).As<IPollService>().InstancePerHttpRequest();
            builder.Register(c => new ProductAttributeService(c.Resolve<IDalContext>())).As<IProductAttributeService>().InstancePerHttpRequest();
            builder.Register(c => new ProductService(c.Resolve<IDalContext>())).As<IProductService>().InstancePerHttpRequest();
            builder.Register(c => new ProductTypeService(c.Resolve<IDalContext>())).As<IProductTypeService>().InstancePerHttpRequest();
            builder.Register(c => new SettingService(c.Resolve<IDalContext>())).As<ISettingService>().InstancePerHttpRequest();
            builder.Register(c => new PageService(c.Resolve<IDalContext>())).As<IPageService>().InstancePerHttpRequest();
            builder.Register(c => new SearchService()).As<ISearchService>().InstancePerHttpRequest();
            builder.Register(c => new ExportManager(c.Resolve<IUserService>())).As<IExportManager>().InstancePerHttpRequest();

            builder.Register(c => new OrderService(c.Resolve<IDalContext>())).As<IOrderService>().InstancePerHttpRequest();
            builder.Register(c => new UserService(c.Resolve<IDalContext>())).As<IUserService>().InstancePerHttpRequest();
            builder.Register(c => new CollectionService(c.Resolve<IDalContext>())).As<ICollectionService>().InstancePerHttpRequest();
            //builder.Register(c => new TelerikLocalizationServiceFactory(c.Resolve<Services.ILocalizationService>())).As<ILocalizationServiceFactory>().InstancePerHttpRequest();

            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));


            DI.Current.Register(c => DependencyResolver.Current.GetService<IDalContext>());
            DI.Current.Register(c => DependencyResolver.Current.GetService<ICacheManager>());
            DI.Current.Register(c => DependencyResolver.Current.GetService<Services.ILocalizationService>());
            DI.Current.Register<ILocalizationServiceFactory>(c => new TelerikLocalizationServiceFactory(DependencyResolver.Current.GetService<Services.ILocalizationService>())); 

            //builder.RegisterType<TelerikLocalizationServiceFactory>().As<Telerik.Web.Mvc.Infrastructure.ILocalizationServiceFactory>().InstancePerHttpRequest();
            
        }

        protected void Application_Error(Object sender, EventArgs e)
        {
            LogException(Server.GetLastError());
        }

        protected void LogException(Exception exc)
        {
            if (exc == null)
                return;

            try
            {
                var logger = DependencyResolver.Current.GetService<ILogService>();
                logger.Error(exc.Message, exc, WorkContext.UserLoginInfo == null ? null : WorkContext.UserLoginInfo.UserName);
            }
            catch
            {
                //don't throw new exception if occurs
            }
        }
    }
}
