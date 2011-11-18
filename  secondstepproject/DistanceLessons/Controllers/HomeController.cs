using System.Web.Mvc;
using DistanceLessons.Models;

namespace DistanceLessons.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
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

/*
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
