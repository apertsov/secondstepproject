using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using DistanceLessons.Attributes;
using DistanceLessons.Models;

namespace DistanceLessons.Controllers
{
    [Localization]
    [Authorize]
    public class AccountController : Controller
    {
        public AccountController()
            : base()
        {
            if (MyRoles == null) { MyRoles = new MyRoleProvider(); }
        }

        private DataEntitiesManager _db = new DataEntitiesManager();

        public MyRoleProvider MyRoles { get; set; }
        //
        // GET: /Account/LogOn

        [AllowAnonymous]
        public ActionResult LogOn()
        {
            return View();
        }

        //
        // POST: /Account/LogOn

        [AllowAnonymous]
        [HttpPost]
        public ActionResult LogOn(LogOnModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                if (Membership.ValidateUser(model.UserName, model.Password))
                {
                    FormsAuthentication.SetAuthCookie(model.UserName, model.RememberMe);

                    if (Url.IsLocalUrl(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Не правильний логін або пароль.");
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/LogOff

        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();

            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /Account/Register

        [AllowAnonymous]
        public ActionResult Register()
        {
            if (!MyRoles.IsExistUsersInRole(UserRoles.Admin.ToString()))
            {
                return RedirectToAction("AdminSetup");
            }
            return View();//ContextDependentView();
        }

        //
        // POST: /Account/Register

        [AllowAnonymous]
        [HttpPost]
        public ActionResult Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                // Attempt to register the user
                MembershipCreateStatus createStatus;
                Membership.CreateUser(model.UserName, model.Password, model.Email, null, null, false, null, out createStatus);

                if (createStatus == MembershipCreateStatus.Success)
                {
                    return RedirectToAction("Welcome", "Account");
                }
                else
                {
                    ModelState.AddModelError("", ErrorCodeToString(createStatus));
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        // **************************************
        // URL: /Account/AdminSetup
        // **************************************

        [AllowAnonymous]
        public ActionResult AdminSetup()
        {
            if (MyRoles.IsExistUsersInRole(UserRoles.Admin.ToString()))
                return RedirectToAction("Register");
            return View();//ContextDependentView();
        }


        [AllowAnonymous]
        [HttpPost]
        public ActionResult AdminSetup(RegisterModel model)
        {
            if (ModelState.IsValid)
            {

                MembershipCreateStatus createStatus;
                Membership.CreateUser(model.UserName, model.Password, model.Email, null, null, true, null, out createStatus);

                if (createStatus == MembershipCreateStatus.Success)
                {
                    MyRoles.ChangeUserRole(model.UserName, UserRoles.Admin.ToString());
                    FormsAuthentication.SetAuthCookie(model.UserName, createPersistentCookie: false);
                    return RedirectToAction("Index", "Admin");
                }

                else
                {
                    ModelState.AddModelError("", ErrorCodeToString(createStatus));
                }
            }
            return View(model);
        }



        // GET: /Account/ChangePassword

        public ActionResult ChangePassword()
        {
            return View();
        }


        //
        // POST: /Account/ChangePassword

        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordModel model)
        {
            if (ModelState.IsValid)
            {

                // ChangePassword will throw an exception rather
                // than return false in certain failure scenarios.
                bool changePasswordSucceeded;
                try
                {
                    MembershipUser currentUser = Membership.GetUser(User.Identity.Name, userIsOnline: true);
                    changePasswordSucceeded = currentUser.ChangePassword(model.OldPassword, model.NewPassword);
                }
                catch (Exception)
                {
                    changePasswordSucceeded = false;
                }

                if (changePasswordSucceeded)
                {
                    return RedirectToAction("ChangePasswordSuccess");
                }
                else
                {
                    ModelState.AddModelError("", "Не правильний поточний пароль");
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ChangePasswordSuccess

        public ActionResult ChangePasswordSuccess()
        {
            return View();
        }
        [AllowAnonymous]
        public ActionResult Welcome()
        {
            return View();
        }

        public ActionResult FewPermissions()
        {
            return View("NotFound");
        }

        // **************************************
        // URL: /Account/Activate/username/key
        // **************************************

        [AllowAnonymous]
        public ActionResult Activate(string username, string key)
        {
            if (_db.ActivateUser(username, key) == false)
                ViewBag.active = false;
            else
                ViewBag.active = true;
            return View();
        }
    

        private IEnumerable<string> GetErrorsFromModelState()
        {
            return ModelState.SelectMany(x => x.Value.Errors
                .Select(error => error.ErrorMessage));
        }

        #region Status Codes
        private static string ErrorCodeToString(MembershipCreateStatus createStatus)
        {
            // See http://go.microsoft.com/fwlink/?LinkID=177550 for
            // a full list of status codes.
            switch (createStatus)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return "Логін користувача вже використовується. Спробуйте ввести інший.";

                case MembershipCreateStatus.DuplicateEmail:
                    return "Користувач з такою поштою вже зареєстрований. Введіть бідь-ласка іншу пошту.";

                case MembershipCreateStatus.InvalidPassword:
                    return "Не правильно введений пароль. Спробуйте ввести інший.";

                case MembershipCreateStatus.InvalidEmail:
                    return "Не правильна пошта. Перевірте її і спробуйте ще раз.";

                case MembershipCreateStatus.InvalidAnswer:
                    return "Не правильна відповідь. Перевірте її і спробуйте ще раз.";

                case MembershipCreateStatus.InvalidQuestion:
                    return "The password retrieval question provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidUserName:
                    return "The user name provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.ProviderError:
                    return "The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                case MembershipCreateStatus.UserRejected:
                    return "The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                default:
                    return "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
            }
        }
        #endregion
    }
}
