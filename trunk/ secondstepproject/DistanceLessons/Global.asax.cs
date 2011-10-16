using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using DistanceLessons.Models;

namespace DistanceLessons
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
                "Activate",
                "Account/Activate/{username}/{key}",
                new
                {
                  controller = "Account",
                  action = "Activate",
                  username = UrlParameter.Optional,
                  key = UrlParameter.Optional
                 });
            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );

        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);

            MyRoleProvider roleProv = new MyRoleProvider();
            if (!roleProv.RoleExists(UserRoles.Student.ToString()))
                roleProv.CreateRole(UserRoles.Student.ToString());
            if (!roleProv.RoleExists(UserRoles.Teacher.ToString()))
                roleProv.CreateRole(UserRoles.Teacher.ToString());
            if (!roleProv.RoleExists(UserRoles.Dean.ToString()))
                roleProv.CreateRole(UserRoles.Dean.ToString());
            if (!roleProv.RoleExists(UserRoles.Admin.ToString()))
                roleProv.CreateRole(UserRoles.Admin.ToString());
        }
    }
}