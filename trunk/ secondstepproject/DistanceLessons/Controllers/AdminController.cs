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
            return View(A.GetCategoryList());
        }

        public ActionResult Courses()
        {
            DataEntitiesManager A = new DataEntitiesManager(); 
            return View(A.GetCourseList());
        }
//in process
    [HttpGet]
        public ActionResult CreateCours()
        {
            DataEntitiesManager A = new DataEntitiesManager();
        
            return View();
        }
    //in process
    [HttpPost]
    public ActionResult CreateCours(Cours obj)
    {
        obj.CategoryId = Guid.NewGuid();
        DataEntitiesManager A = new DataEntitiesManager();
        
        return View();
    }
    //in process
    [HttpGet]
    public ActionResult EditCours(Guid id)
    {
        DataEntitiesManager A = new DataEntitiesManager();

        return View();
    }
    //in process
    [HttpGet]
    public ActionResult DeleteCours(Guid id)
    {   
        DataEntitiesManager A = new DataEntitiesManager();

        return View();
    }
    //in process
    [HttpGet]
    public ActionResult DetailsCours(Guid id)
    {
        DataEntitiesManager A = new DataEntitiesManager();
        Category cat = new Category();
        cat.CategoryId = id;
        return View(cat.CategoryId);
    }

        //in process
    [HttpGet]
        public ActionResult CreateUser()
        {
            DataEntitiesManager A = new DataEntitiesManager();
        
            return View();
        }
    //in process
    [HttpPost]
    public ActionResult CreateUser(Cours obj)
    {
        obj.CategoryId = Guid.NewGuid();
        DataEntitiesManager A = new DataEntitiesManager();
        
        return View();
    }
    //in process
    [HttpGet]
    public ActionResult EditUser(Guid id)
    {
        DataEntitiesManager A = new DataEntitiesManager();

        return View();
    }
    //in process
    [HttpGet]
    public ActionResult DeleteUser(Guid id)
    {   
        DataEntitiesManager A = new DataEntitiesManager();

        return View();
    }
    //in process
    [HttpGet]
    public ActionResult DetailsUser(Guid id)
    {
        DataEntitiesManager A = new DataEntitiesManager();
        User user = new User();

        return View();
    }

                //in process
    [HttpGet]
        public ActionResult CreateCategory()
        {
            DataEntitiesManager A = new DataEntitiesManager();
            
            return View();
        }
    //in process
    [HttpPost]
    public ActionResult CreateCategory(Cours obj)
    {
        obj.CategoryId = Guid.NewGuid();
        DataEntitiesManager A = new DataEntitiesManager();
        
        return View();
    }
    //in process
    [HttpGet]
    public ActionResult EditCategory(Guid id)
    {
        DataEntitiesManager A = new DataEntitiesManager();

        return View();
    }
    //in process
    [HttpGet]
    public ActionResult DeleteCategory(Guid id)
    {   
        DataEntitiesManager A = new DataEntitiesManager();

        return View();
    }
    //in process
    [HttpGet]
    public ActionResult DetailsCategory(Guid id)
    {
        DataEntitiesManager A = new DataEntitiesManager();

        return View();
    }

        public ActionResult Message()
        {
            return View();
        }
    }
}
