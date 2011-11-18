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
       DataEntitiesManager _db;

        public HomeController()
            : base()
        {
             _db= new DataEntitiesManager();
        }
        public ActionResult Index()
        {
            return View();
        }
        
        public ActionResult Courses()
        {
            return View(_db.GetValidCourses());
        }
 /*     
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
 */
    }
}
