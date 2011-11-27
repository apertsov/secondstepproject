using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DistanceLessons.Attributes;
using DistanceLessons.Models;

namespace DistanceLessons.Controllers
{
  //  [AutorizeFilterAttribute]
    
    [Authorize]
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

        [Authorize(Roles = "Admin, Teacher, Dean")]  
        [HttpGet]
        public ActionResult Index()
        {
            return View(new EducationElementsModel { Categories = _db.CategoriesDictionaryAll(), Courses = new Dictionary<Guid, string>(), Modules = new Dictionary<Guid, string>(), Parameters = new BetweenPartialModel { pickedElement = null, ElementType = ElementsType.None } });
        }

        [Authorize(Roles = "Admin, Teacher, Dean")]  
        [HttpGet]
        public ActionResult UserResults(Guid? id, string divIdToReplace, ElementsType type = ElementsType.None)
        {
            return PartialView("UserResults", new BetweenPartialModel { pickedElement = id, ControllerName = "UserResults", ElementType = type, DivIdToReplace = divIdToReplace });
        }

        [Authorize(Roles = "Admin, Teacher, Dean")]  
        [HttpGet]
        public ActionResult EducationElements(Guid? id, string redirectController, string divIdToReplace, ElementsType type = ElementsType.None)
        {
            if (id != null)
            {

                if (type == ElementsType.Category) // picked category
                {
                    return PartialView("EducationElementsPartial", new EducationElementsModel { Categories = _db.CategoriesDictionaryAll(), Courses = _db.CoursesDictionaryFromCategory((Guid)id), Modules = new Dictionary<Guid, string>(), Parameters = new BetweenPartialModel { pickedElement = id, ControllerName = redirectController, ElementType = ElementsType.Category, DivIdToReplace = divIdToReplace } });
                }

                if (type == ElementsType.Course) // picked course
                {
                    return PartialView("EducationElementsPartial", new EducationElementsModel { Categories = _db.CategoryDictionary(_db.CategoryIdFromCourseId((Guid)id)), Courses = _db.CoursesDictionaryFromCategory(_db.CategoryIdFromCourseId((Guid)id)), Modules = _db.ModulesDictionaryFromCourse((Guid)id), Parameters = new BetweenPartialModel { pickedElement = id, ControllerName = redirectController, ElementType = ElementsType.Course, DivIdToReplace = divIdToReplace } });
                }

                if (type == ElementsType.Module) // picked module
                {
                    return PartialView("EducationElementsPartial", new EducationElementsModel { Categories = _db.CategoryDictionary(_db.CategoryIdFromModuleId((Guid)id)), Courses = _db.CourseDictionary(_db.CourseIdFromModuleId((Guid)id)), Modules = _db.ModulesDictionaryFromCourse(_db.CourseIdFromModuleId((Guid)id)), Parameters = new BetweenPartialModel { pickedElement = id, ControllerName = redirectController, ElementType = ElementsType.Module, DivIdToReplace = divIdToReplace } });
                }
            }
            // default show
            return PartialView("EducationElementsPartial", new EducationElementsModel { Categories = _db.CategoriesDictionaryAll(), Courses = new Dictionary<Guid, string>(), Modules = new Dictionary<Guid, string>(), Parameters = new BetweenPartialModel { pickedElement = id, ControllerName = redirectController, ElementType = ElementsType.None, DivIdToReplace = divIdToReplace } });
        }

        [Authorize(Roles = "Admin, Teacher, Dean")]  
        [HttpGet]
        public ActionResult UserResultsTable(Guid id, string redirectController, string divIdToReplace, ElementsType type = ElementsType.None)
        {
            if (type == ElementsType.Module) // picked module
            {
                DetailModuleTestResultsModel model = new DetailModuleTestResultsModel { Parameters = new BetweenPartialModel { pickedElement = id, ControllerName = redirectController, DivIdToReplace = divIdToReplace, ElementType = ElementsType.Module }, ModuleName = _db.ModuleName(id), TestResults = new List<TestResultModel>() };
                List<UserModule> userResults = _db.UserModules(id);
                foreach (UserModule userResult in userResults)
                    model.TestResults.Add(new TestResultModel { Login = userResult.User.Login, Result = userResult.Passed, StartTesting = userResult.StartTime });

                if (_db.IsTeacherCourse(_db.CourseIdFromModuleId(id), User.Identity.Name))
                    ViewBag.IsTeacherCourse = true;
                else ViewBag.IsTeacherCourse = false;
                return PartialView("DetailModuleResultsPartial", model);
            }

            if (type == ElementsType.Course) // picked course
            {
                ViewBag.controller = redirectController;
                ViewBag.replaceDiv = divIdToReplace;
                return PartialView(_db.ResultsUsersCourse(id));
            }

            return PartialView("UserResultsTable", null);
        }

        [Authorize(Roles = "Admin, Teacher, Dean")]  
        [HttpGet]
        public ActionResult DetailsModuleUserAnswers(Guid id, string username, string controllerName, string divIdToReplace)
        {
            UserModule userResult = _db.UserModule(id, username);
            ModuleUserAnswers model = new ModuleUserAnswers { Parameters = new BetweenPartialModel { pickedElement = id, ControllerName = controllerName, DivIdToReplace = divIdToReplace, ElementType = ElementsType.Module }, Username = username };
            if (userResult == null) return PartialView(model);
            model.ModuleTitle = userResult.Module.Title;
            model.Result = userResult.Passed;
            model.Answers = new List<DetailAnswersModel>();
            List<ShowTest> showedTests = _db.ShowTestsInModule(id, username);
            List<Answer> answers = new List<Answer>();
            foreach (ShowTest test in showedTests)
            {
                answers.Clear();
                List<UserAnswer> userAnswers = _db.UserAnswersOnTest(test.TestId, id, username);
                model.Answers.Add(new DetailAnswersModel
                {
                    UserAnswers = userAnswers,
                    TestAndAnswers = new TestAndAnswersModel { Test = test.Test, AnswerList = _db.TestAnswers(test.TestId) },
                    IsRightAnswer = _db.IsTrueAnswer(userAnswers, test.TestId)
                });
            }
            return PartialView(model);
        }

        [Authorize(Roles = "Admin, Teacher, Dean")]  
        [HttpGet]
        public ActionResult DeletePassModule(Guid id, string username, string controllerName, string divIdToReplace)
        {
            _db.DeleteModuleTest(id, username);
            return PartialView("UserResults", new BetweenPartialModel { pickedElement = id, ControllerName = "UserResults", ElementType = ElementsType.Module, DivIdToReplace = divIdToReplace });
        }

        [HttpGet]
        public ActionResult UserResult()
        {
            return PartialView(_db.ResultUserCourse(User.Identity.Name));
        }

    }
}
