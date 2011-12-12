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
        private readonly DataEntitiesManager _db = new DataEntitiesManager();
        private string chooseLang = "uk-ua";
        private HttpCookie university_cookies;

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Courses()
        {
            return View("Courses", _db.GetValidCourses());
        }

        private void ReplaceCookie(string cookie, string culture)
        {
            Response.Cookies[cookie].Value = null;
            Response.Cookies[cookie].Expires = DateTime.Now.AddDays(-7);

            university_cookies = new HttpCookie(cookie);
            university_cookies.Value = culture;
            university_cookies.Expires = DateTime.Now.AddDays(7);
            Response.Cookies.Add(university_cookies);
        }

        public ActionResult English()
        {
            Session["culture"] = "en-us";
            chooseLang = Session["culture"].ToString();
            ReplaceCookie("language", chooseLang);

            return RedirectToAction("Index");
        }

        public ActionResult Ukraine()
        {
            Session["culture"] = "uk-ua";
            chooseLang = Session["culture"].ToString();
            ReplaceCookie("language", chooseLang);

            return RedirectToAction("Index");
        }

        public ActionResult Russian()
        {
            Session["culture"] = "ru-ru";
            chooseLang = Session["culture"].ToString();
            ReplaceCookie("language", chooseLang);

            return RedirectToAction("Index");
        }

        [Authorize]
        public ActionResult MessageBox()
        {
            Session["MessageCount"] = (_db.GetNewMessageForUser(User.Identity.Name).Count != 0)
                                          ? _db.GetNewMessageForUser(User.Identity.Name).Count
                                          : 0;
            return PartialView("_NewMessageBox");
        }
    }
}