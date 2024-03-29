﻿using System;
using System.Web.Mvc;
using DistanceLessons.Attributes;
using DistanceLessons.Models;
using NotFoundMvc;

namespace DistanceLessons.Controllers
{
    [Localization]
    [Authorize]
    public class ProfileController : Controller
    {
        private DataEntitiesManager _db = new DataEntitiesManager();

        //
        // GET: /Profile/
    [HttpGet]
          [EnableCompression]
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
        [EnableCompression]
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
        [EnableCompression]
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
        [EnableCompression]
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
        [EnableCompression]
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
        [EnableCompression]
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

        [HttpGet]
          [EnableCompression]
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

        [HttpGet]
        [EnableCompression]
        public ActionResult NewMessages()
        {
            return PartialView("_NewMessages", _db.GetNewMessageForUser(User.Identity.Name));
        }

        [HttpGet]
        [EnableCompression]
        public ActionResult NewMessage(String user)
        {
            if ((String.IsNullOrEmpty(user)) || (!_db.ExistUser(user)))
            {
                return View();
            }

            SendMessageModel temp = new SendMessageModel();
            temp.Name = user;
            return View(temp);
        }

        [HttpPost]
        [EnableCompression]
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
                    _db.AddMessage_status(_db.GetMessageStatus(obj.Messages, User.Identity.Name), 1);
                }
                return RedirectToAction("Index");
            }
            return View(obj);
        }

        [HttpGet]
        [EnableCompression]
        public ActionResult DeleteMessage(Guid? MessageId)
        {
            if ((MessageId == null) || (!_db.ExistMessage((Guid)MessageId)))
            {
                return new NotFoundViewResult();
            }
            Message_status upd_ = _db.GetMessageStatById((Guid)MessageId, _db.GetUserId(User.Identity.Name));
            upd_.Status = 2;
            UpdateModel(upd_);
            _db.Save();
            ViewBag.Exist = true;
            return PartialView("_Messages", _db.GetMessagesByUser(User.Identity.Name, (byte)Session["choose_type"]));
        }

        [HttpGet]
        [EnableCompression]
        public ActionResult ReadMessage(Guid? MessageId)
        {
            if ((MessageId == null) || (!_db.ExistMessage((Guid)MessageId)))
            {
                return new NotFoundViewResult();
            }
            Message_status upd_ = _db.GetMessageStatById((Guid)MessageId, _db.GetUserId(User.Identity.Name));
            if (upd_.Status != 10) upd_.Status = 1;
            UpdateModel(upd_);
            _db.Save();
            return PartialView("_Message", _db.GetMessageStatById((Guid)MessageId, _db.GetUserId(User.Identity.Name)));
        }

        [HttpGet]
        [EnableCompression]
        public ActionResult ConfirmSubscribe(Guid? CourId, Guid? SubscribeUser, Guid? MessId)
        {
            if ((CourId == null) || (SubscribeUser == null) || (MessId == null) || (!_db.ExistCourse((Guid)CourId))|| (!_db.ExistUser((Guid)SubscribeUser))|| (!_db.ExistMessage((Guid)MessId)))
            {
                return new NotFoundViewResult();
            }
            UserCours subscribe = new UserCours();
            subscribe.UserCourseId = Guid.NewGuid();
            subscribe.CourseId = (Guid)CourId;
            subscribe.UserId = (Guid)SubscribeUser;
            _db.AddUserCourse(subscribe);
            _db.Save();

            Message_status upd_ = _db.GetMessageStatById((Guid)MessId, _db.GetUserId(User.Identity.Name));
            upd_.UserId = _db.GetUserId(User.Identity.Name);
            upd_.Status = 11;
            UpdateModel(upd_);
            _db.Save();

            Message confirm = new Message();
            confirm.MessageId = Guid.NewGuid();
            confirm.Message1 = "Викладач <a href=profile/info?user=" + User.Identity.Name + ">" + _db.GetUser(User.Identity.Name).Login + "</a> затвердив Вашу підписку на курс - '" + _db.GetCourse((Guid)CourId).Title + "'.Про терміни здачі модуля, та інші новини по предмету, Ви будете оповіщенні особистим повідомленням.<br />&nbsp;---&nbsp;</br>Дякуємо за підписку. Вдалого навчання! ;)";
            confirm.DateOfSender = DateTime.Now;
            confirm.UserId_From = _db.GetUserId(User.Identity.Name);
            confirm.UserId_To = (Guid)SubscribeUser;
            confirm.Title = "Підписка на курс '" + _db.GetCourse((Guid)CourId).Title + "'";

            _db.AddMessage(confirm);
            _db.AddMessage_status(_db.GetMessageStatus(confirm, _db.GetUser(confirm.UserId_To).Login), 0);
            _db.AddMessage_status(_db.GetMessageStatus(confirm, _db.GetUser(confirm.UserId_From).Login), 2);

            return RedirectToAction("Index");
        }

        [HttpGet]
        [EnableCompression]
        public ActionResult CancelSubscribe(Guid? CourId, Guid? SubscribeUser, Guid? MessId)
        {
            if ((CourId == null) || (SubscribeUser == null) || (MessId == null) || (!_db.ExistCourse((Guid)CourId)) || (!_db.ExistUser((Guid)SubscribeUser)) || (!_db.ExistMessage((Guid)MessId)))
            {
                return new NotFoundViewResult();
            }
            Message_status upd_ = _db.GetMessageStatById((Guid)MessId, _db.GetUserId(User.Identity.Name));
            upd_.UserId = _db.GetUserId(User.Identity.Name);
            upd_.Status = 12;
            UpdateModel(upd_);
            _db.Save();

            Message confirm = new Message();
            confirm.MessageId = Guid.NewGuid();
            confirm.Message1 = "Викладач <a href=profile/info?user=" + User.Identity.Name + ">" + _db.GetUser(User.Identity.Name).Login + "</a> відмовив Вам в підписці на курс - '" + _db.GetCourse((Guid)CourId).Title + "'.<br />&nbsp;---&nbsp;</br><i><b>Адміністрація:</b> Вибачте за незручнсті! Спробуйте загальнодоступні курси.</i>";
            confirm.DateOfSender = DateTime.Now;
            confirm.UserId_From = _db.GetUserId(User.Identity.Name);
            confirm.UserId_To = (Guid)SubscribeUser;
            confirm.Title = "Підписка на курс '" + _db.GetCourse((Guid)CourId).Title + "'";

            _db.AddMessage(confirm);
            _db.AddMessage_status(_db.GetMessageStatus(confirm, _db.GetUser(confirm.UserId_To).Login), 0);
            _db.AddMessage_status(_db.GetMessageStatus(confirm, _db.GetUser(confirm.UserId_From).Login), 2);

            return RedirectToAction("Index");
        }
    }
}
