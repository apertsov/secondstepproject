using System;
using System.Web.Mvc;
using DistanceLessons.Attributes;
using DistanceLessons.Models;

namespace DistanceLessons.Controllers
{
    [Localization]
    [Authorize]
    public class NewsController : Controller
    {
        private readonly DataEntitiesManager _db = new DataEntitiesManager();
        private int itemOnPage = 5;

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
                obj.Publication = DateTime.Now;
                obj.UserId = _db.GetUserId(User.Identity.Name);

                _db.AddNew(obj);
                _db.Save();
            }
            catch
            {
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Edit(Guid? id)
        {
            if ((id == null) || (!_db.ExistNew((Guid)id)))
            {
                return new NotFoundMvc.NotFoundViewResult();
            }
            if (!Request.IsAjaxRequest())
            {
                return View(_db.GetNew((Guid)id));
            }
            else
            {
                return PartialView("_Edit_PartialPage", _db.GetNew((Guid)id));
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
        public ActionResult Delete(Guid? id)
        {
            if ((id == null) || (!_db.ExistNew((Guid)id)))
            {
                return new NotFoundMvc.NotFoundViewResult();
            }
            if (!Request.IsAjaxRequest())
            {
                return View(_db.GetNew_withTag((Guid)id));
            }
            else
            {
                return PartialView("_Delete_PartialPage", _db.GetNew_withTag((Guid)id));
            }
        }

        [HttpPost]
        public ActionResult Delete(Guid? id, FormCollection collection)
        {
            if ((id == null) || (!_db.ExistNew((Guid)id))||(!User.IsInRole("Student")))
            {
                return new NotFoundMvc.NotFoundViewResult();
            }
            _db.DeleteNew((Guid)id);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Detail(Guid id)
        {
            if ((id == null) || (!_db.ExistNew((Guid)id)))
            {
                return new NotFoundMvc.NotFoundViewResult();
            }
            if (!Request.IsAjaxRequest())
            {
                return View(_db.GetNew_withTag(id));
            }
            else
            {
                return PartialView("_Detail_PartialPage", _db.GetNew_withTag(id));
            }
        }
    }
}