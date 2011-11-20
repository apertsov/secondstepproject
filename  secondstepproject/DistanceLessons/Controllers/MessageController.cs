using System;
using System.Web.Mvc;
using DistanceLessons.Attributes;
using DistanceLessons.Models;
using System.Collections.Generic;

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
        public ActionResult Create()
        {
            Message msg = new Message();
            msg.DateOfSender = System.DateTime.Now;
            
            return View(msg);

            
        } 

        
        [HttpPost]
        public ActionResult Create(Message obj)
        {
            string logIn = Request.Form["Login"];

            List<User> userList= _db.GetUserList();

            foreach (User userObj in userList)
            {
                if (userObj.Login == logIn)
                {
                    obj.MessageId = Guid.NewGuid();
                    obj.DateOfSender = System.DateTime.Now;
                    obj.UserId_From = _db.GetUser(User.Identity.Name).UserId;

                    obj.UserId_To = _db.GetUserId(logIn);
                    _db.AddMessage(obj);
                    _db.Save();

                    return RedirectToAction("Index");

                    break;
                }
                
 
                
            }

            return RedirectToAction("Create"); 
            
            
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
