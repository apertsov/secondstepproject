using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DistanceLessons.Models;

namespace DistanceLessons.Controllers
{
    public class AdminController : Controller
    {
        //
        // GET: /Admin/

        private int itemOnPage = 5;
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
            ViewBag.Users = _db.GetUserList();
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
            ViewBag.Users = _db.GetUserList();
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
            _db.AddCategory(obj);
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
            _db.Save();
            return RedirectToAction("Categories");
        }

        [HttpGet]
        public ActionResult DeleteCategory(Guid id)
        {
            _db.DeleteCategory(id);
            return RedirectToAction("Categories");
        }

        [HttpGet]
        public ActionResult DetailsCategory(Guid id)
        {
            return View(_db.GetCategory(id));
        }

        public ActionResult Message()
        {
            return View();
        }

        public ActionResult News(int numPage=0)
        {
            ViewData["numPage"] = numPage;
            ViewData["itemsCount"] = _db.GetNewsList().Count;
            ViewData["itemOnPage"] = itemOnPage;

            if (!Request.IsAjaxRequest())
            {
                return View(_db.GetNewsList_time(itemOnPage, numPage));
            }
            else
            {
                return PartialView("_News", _db.GetNewsList_time(itemOnPage, numPage));
            }
        }


        [HttpGet]
        public ActionResult CreateNew()
        {
            return View();
        }


        [HttpPost]
        public ActionResult CreateNew(News obj)
        {
            try
            {
                obj.NewId = Guid.NewGuid();
                obj.Publication = System.DateTime.Now;
                obj.UserId = _db.GetUserId(User.Identity.Name);

                _db.AddNew(obj);
                _db.Save();
            }
            catch { }

            return RedirectToAction("News");
        }

        [HttpGet]
        public ActionResult EditNew(Guid id)
        {
            return View(_db.GetNew(id));
        }

        [HttpPost]
        public ActionResult EditNew(News obj)
        {
            News old = _db.GetNew(obj.NewId);
            UpdateModel(old);
            _db.Save();
            return RedirectToAction("News");
        }

        [HttpGet]
        public ActionResult DeleteNew(Guid id)
        {
            _db.DeleteNew(id);
            return RedirectToAction("News");
        }
    }
}
