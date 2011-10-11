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

            AccountRoleService roleService = new AccountRoleService(new MyRoleProvider());
            if (!roleService.RoleExists(UserRoles.Student.ToString()))
                roleService.CreateRole(UserRoles.Student.ToString());
            if (!roleService.RoleExists(UserRoles.Teacher.ToString()))
                roleService.CreateRole(UserRoles.Teacher.ToString());
            if (!roleService.RoleExists(UserRoles.Dean.ToString()))
                roleService.CreateRole(UserRoles.Dean.ToString());
            if (!roleService.RoleExists(UserRoles.Admin.ToString()))
                roleService.CreateRole(UserRoles.Admin.ToString());
        }
    }
}