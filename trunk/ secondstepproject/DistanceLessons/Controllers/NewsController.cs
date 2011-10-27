using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DistanceLessons.Models;

namespace DistanceLessons.Controllers
{
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
                return View(_db.GetNewsList_time(itemOnPage, numPage));
            }
            else
            {
                return PartialView("_New_PartialPage", _db.GetNewsList_time(itemOnPage, numPage));
            }
        }

        [HttpGet]
        public ActionResult CreateNew()
        {
            if (!Request.IsAjaxRequest())
            {
                return View();
            }
            else
            {
                return PartialView("_CreateNew_PartialPage");
            }
        }

        [HttpPost]
        public ActionResult CreateNew(News obj)
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
        public ActionResult EditNew(Guid id)
        {
            if (!Request.IsAjaxRequest())
            {
                return View(_db.GetNew(id));
            }
            else
            {
                return PartialView("_EditNew_PartialPage", _db.GetNew(id));
            }
        }

        [HttpPost]
        public ActionResult EditNew(News obj)
        {
            News old = _db.GetNew(obj.NewId);
            UpdateModel(old);
            _db.Save();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult DeleteNew(Guid id)
        {
            _db.DeleteNew(id);
            return RedirectToAction("Index");
        }
    }
}
