using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DistanceLessons.Models;

namespace DistanceLessons.Controllers
{
    public class ProfileController : Controller
    {
        private DataEntitiesManager _db = new DataEntitiesManager();

        //
        // GET: /Profile/

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult UserInfo()
        {
            if (_db.ExistInformation(User.Identity.Name))
            {
                return PartialView("_Information_PartialPage", _db.UserInformation(_db.GetUserId(User.Identity.Name)));
            }
            else
            {
                return PartialView("_Create_Information_PartialPage");
            }
        }

        [HttpPost]
        public ActionResult UserInfo(Information model)
        {
            if (_db.ExistInformation(User.Identity.Name))
            {
                Information old = _db.GetInformation(model.InformationId);
                UpdateModel(model);
                _db.Save();
                return RedirectToAction("Index");
            }
            else
            {
                User user = _db.GetUser(User.Identity.Name);
                model.UserId = user.UserId;
                model.InformationId = Guid.NewGuid();
                _db.AddInformation(model);
                return RedirectToAction("Index");
            }
        }
    }
}
