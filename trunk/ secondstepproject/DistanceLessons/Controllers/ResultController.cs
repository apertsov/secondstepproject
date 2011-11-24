using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DistanceLessons.Attributes;
using DistanceLessons.Models;

namespace DistanceLessons.Controllers
{
    public class ResultController : Controller
    {
        //
        // GET: /Dean/
        private DataEntitiesManager _db;

        public ResultController()
            : base()
        {
            _db = new DataEntitiesManager();
        }

        public ActionResult Index()
        {
            return View(new EducationElementsModel { Categories = _db.CategoriesDictionaryAll(), Courses = new Dictionary<Guid, string>(), Modules = new Dictionary<Guid, string>(), pickedElement = null });
        }

        public ActionResult UserResults(Guid? id, int type = 0)
        {
            return PartialView("UserResults", new EducationElementsModel { pickedElement = id, ControllerName = "UserResults", type = type });
        }

        public ActionResult UserResultsTable(Guid id,string redirectController ,int type = 0)
        {
            ViewBag.controller = redirectController;
            if (type == 2) // picked course
            {
                return PartialView(_db.ResultUserCourse(id));
            }

            if (type == 3) // picked module
            {
                DetailModuleTestResultsModel model = new DetailModuleTestResultsModel { ModuleId = id, ModuleName = _db.ModuleName(id), TestResults = new List<TestResultModel>() };
                List<UserModule> userResults = _db.UserModules(id);
                foreach (UserModule userResult in userResults)
                    model.TestResults.Add(new TestResultModel { Login = userResult.User.Login, Result = userResult.Passed, StartTesting = userResult.StartTime });
         
                return  PartialView("DetailModuleResultsPartial",model);
            }

            return PartialView("UserResultsTable", null); 
        }

        public ActionResult EducationElements(Guid? id, string redirectController, int type = 0)
        {
            if (id != null)
            {
                if (type == 1) // picked category
                {
                    return PartialView("EducationElementsPartial",new EducationElementsModel { Categories = _db.CategoriesDictionaryAll(), Courses = _db.CoursesDictionaryFromCategory((Guid)id), Modules = new Dictionary<Guid, string>(), pickedElement = id, ControllerName=redirectController, type=1 });
                }

                if (type == 2) // picked course
                {
                    return PartialView("EducationElementsPartial", new EducationElementsModel { Categories = _db.CategoryDictionary(_db.CategoryIdFromCourseId((Guid)id)), Courses = _db.CoursesDictionaryFromCategory(_db.CategoryIdFromCourseId((Guid)id)), Modules = _db.ModulesDictionaryFromCourse((Guid)id), pickedElement = id, ControllerName = redirectController, type = 2 });
                }

                if (type == 3) // picked module
                {
                    return PartialView("EducationElementsPartial", new EducationElementsModel { Categories = _db.CategoryDictionary(_db.CategoryIdFromModuleId((Guid)id)), Courses = _db.CourseDictionary(_db.CourseIdFromModuleId((Guid)id)), Modules = _db.ModulesDictionaryFromCourse(_db.CourseIdFromModuleId((Guid)id)), pickedElement = id, ControllerName = redirectController, type = 3 });
                }
            }
            // default show
            return PartialView("EducationElementsPartial", new EducationElementsModel { Categories = _db.CategoriesDictionaryAll(), Courses = new Dictionary<Guid, string>(), Modules = new Dictionary<Guid, string>(), pickedElement = null, ControllerName = redirectController, type = 0 });
        }

    }
}
