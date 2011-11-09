using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DistanceLessons.Models;

namespace DistanceLessons.Controllers
{
    [Authorize(Roles = "Admin, Teacher")]
    public class TestController : Controller
    {
        private DataEntitiesManager _db = new DataEntitiesManager();
        //
        // GET: /Test/
        [HttpGet]
        public ActionResult Index(Guid id)
        {
            // if ( HttpContext.Application.Add("LessonId",id);
            if (_db.IsLesson(id)) Session["LessonId"] = id;
            return View(_db.LessonTestsAndAnswers(id));
        }



        [HttpGet]
        public ActionResult EditTest(Guid id)
        {
            TestAndAnswersModel model = new TestAndAnswersModel { Test = _db.Test(id), AnswerList = _db.TestAnswers(id) };
            if (!Request.IsAjaxRequest())
            {
                return View(model);
            }
            else
            {
                return PartialView("EditTestPartial", model);
            }
        }

        [HttpPost]
        public ActionResult EditTest(TestAndAnswersModel obj)
        {
            if (!Request.IsAjaxRequest())
            {
                if (String.IsNullOrEmpty(obj.Test.Question))
                {
                    ModelState.AddModelError("", "Заповніть рядок запитання");
                    return View("EditTest", obj);
                }
                foreach (Answer answer in obj.AnswerList)
                {
                    if (String.IsNullOrEmpty(answer.Answer1))
                    {
                        ModelState.AddModelError("", "Заповніть всі відповіді");
                        return View("EditTest", obj);
                    }
                }
            }
            _db.UpdateTest(obj.Test);
            _db.UpdateAnswers(obj.AnswerList);
            _db.Save();
            if (!Request.IsAjaxRequest())
            {
                return View("Index", _db.LessonTestsAndAnswers(obj.Test.LessonId));
            }
            else
            {
                return PartialView("QuestionsAndAnswersPartialPage", _db.LessonTestsAndAnswers(obj.Test.LessonId));
            }
        }

        [HttpGet]
        public ActionResult AddAnswer(Guid id)
        {
            if (_db.IsTest(id))
                _db.AddAnswer(new Answer { AnswerId = Guid.NewGuid(), TestId = id, Valid = false });
            if (!Request.IsAjaxRequest())
            {
                return View("EditTest", new TestAndAnswersModel { Test = _db.Test(id), AnswerList = _db.TestAnswers(id) });
            }
            else
            {

                return PartialView("EditTestPartial", new TestAndAnswersModel { Test = _db.Test(id), AnswerList = _db.TestAnswers(id) });
            }
        }

        [HttpGet]
        public ActionResult DeleteAnswer(Guid id, Guid testId)
        {
            if (_db.IsAnswer(id))
                _db.DeleteAnswer(id);

            if (!Request.IsAjaxRequest())
            {
                return View("EditTest", new TestAndAnswersModel { Test = _db.Test(testId), AnswerList = _db.TestAnswers(testId) });
            }
            else
            {
                return PartialView("EditTestPartial", new TestAndAnswersModel { Test = _db.Test(testId), AnswerList = _db.TestAnswers(testId) });
            }
        }

        [HttpGet]
        public ActionResult AddTest(Guid id)
        {
            //if (!_db.IsLesson(id)) перехід до списку предметів викладача
            Test test = new Test { TestId = Guid.NewGuid(), LessonId = id, Question = "" };
            List<Answer> answers = new List<Answer>();
            answers.Add(new Answer { AnswerId = Guid.NewGuid(), TestId = test.TestId, Valid = true });
            answers.Add(new Answer { AnswerId = Guid.NewGuid(), TestId = test.TestId, Valid = false });
            TestAndAnswersModel model = new TestAndAnswersModel { Test = test, AnswerList = answers };
            _db.AddTest(test);
            _db.AddAnswer(answers[0]);
            _db.AddAnswer(answers[1]);
            if (!Request.IsAjaxRequest())
                return View("EditTest", model);
            else
                return PartialView("EditTestPartial", model);
        }

        [HttpGet]
        public ActionResult DeleteTest(Guid testId)
        {
            if (_db.IsTest(testId))
                _db.DeleteTestWithAnswers(testId);
            if (!Request.IsAjaxRequest())
            {
                return View("Index", _db.LessonTestsAndAnswers((Guid)Session["LessonId"]));
            }
            else
            {
                return PartialView("QuestionsAndAnswersPartialPage", _db.LessonTestsAndAnswers((Guid)Session["LessonId"]));
            }
        }

        [HttpGet]
        public ActionResult PassModule(Guid id)
        {
            Module module = _db.Module(id);
            if ((DateTime.Now < module.DateBeginTesting) && (DateTime.Now >= module.DateEndTesting))
                return View("NotInTestTime", module);

            if (!_db.IsExistModuleUserAnswers(id, User.Identity.Name)) // чи користувач починав тестування з цього модуля 
            {// не починав
                List<Test> tests = _db.ModuleTests(id);
                if (module.CountQuestions != null)
                {
                    TestHelper.FilterTests(ref tests, (int)module.CountQuestions);
                }
                _db.AddNullAnswersOnTests(tests, id, User.Identity.Name);
            }

         //   if (_db.IsAllShowedModuleUserAnswers(id,User.Identity.Name)) // модуль вже зданий
                //return View("ShowModuleResults");
            Session.Add("TestTime", module.TimeForPassTest);
            return View(NextNotShowedTest(id, User.Identity.Name));
        }
/*
        [HttpPost]
        public ActionResult PassModule(Guid id)
        {

        }
        */
        private PassTestModel NextNotShowedTest(Guid ModuleId, string username)
        {
            List<UserAnswer> answers = _db.GetUserAnswersNotShowed(ModuleId, username);
            Random rand = new Random();
            Guid? TestId = answers[rand.Next(answers.Count)].TestId;
            if (TestId == null) throw new Exception();
            return new PassTestModel { ModuleId = ModuleId, TestAndAnswers = new TestAndAnswersModel { Test = _db.Test((Guid)TestId), AnswerList = _db.TestAnswers((Guid)TestId) } };
        }

    }
}
