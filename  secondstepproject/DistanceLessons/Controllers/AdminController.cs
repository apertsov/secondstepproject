using System;
using System.Web.Mvc;
using DistanceLessons.Attributes;
using DistanceLessons.Models;

namespace DistanceLessons.Controllers
{
    [Localization]
    public class AdminController : Controller
    {
        //
        // GET: /Admin/

        private DataEntitiesManager _db = new DataEntitiesManager();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Users()
        {
            return View(_db.GetUserList());

        }

        public ActionResult Categories()
        {
            return View(_db.GetCategoryList());
        }

        public ActionResult Courses()
        {
            return View(_db.GetCourseList());
        }

        [HttpGet]
        public ActionResult CreateCours()
        {
            ViewBag.Categories = _db.GetCategoryList();
            ViewBag.Users = _db.GetGetUserByRole("Teacher");
            return View();
        }


        [HttpPost]
        public ActionResult CreateCours(Cours obj)
        {
            obj.CourseId = Guid.NewGuid();
            _db.AddCours(obj);
            return RedirectToAction("Courses");
        }


        [HttpGet]
        public ActionResult EditCours(Guid id)
        {
            ViewBag.Categories = _db.GetCategoryList();
            ViewBag.Users = _db.GetGetUserByRole("Teacher");
            return View(_db.GetCourse(id));
        }

        [HttpPost]
        public ActionResult EditCours(Cours obj)
        {
            Cours old = _db.GetCourse(obj.CourseId);
            UpdateModel(old);
            _db.Save();
            return RedirectToAction("Courses");
        }

        [HttpGet]
        public ActionResult DeleteCours(Guid id)
        {
            _db.DeleteCourse(id);
            return RedirectToAction("Courses");
        }


        [HttpGet]
        public ActionResult DetailsCours(Guid id)
        {
            return View(_db.GetCourse(id));
        }


        [HttpGet]
        public ActionResult CreateUser()
        {
            ViewBag.Roles = _db.GetRoleList();
            return View();
        }

        [HttpPost]
        public ActionResult CreateUser(User obj)
        {

            obj.UserId = Guid.NewGuid();
            obj.CreatedDate = DateTime.Now;
            obj.LastLoginDate = DateTime.Now;
            _db.AddUser(obj);
            return RedirectToAction("Users");

        }

        [HttpGet]
        public ActionResult EditUser(Guid id)
        {
            ViewBag.Roles = _db.GetRoleList();
            return View(_db.GetUser(id));
        }


        [HttpPost]
        public ActionResult EditUser(User obj)
        {
            User old = _db.GetUser(obj.UserId);
            UpdateModel(old);
            _db.Save();
            return RedirectToAction("Users");
        }


        [HttpGet]
        public ActionResult DeleteUser(Guid id)
        {
            _db.DeleteUser(id);
            return RedirectToAction("Users");
        }

        [HttpGet]
        public ActionResult DetailsUser(Guid id)
        {
            string login=_db.GetUser(id).Login;
            ViewBag.IsInfo = _db.ExistInformation(login);
            ViewBag.Info = _db.UserInformation(login);
            ViewBag.IsContact = _db.ExistContacts(login);
            ViewBag.Contacts = _db.UserContacts(login);
            return View(_db.GetUser(id));
        }


        [HttpGet]
        public ActionResult CreateCategory()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateCategory(Category obj)
        {
            obj.CategoryId = Guid.NewGuid();
            if (ModelState.IsValid)
                _db.AddCategory(obj);
            else View();
            return RedirectToAction("Categories");
        }

        [HttpGet]
        public ActionResult EditCategory(Guid id)
        {
            return View(_db.GetCategory(id));
        }

        [HttpPost]
        public ActionResult EditCategory(Category obj)
        {
            Category old = _db.GetCategory(obj.CategoryId);
            UpdateModel(old);
            if (ModelState.IsValid)
            _db.Save();
            else View(old.CategoryId);
            return RedirectToAction("Categories");
        }

        [HttpGet]
        public ActionResult DeleteCategory(Guid id)
        {
            _db.DeleteCategory(id);
            return RedirectToAction("Categories");
        }

    }
}
