using System;
using System.Web.Mvc;
using DistanceLessons.Attributes;
using DistanceLessons.Models;

namespace DistanceLessons.Controllers
{
    [Localization]
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        //
        // GET: /Admin/

        private DataEntitiesManager _db = new DataEntitiesManager();
/*
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
*/
        [HttpGet]
        public ActionResult Users()
        {
            return View(_db.GetUserList());

        }

        [HttpGet]
        public ActionResult Categories()
        {
            return View(_db.GetCategoryList());
        }

        [HttpGet]
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
        public ActionResult DeleteCours(Guid? id)
        {
            if ((id == null) || (!_db.ExistCourse((Guid)id)))
            {
                return new NotFoundMvc.NotFoundViewResult();
            }
            _db.DeleteCourse((Guid)id);
            return RedirectToAction("Courses");
        }


        [HttpGet]
        public ActionResult DetailsCours(Guid? id)
        {
            if ((id == null) || (!_db.ExistCourse((Guid)id)))
            {
                return new NotFoundMvc.NotFoundViewResult();
            }
            return View(_db.GetCourse((Guid)id));
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
        public ActionResult EditUser(Guid? id)
        {
            if ((id==null)||(!_db.ExistUser((Guid)id)))
            {
                return new NotFoundMvc.NotFoundViewResult();
            }
            ViewBag.Roles = _db.GetRoleList();
            ViewBag.IsMe = (_db.GetUser(User.Identity.Name).UserId==id)?true:false;
            return View(_db.GetUser((Guid)id));
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
        public ActionResult DeleteUser(Guid? id)
        {
            if ((id == null) || (!_db.ExistUser((Guid)id)))
            {
                return new NotFoundMvc.NotFoundViewResult();
            }
            _db.DeleteUser((Guid)id);
            return RedirectToAction("Users");
        }

        [HttpGet]
        public ActionResult DetailsUser(Guid? id)
        {
            if ((id == null) || (!_db.ExistUser((Guid)id)))
            {
                return new NotFoundMvc.NotFoundViewResult();
            }
            string login=_db.GetUser((Guid)id).Login;
            ViewBag.IsInfo = _db.ExistInformation(login);
            ViewBag.Info = _db.UserInformation(login);
            ViewBag.IsContact = _db.ExistContacts(login);
            ViewBag.Contacts = _db.UserContacts(login);
            return View(_db.GetUser((Guid)id));
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
        public ActionResult EditCategory(Guid? id)
        {
            if ((id==null)||(!_db.ExistCategory((Guid)id)))
            {
                return new NotFoundMvc.NotFoundViewResult();
            }
            return View(_db.GetCategory((Guid)id));
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
        public ActionResult DeleteCategory(Guid? id)
        {
            if  ((id==null)||(!_db.ExistCategory((Guid)id)))
            {
                return new NotFoundMvc.NotFoundViewResult();
            }
            _db.DeleteCategory((Guid)id);
            return RedirectToAction("Categories");
        }

    }
}
