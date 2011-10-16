using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DistanceLessons.Models;

namespace DistanceLessons.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Name = User.Identity.Name;
            ViewBag.Student = User.IsInRole("Student");
            ViewBag.Admin = User.IsInRole("Admin");
            DataEntitiesManager A = new DataEntitiesManager();
            ViewBag.A=A;
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your quintessential app description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your quintessential contact page.";

            return View();
        }

       
        public ActionResult Teachers()
        {
            DataEntitiesManager dataManager = new DataEntitiesManager();
            ViewBag.Teachers = dataManager.QTeachers();

            return View();
        }


        public ActionResult Categories()
        {
            DataEntitiesManager dataManager = new DataEntitiesManager();
            ViewBag.Categories = dataManager.QCategorys();

            return View();
        }

        public ActionResult Courses(DataEntitiesManager.RQCategorys cat)
        {
            DataEntitiesManager dataManager = new DataEntitiesManager();
            List<DataEntitiesManager.RQCourses> lst = new List<DataEntitiesManager.RQCourses>();

            var courseLst = dataManager.QCourses();

            foreach (DataEntitiesManager.RQCourses obj in courseLst)
            {
                if (obj.category == cat.title)
                lst.Add(obj);
            }

            ViewBag.Courses = lst;
            return View();
        }
    }
}
