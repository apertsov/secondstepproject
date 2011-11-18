using System;
using System.Web;
using System.Web.Mvc;
using DistanceLessons.Attributes;
using DistanceLessons.Models;

namespace DistanceLessons.Controllers
{
    [Localization]
    public class NewsController : Controller
    {
        private int itemOnPage = 5;
        private DataEntitiesManager _db = new DataEntitiesManager();

        //
        // GET: /News/

        public ActionResult Index(int numPage = 0)
        {
            ViewData["numPage"] = numPage;
            ViewData["itemsCount"] = _db.GetNewsList().Count;
            ViewData["itemOnPage"] = itemOnPage;

            if (!Request.IsAjaxRequest())
            {
                return View(_db.GetNewsList_time(itemOnPage, numPage, 1));
            }
            else
            {
                return PartialView("_New_PartialPage", _db.GetNewsList_time(itemOnPage, numPage, 1));
            }
        }

        [HttpGet]
        public ActionResult Create()
        {
            return PartialView("_Create_PartialPage");
        }

        [HttpPost]
        public ActionResult Create(News obj)
        {
            try
            {
                obj.NewId = Guid.NewGuid();
                obj.Publication = System.DateTime.Now;
                obj.UserId = _db.GetUserId(User.Identity.Name);

                _db.AddNew(obj);
                _db.Save();
            }
            catch { }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Edit(Guid id)
        {
            if (!Request.IsAjaxRequest())
            {
                return View(_db.GetNew(id));
            }
            else
            {
                return PartialView("_Edit_PartialPage", _db.GetNew(id));
            }
        }

        [HttpPost]
        public ActionResult Edit(News obj)
        {
            News old = _db.GetNew(obj.NewId);
            UpdateModel(old);
            _db.Save();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Delete(Guid id)
        {
            if (!Request.IsAjaxRequest())
            {
                return View(_db.GetNew_withTag(id));
            }
            else
            {
                return PartialView("_Delete_PartialPage", _db.GetNew_withTag(id));
            }
        }

        [HttpPost]
        public ActionResult Delete(Guid id, FormCollection collection)
        {
            _db.DeleteNew(id);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Detail(Guid id)
        {
            if (!Request.IsAjaxRequest())
            {
                return View(_db.GetNew_withTag(id));
            }
            else
            {
                return PartialView("_Detail_PartialPage", _db.GetNew_withTag(id));
            }
        }






        private HttpCookie university_cookies;
        private string choseLang = "uk-ua";

        private void LangCookie(string cookie, string culture)
        {
            if (Request.Cookies[cookie] != null)
            {
                if (Request.Cookies[cookie].Value != null)
                    culture = string.Format(Request.Cookies[cookie].Value);
            }

            if (!System.Threading.Thread.CurrentThread.CurrentUICulture.Name.ToLower().StartsWith(culture.ToLower()))
            {
                System.Threading.Thread.CurrentThread.CurrentUICulture =
                    System.Globalization.CultureInfo.CreateSpecificCulture(culture);
                System.Threading.Thread.CurrentThread.CurrentCulture =
                    System.Globalization.CultureInfo.CreateSpecificCulture(culture);
            }
        }

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
