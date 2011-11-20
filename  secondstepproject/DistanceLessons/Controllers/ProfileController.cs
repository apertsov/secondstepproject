using System;
using System.Web.Mvc;
using DistanceLessons.Attributes;
using DistanceLessons.Models;
using NotFoundMvc;

namespace DistanceLessons.Controllers
{
    [Localization]
    public class ProfileController : Controller
    {
        private DataEntitiesManager _db = new DataEntitiesManager();

        //
        // GET: /Profile/

        [HttpGet]
        public ActionResult Index()
        {
            ViewData["exist"] = false;
            if (_db.ExistInformation(User.Identity.Name)) ViewData["exist"] = true;

            if (!Request.IsAjaxRequest())
                {
                    if (_db.ExistInformation(User.Identity.Name)) return View(_db.UserInformation(User.Identity.Name));
                    else return View();
                }
                else
                {
                    if (_db.ExistInformation(User.Identity.Name)) return PartialView("_Info_PartialPage", _db.UserInformation(User.Identity.Name));
                    else return PartialView("_CreateInfo_PartialPage");
                }
        }

        [HttpPost]
        public ActionResult Index(Information obj)
        {
            if (!_db.ExistInformation(User.Identity.Name))
            {
                obj.UserId = _db.GetUserId(User.Identity.Name);
                obj.InformationId = Guid.NewGuid();
                _db.AddInformation(obj);
                _db.Save();
            }
            return RedirectToAction("Index");
        }


        [HttpGet]
        public ActionResult Information()
        {
            if (_db.ExistInformation(User.Identity.Name))
            {
                return PartialView("_EditInfo_PartialPage", _db.UserInformation(_db.GetUserId(User.Identity.Name)));
            }
            else
            {
                return PartialView("_CreateInfo_PartialPage");
            }
        }

        [HttpPost]
        public ActionResult Information(Information obj)
        {
            if (_db.ExistInformation(User.Identity.Name))
            {
                Information old = _db.GetInformation(obj.InformationId);
                UpdateModel(old);
                _db.Save();
            }
            else
            {
                obj.UserId = _db.GetUserId(User.Identity.Name);
                obj.InformationId = Guid.NewGuid();
                _db.AddInformation(obj);
                _db.Save();
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Info(string user)
        {
            if ((String.IsNullOrEmpty(user)) || (!_db.ExistInformation(user)))
            {
                return new NotFoundViewResult();
            }
            else
            {
                if (User.Identity.Name == user)
                {
                    return RedirectToAction("Index", "Profile");
                }
                else
                {
                    return View("profile", _db.UserInformation(user));
                }
            }
        }
    }
}
