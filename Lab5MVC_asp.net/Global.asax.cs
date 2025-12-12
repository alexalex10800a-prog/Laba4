using BLL2;
using Lab5MVC_asp.net.Util;
using Ninject;
using Ninject.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
namespace Lab5MVC_asp.net
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            //dependensies
            string connectionString = "data source=localhost;initial catalog=new;integrated security=True;encrypt=False;MultipleActiveResultSets=True;App=EntityFramework";

            var kernel = new StandardKernel(new NinjectRegistrations(), new ServiceModule(connectionString));
            DependencyResolver.SetResolver(new NinjectDependencyResolver(kernel));
        }
    }
}
