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
        //
        // GET: /Profile/

        [HttpGet]
        public ActionResult Index()
        {
            DataEntitiesManager _db = new DataEntitiesManager();
            ViewBag.Mes = _db.GetMessageList_UserID(User.Identity.Name, 0);
            return View();
        }

    }
}
