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

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Users()
        {
           DataEntitiesManager A = new DataEntitiesManager();
           return View(A.GetUserList());

        }

        public ActionResult Categories()
        {
            DataEntitiesManager A = new DataEntitiesManager();
            return View(A.GetCategories());
        }

        public ActionResult Courses()
        {
            DataEntitiesManager A = new DataEntitiesManager(); 
            return View(A.GetCourseList());
        }

    [HttpGet]
        public ActionResult CreateCours()
        {
        DataEntitiesManager A = new DataEntitiesManager();
        ViewBag.Categories = A.GetCategoryList();
        ViewBag.Users = A.GetUserList();
        return View();
        }


    [HttpPost]
    public ActionResult CreateCours(Cours obj)
    {
        DataEntitiesManager A = new DataEntitiesManager();
        obj.CourseId = Guid.NewGuid();
        A.AddCours(obj);
        return RedirectToAction("Courses");
    }


    [HttpGet]
    public ActionResult EditCours(Guid id)
    {
        DataEntitiesManager A = new DataEntitiesManager();
        ViewBag.Categories = A.GetCategoryList();
        ViewBag.Users = A.GetUserList();
        return View(A.GetCourse(id));
    }

    [HttpPost]
    public ActionResult EditCours(Cours obj)
    {
        DataEntitiesManager A = new DataEntitiesManager();
        Cours old = A.GetCourse(obj.CourseId);
        UpdateModel(old);
        A.Save();
        return RedirectToAction("Courses");
    }

    [HttpGet]
    public ActionResult DeleteCours(Guid id)
    {
        DataEntitiesManager A = new DataEntitiesManager();
        A.DeleteCourse(id);
        return RedirectToAction("Courses");
    }


    [HttpGet]
    public ActionResult DetailsCours(Guid id)
    {
        DataEntitiesManager A = new DataEntitiesManager();
         return View(A.GetCourse(id));
    }


    [HttpGet]
        public ActionResult CreateUser()
        {
            DataEntitiesManager A = new DataEntitiesManager();
            ViewBag.Roles = A.GetRoleList();
            return View();
        }

    [HttpPost]
    public ActionResult CreateUser(User obj)
    {

        obj.UserId = Guid.NewGuid();
        obj.CreatedDate = DateTime.Now;
        obj.LastLoginDate = DateTime.Now;
        DataEntitiesManager A = new DataEntitiesManager();
        A.AddUser(obj);
        return RedirectToAction("Users");
 
    }

    [HttpGet]
    public ActionResult EditUser(Guid id)
    {
        DataEntitiesManager A = new DataEntitiesManager();
        ViewBag.Roles = A.GetRoleList();
        return View(A.GetUser(id));
    }


    [HttpPost]
    public ActionResult EditUser(User obj)
    {
        DataEntitiesManager A = new DataEntitiesManager();
        User old = A.GetUser(obj.UserId);
        UpdateModel(old);
        A.Save();
        return RedirectToAction("Users");
    }


    [HttpGet]
    public ActionResult DeleteUser(Guid id)
    {   
        DataEntitiesManager A = new DataEntitiesManager();
        A.DeleteUser(id);
        return RedirectToAction("Users");
    }

    [HttpGet]
    public ActionResult DetailsUser(Guid id)
    {
        DataEntitiesManager A = new DataEntitiesManager();
        return View(A.GetUser(id));
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
        DataEntitiesManager A = new DataEntitiesManager();
        A.AddCategory(obj);
        return RedirectToAction("Categories");
    }

    [HttpGet]
    public ActionResult EditCategory(Guid id)
    {
        DataEntitiesManager A = new DataEntitiesManager();
        return View(A.GetCategory(id));
    }

    [HttpPost]
    public ActionResult EditCategory(Category obj)
    {
        DataEntitiesManager A = new DataEntitiesManager();
        Category old = A.GetCategory(obj.CategoryId);
        UpdateModel(old);
        A.Save();
        return RedirectToAction("Categories");
    }

    [HttpGet]
    public ActionResult DeleteCategory(Guid id)
    {   
        DataEntitiesManager A = new DataEntitiesManager();
        A.DeleteCategory(id);
        return RedirectToAction("Categories");
    }

    [HttpGet]
    public ActionResult DetailsCategory(Guid id)
    {
        DataEntitiesManager A = new DataEntitiesManager();
        return View(A.GetCategory(id));
    }

        public ActionResult Message()
        {
            return View();
        }
    }
}
