using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DistanceLessons.Models;

namespace DistanceLessons.Controllers
{
    public class VirtualJournalController : Controller
    {
        //
        // GET: /VirtualJjournal/

        public ActionResult Index()
        {

            
            DataEntitiesManager dataMeneger = new DataEntitiesManager();
            List<DataEntitiesManager.RQUserModules> lst = new List<DataEntitiesManager.RQUserModules>();

            var moduleLst = dataMeneger.GetUserProgress();

            foreach (DataEntitiesManager.RQUserModules obj in moduleLst)
            {
                lst.Add(obj);
            }

            ViewBag.UserModules = lst;

            return View();

        }
    }

}
