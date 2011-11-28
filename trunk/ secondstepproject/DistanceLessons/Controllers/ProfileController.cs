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
                if (_db.ExistInformation(User.Identity.Name)) return View(_db.GetUserProfile(_db.GetUserId(User.Identity.Name)));
                else return View();
            }
            else
            {
                if (_db.ExistInformation(User.Identity.Name)) return PartialView("_Info_PartialPage", _db.GetUserProfile(_db.GetUserId(User.Identity.Name)));
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
                    return View("profile", _db.GetUserProfile(_db.GetUserId(user)));
                }
            }
        }

        public ActionResult Messages()
        {
            return PartialView("_Messages",_db.GetMessagesByUser(User.Identity.Name, 0));
        }

        [HttpGet]
        public ActionResult SendMessage()
        {
            return PartialView("_SendMessage");
        }

        [HttpPost]
        public ActionResult SendMessage(SendMessageModel obj)
        {
            obj.Message.MessageId = Guid.NewGuid();
            obj.Message.UserId_From = _db.GetUserId(User.Identity.Name);
            obj.Message.UserId_To = _db.GetUserId(obj.Name);
            obj.Message.DateOfSender = DateTime.Now;

            _db.AddMessage(obj.Message);
            _db.AddMessage_status(_db.GetMessageStatus(obj.Message,obj.Name));
            _db.AddMessage_status(_db.GetMessageStatus(obj.Message,User.Identity.Name));

            return RedirectToAction("Index");
        }
    }
}
