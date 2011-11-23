﻿using System;
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

            string logIn = Request.Form["Login"];

            if (_db.GetUser(User.Identity.Name).Login == logIn)
            {
                ViewBag.ErrorUser = 1;
            }
            else 
            { 
                ViewBag.ErrorUser = 0;
            }

           // if(n == 0) ViewBag.ErrorUser = 0;
           // if(n == 1) ViewBag.ErrorUser = 1;
           // if(n == 2) ViewBag.ErrorUser = 2;
            
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
                    obj.UserId_From = _db.GetUser(User.Identity.Name).UserId;
                    obj.UserId_To = _db.GetUserId(logIn);

                    if (obj.UserId_From != obj.UserId_To)
                    {
                        obj.MessageId = Guid.NewGuid();
                        obj.DateOfSender = System.DateTime.Now;
                        obj.Status = 1;
                        _db.AddMessage(obj);
                        _db.Save();

                        return RedirectToAction("Index");
                    }
                    else
                    {
                        //ViewBag.ErrorUser = 1;
                        TempData["Message"] = "Ви знаходитесь під цим логіном";
                        return RedirectToAction("Create");
                    }
                    // break;
                }
                              
            }

            //ViewBag.ErrorUser = 0;
            TempData["Message"] = "Користувач з таким логіном відсутній";
            return RedirectToAction("Create"); 
                        
        }

        [HttpGet]
        public ActionResult CreateMessage(string user)
        {
            Message msg = new Message();
            msg.DateOfSender = System.DateTime.Now;
            msg.UserId_To = _db.GetUserId(user);
            msg.User1 = _db.GetUser(user);
            return View(msg);

        }

        [HttpPost]
        public ActionResult CreateMessage(Message obj)
        {

            obj.UserId_From = _db.GetUser(User.Identity.Name).UserId;
            //string logIn = Request.Form["Login"];

            //obj.UserId_To = obj.User1.UserId; //_db.GetUserId(logIn);
            obj.MessageId = Guid.NewGuid();
            obj.DateOfSender = System.DateTime.Now;
            //  obj.Status = 1;
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
        public ActionResult Delete(Guid id, byte status)
        {
            
            _db.DeleteMessage(id);
            return RedirectToAction("Index");
        }

        
    }
}
