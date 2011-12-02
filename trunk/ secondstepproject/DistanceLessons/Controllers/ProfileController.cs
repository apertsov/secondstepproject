﻿using System;
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

        public ActionResult Index()
        {
            if (!Request.IsAjaxRequest())
            {
                return View(_db.GetUserProfile(_db.GetUserId(User.Identity.Name)));
            }
            else
            {
                return PartialView("_Info_PartialPage", _db.GetUserProfile(_db.GetUserId(User.Identity.Name)));
            }
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
        public ActionResult ContactInformation()
        {
            if (_db.ExistContact(User.Identity.Name))
            {
                return PartialView("_EditContactInfo_PartialPage", _db.GetUserContact(_db.GetUserId(User.Identity.Name)));
            }
            else
            {
                return PartialView("_CreateContactInfo_PartialPage");
            }
        }

        [HttpPost]
        public ActionResult ContactInformation(Contact obj)
        {
            if (_db.ExistContact(User.Identity.Name))
            {
                Contact old = _db.GetContact(obj.ContactId);
                UpdateModel(old);
                _db.Save();
            }
            else
            {
                obj.UserId = _db.GetUserId(User.Identity.Name);
                obj.ContactId = Guid.NewGuid();
                _db.AddContact(obj);
                _db.Save();
            }
            return RedirectToAction("Index");
        }


        [HttpGet]
        public ActionResult Info(string user)
        {
            if ((String.IsNullOrEmpty(user)) || (!_db.ExistUser(user)))
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
                    ViewData["username"] = user;
                    return View("profile", _db.GetUserProfile(_db.GetUserId(user)));
                }
            }
        }

        public ActionResult Messages(byte type)
        {
            Session["choose_type"] = type;
            ViewBag.Exist = false;
            foreach (var item in _db.GetMessagesByUser(User.Identity.Name, type))
            {
                if ((item.Messages.Status == 10) || (item.Messages.Status == 11) || (item.Messages.Status == 12) || (item.Messages.Status == 0) || (item.Messages.Status == 1)) ViewBag.Exist = true;
            }
            return PartialView("_Messages", _db.GetMessagesByUser(User.Identity.Name, type));
        }

        public ActionResult NewMessages()
        {
            return PartialView("_NewMessages", _db.GetNewMessageForUser(User.Identity.Name));
        }

        [HttpGet]
        public ActionResult NewMessage()
        {
            return View();
        }

        [HttpPost]
        public ActionResult NewMessage(SendMessageModel obj)
        {
            if (ModelState.IsValid)
            {
                if (User.Identity.Name.ToLower() != obj.Name.ToLower())
                {
                    obj.Messages.MessageId = Guid.NewGuid();
                    obj.Messages.UserId_From = _db.GetUserId(User.Identity.Name);
                    obj.Messages.UserId_To = _db.GetUserId(obj.Name);
                    obj.Messages.DateOfSender = DateTime.Now;

                    _db.AddMessage(obj.Messages);
                    _db.AddMessage_status(_db.GetMessageStatus(obj.Messages, obj.Name));
                    _db.AddMessage_status(_db.GetMessageStatus(obj.Messages, User.Identity.Name),1);
                }
                return RedirectToAction("Index");
            }
            return View(obj);
        }

        public ActionResult DeleteMessage(Guid MessageId)
        {
            Message_status upd_ = _db.GetMessageStatById(MessageId, _db.GetUserId(User.Identity.Name));
            upd_.Status = 2;
            UpdateModel(upd_);
            _db.Save();
            return PartialView("_Messages", _db.GetMessagesByUser(User.Identity.Name, (byte)Session["choose_type"]));
        }

        public ActionResult ReadMessage(Guid MessageId)
        {
            Message_status upd_ = _db.GetMessageStatById(MessageId, _db.GetUserId(User.Identity.Name));
            if (upd_.Status != 10) upd_.Status = 1;
            UpdateModel(upd_);
            _db.Save();
            return PartialView("_Message", _db.GetMessageStatById(MessageId, _db.GetUserId(User.Identity.Name)));
        }

        public ActionResult ConfirmSubscribe(Guid CourseId, Guid UserId, Guid MessageId)
        {
            UserCours subscribe = new UserCours();
            subscribe.UserCourseId = Guid.NewGuid();
            subscribe.CourseId = CourseId;
            subscribe.UserId = UserId;
            _db.AddUserCourse(subscribe);
            _db.Save();

            Message_status upd_ = _db.GetMessageStatById(MessageId, _db.GetUserId(User.Identity.Name));
            upd_.Status = 11;
            UpdateModel(upd_);
            _db.Save();

            return RedirectToAction("Index");
        }
    }
}
