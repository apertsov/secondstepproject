using System;
using System.Web.Mvc;
using DistanceLessons.Attributes;
using DistanceLessons.Models;

namespace DistanceLessons.Controllers
{
    [Localization]
    public class MessageController : Controller
    {
        //
        // GET: /Message/

        private DataEntitiesManager _db = new DataEntitiesManager(); 

        public ActionResult Index()
        {
            DataEntitiesManager dm = new DataEntitiesManager();

            ViewBag.Users = _db.GetUserList();

            return View(dm.GetMessageList_UserID(_db.GetUser(_db.GetUserId(User.Identity.Name)).Login, 0));
        }

        public ActionResult Users()
        {
            return View(_db.GetUserList());
        }

        public ActionResult SendList()
        {
            DataEntitiesManager dm = new DataEntitiesManager();

            ViewBag.Users = _db.GetUserList();

            return View(dm.GetMessageList_UserID(_db.GetUser(_db.GetUserId(User.Identity.Name)).Login, 1));
        }

        //
        // GET: /Message/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }

        
        [HttpGet]
        public ActionResult Create(Guid to_Id)
        {
            ViewBag.toId = to_Id;
            Message msg = new Message();
            msg.DateOfSender = System.DateTime.Now;
            msg.UserId_To = to_Id;
            return View(msg);

            
        } 

        

        [HttpPost]
        public ActionResult Create(Message obj, Guid to_Id)
        {
            obj.MessageId = Guid.NewGuid();
            obj.DateOfSender = System.DateTime.Now;
            obj.UserId_From = _db.GetUser(User.Identity.Name).UserId;
            obj.UserId_To = to_Id;
            
            _db.AddMessage(obj);
            _db.Save();
            
            return RedirectToAction("Index");
        }
        
        
 
        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /Message/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here
 
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

    [HttpGet]
        public ActionResult Delete(Guid id)
        {
            _db.DeleteMessage(id);
            return RedirectToAction("Index");
        }

        
    }
}
