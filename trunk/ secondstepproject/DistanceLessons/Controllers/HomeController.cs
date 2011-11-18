using System;
using System.Web;
using System.Web.Mvc;
using DistanceLessons.Attributes;
using DistanceLessons.Models;

namespace DistanceLessons.Controllers
{
    [Localization]
    public class HomeController : Controller
    {
        private HttpCookie university_cookies;
        private string choseLang = "uk-ua";

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

        private void ReplaceCookie(string cookie, string culture)
        {
            Response.Cookies[cookie].Value = null;
            Response.Cookies[cookie].Expires = DateTime.Now.AddMonths(-1);

            university_cookies = new HttpCookie(cookie);
            university_cookies.Value = culture;
            university_cookies.Expires = DateTime.Now.AddMonths(1);
            Response.Cookies.Add(university_cookies);
        }

        public ActionResult English()
        {
            Session["culture"] = "en-us";
            choseLang = Session["culture"].ToString();
            ReplaceCookie("University_Lang", choseLang);

            return RedirectToAction("Index");
        }

        public ActionResult Ukraine()
        {
            Session["culture"] = "uk-ua";
            choseLang = Session["culture"].ToString();
            ReplaceCookie("University_Lang", choseLang);

            return RedirectToAction("Index");
        }

        public ActionResult Russian()
        {
            Session["culture"] = "ru-ru";
            choseLang = Session["culture"].ToString();
            ReplaceCookie("University_Lang", choseLang);

            return RedirectToAction("Index");
        }
    }
}
