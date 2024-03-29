﻿using System;
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
    [Localization]
    public class ResultController : Controller
    {
        //
        // GET: /Dean/
        private DataEntitiesManager _db = new DataEntitiesManager();

        [Authorize(Roles = "Admin, Teacher, Dean")]
        [HttpGet]
        [EnableCompression]
        public ActionResult Index()
        {
            return View(new EducationElementsModel { Categories = _db.CategoriesDictionaryAll(), Courses = new Dictionary<Guid, string>(), Modules = new Dictionary<Guid, string>(), Parameters = new BetweenPartialModel { pickedElement = null, ElementType = ElementsType.None } });
        }

        [Authorize(Roles = "Admin, Teacher, Dean")]
        [HttpGet]
        [EnableCompression]
        public ActionResult UserResults(Guid? id, string divIdToReplace, ElementsType type = ElementsType.None)
        {
            return PartialView("UserResults", new BetweenPartialModel { pickedElement = id, ControllerName = "UserResults", ElementType = type, DivIdToReplace = divIdToReplace });
        }

        [Authorize(Roles = "Admin, Teacher, Dean")]
        [HttpGet]
        [EnableCompression]
        public ActionResult EducationElements(Guid? id, string redirectController, string divIdToReplace, ElementsType type = ElementsType.None)
        {
            if ((id != null) && (type != ElementsType.User))
            {

                if (type == ElementsType.Category) // picked category
                {
                    if ((!_db.ExistCategory((Guid)id)))
                    {
                        return new NotFoundMvc.NotFoundViewResult();
                    }
                    return PartialView("EducationElementsPartial", new EducationElementsModel { Categories = _db.CategoriesDictionaryAll(), Courses = _db.CoursesDictionaryFromCategory((Guid)id), Modules = new Dictionary<Guid, string>(), Parameters = new BetweenPartialModel { pickedElement = id, ControllerName = redirectController, ElementType = ElementsType.Category, DivIdToReplace = divIdToReplace } });
                }

                if (type == ElementsType.Course) // picked course
                {
                    if ((!_db.ExistCourse((Guid)id)))
                    {
                        return new NotFoundMvc.NotFoundViewResult();
                    }
                    return PartialView("EducationElementsPartial", new EducationElementsModel { Categories = _db.CategoryDictionary(_db.CategoryIdFromCourseId((Guid)id)), Courses = _db.CoursesDictionaryFromCategory(_db.CategoryIdFromCourseId((Guid)id)), Modules = _db.ModulesDictionaryFromCourse((Guid)id), Parameters = new BetweenPartialModel { pickedElement = id, ControllerName = redirectController, ElementType = ElementsType.Course, DivIdToReplace = divIdToReplace } });
                }

                if (type == ElementsType.Module) // picked module
                {
                    if ((!_db.ExistModule((Guid)id)))
                    {
                        return new NotFoundMvc.NotFoundViewResult();
                    }
                    return PartialView("EducationElementsPartial", new EducationElementsModel { Categories = _db.CategoryDictionary(_db.CategoryIdFromModuleId((Guid)id)), Courses = _db.CourseDictionary(_db.CourseIdFromModuleId((Guid)id)), Modules = _db.ModulesDictionaryFromCourse(_db.CourseIdFromModuleId((Guid)id)), Parameters = new BetweenPartialModel { pickedElement = id, ControllerName = redirectController, ElementType = ElementsType.Module, DivIdToReplace = divIdToReplace } });
                }
            }
            // default show
            return PartialView("EducationElementsPartial", new EducationElementsModel { Categories = _db.CategoriesDictionaryAll(), Courses = new Dictionary<Guid, string>(), Modules = new Dictionary<Guid, string>(), Parameters = new BetweenPartialModel { pickedElement = id, ControllerName = redirectController, ElementType = ElementsType.None, DivIdToReplace = divIdToReplace } });
        }

        [Authorize(Roles = "Admin, Teacher, Dean")]
        [HttpGet]
        [EnableCompression]
        public ActionResult UserResultsTable(Guid? id, string redirectController, string divIdToReplace, ElementsType type = ElementsType.None)
        {
            if (id == null)
            {
                return new NotFoundMvc.NotFoundViewResult();
            }
            if (type == ElementsType.Module) // picked module
            {
                if ((!_db.ExistModule((Guid)id)))
                {
                    return new NotFoundMvc.NotFoundViewResult();
                }
                DetailModuleTestResultsModel model = new DetailModuleTestResultsModel
                {
                    Parameters = new BetweenPartialModel
                    {
                        pickedElement = id,
                        ControllerName = redirectController,
                        DivIdToReplace = divIdToReplace,
                        ElementType = ElementsType.Module
                    },
                    ModuleName = _db.ModuleName((Guid)id),
                    TestResults = new List<TestResultModel>()
                };
                List<UserModule> userResults = _db.UserModules((Guid)id);
                foreach (UserModule userResult in userResults)
                {
                    Information userInfo = _db.UserInformation(userResult.UserId);
                    model.TestResults.Add(new TestResultModel
                    {
                        Login = userResult.User.Login,
                        UserId = userResult.UserId,
                        StartTesting = userResult.StartTime,
                        FirstName = userInfo != null ? userInfo.FirstName : String.Empty,
                        LastName = userInfo != null ? userInfo.LastName : String.Empty,
                        MiddleName = userInfo != null ? userInfo.MidName : String.Empty,
                        MaxPoints = userResult.ModuleId == null ? (int?)null : userResult.Module.MaxPoints,
                        Result = userResult.Passed == null ? (int?)null : (int)_db.CalcCourseResult((float)userResult.Passed, userResult.Module.MaxPoints)
                    });
                }

                if (_db.IsTeacherCourse(_db.CourseIdFromModuleId((Guid)id), User.Identity.Name))
                    ViewBag.IsTeacherCourse = true;
                else ViewBag.IsTeacherCourse = false;
                return PartialView("DetailModuleResultsPartial", model);
            }

            if (type == ElementsType.Course) // picked course
            {
                if ((!_db.ExistCourse((Guid)id)))
                {
                    return new NotFoundMvc.NotFoundViewResult();
                }
                ViewBag.controller = redirectController;
                ViewBag.replaceDiv = divIdToReplace;
                return PartialView(_db.ResultsUsersCourse((Guid)id));
            }

            if (type == ElementsType.User)
            {
                if ((!_db.ExistUser((Guid)id)))
                {
                    return new NotFoundMvc.NotFoundViewResult();
                }
                ViewBag.controller = redirectController;
                ViewBag.replaceDiv = divIdToReplace;
                return PartialView("StudentResult", _db.StudentResult(_db.GetUsername((Guid)id)));
            }

            return PartialView("UserResultsTable", null);
        }

         [HttpGet]
        [Authorize(Roles = "Admin, Teacher, Dean")]
        [EnableCompression]
        public ActionResult DetailsModuleUserAnswers(Guid? id, string username, string controllerName, string divIdToReplace)
        {
            if ((id == null) || (!_db.ExistModule((Guid)id)) || (String.IsNullOrEmpty(username)) || (!_db.ExistUser(username)))
            {
                return new NotFoundMvc.NotFoundViewResult();
            }
            UserModule userResult = _db.UserModule((Guid)id, username);
            ModuleUserAnswers model = new ModuleUserAnswers { Parameters = new BetweenPartialModel { pickedElement = id, ControllerName = controllerName, DivIdToReplace = divIdToReplace, ElementType = ElementsType.Module }, Username = username, Answers = new List<DetailAnswersModel>() };
            if (userResult == null) return PartialView(model);
            model.ModuleTitle = userResult.Module.Title;
            model.Answers = new List<DetailAnswersModel>();
            model.ResultPercent = userResult.Passed;
            model.Result = _db.CalcCourseResult((float)userResult.Passed, userResult.Module.MaxPoints);
            List<ShowTest> showedTests = _db.ShowTestsInModule((Guid)id, username);
            List<Answer> answers = new List<Answer>();
            foreach (ShowTest test in showedTests)
            {
                answers.Clear();
                List<UserAnswer> userAnswers = _db.UserAnswersOnTest(test.TestId, (Guid)id, username);
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
        [EnableCompression]
        public ActionResult DeletePassModule(Guid? id, string username, string controllerName, string divIdToReplace)
        {
            if ((id == null) ||(String.IsNullOrEmpty(username))||(!_db.ExistUser(username))|| (!_db.ExistModule((Guid)id)))
            {
                return new NotFoundMvc.NotFoundViewResult();
            }
            _db.DeleteModuleTest((Guid)id, username);
            return PartialView("UserResults", new BetweenPartialModel { pickedElement = id, ControllerName = "UserResults", ElementType = ElementsType.Module, DivIdToReplace = divIdToReplace });
        }

        [HttpGet]
        [EnableCompression]
        public ActionResult UserResult()
        {
            return PartialView(_db.ResultUserCourse(User.Identity.Name));
        }

        [HttpGet]
        [EnableCompression]
        public ActionResult StudentResult()   //string username, string divIdToReplace = "", string redirectController = "")
        {
            ViewBag.controller = "";
            ViewBag.replaceDiv = "";
            return PartialView(_db.StudentResult(User.Identity.Name));
        }




    }
}
