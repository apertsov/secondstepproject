using System;
using System.Collections.Generic;
using System.Web.Mvc;
using DistanceLessons.Attributes;
using DistanceLessons.Models;

namespace DistanceLessons.Controllers
{
    [Localization]
    [Authorize(Roles = "Admin, Teacher, Student")]
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

            if (!_db.IsStartModuleTest(id, User.Identity.Name)) // чи користувач починав тестування з цього модуля 
            {// не починав
                List<Test> tests = _db.ModuleTests(id);
                if (tests.Count == 0) return View("TestingNotPrepared", module);
                if (module.CountQuestions != null) //якщо null то додаються всі тести
                {
                    TestHelper.FilterTests(ref tests, (int)module.CountQuestions);
                }
                _db.AddNotShowedTests(tests, id, User.Identity.Name);
                DateTime endTime = new DateTime();
                endTime=DateTime.Now.AddMinutes(module.TimeForPassTest);
                _db.AddUserModules(new UserModule { UserModuleId = Guid.NewGuid(), ModuleId = id, UserId = _db.GetUserId(User.Identity.Name), Passed = null, EndTime = endTime, StartTime = DateTime.Now });
                return View(NextNotShowedTest(id, User.Identity.Name,endTime.Subtract(DateTime.Now)));
            }
            UserModule previousResult = _db.UserModule(id, User.Identity.Name);
            if (previousResult.Passed!=null) // модуль вже зданий
                return View("CalcModuleResults", previousResult);
           // уже починав тестування і не закінчив його
            // якщо час закінчився то рахуємо результат
            if (previousResult.EndTime.CompareTo(DateTime.Now) < 0)
                return RedirectToAction("CalcModuleResults", new { id = id });
            return View(NextNotShowedTest(id, User.Identity.Name,previousResult.EndTime.Subtract(DateTime.Now)));
        }

        [HttpPost]
        public ActionResult PassModule(Guid moduleId, Guid testId)
        {
            // PassTestModel { PassedModule = _db.Module(moduleId), TestAndAnswers = new TestAndAnswersModel { Test = _db.Test((Guid)answerforPass.TestId), AnswerList = _db.TestAnswers((Guid)answerforPass.TestId) } };
            List<Answer> answers = _db.TestAnswers(testId);
            bool isChecked;
            foreach (Answer answer in answers)
                if ((Request.Form.GetValues(answer.AnswerId.ToString())[0] != null) && (Boolean.TryParse(Request.Form.GetValues(answer.AnswerId.ToString())[0], out isChecked) == true))
                {
                    if (isChecked) _db.AddUserAnswer(testId, moduleId, User.Identity.Name, answer.AnswerId);
                }
            if (_db.IsAllShowedModuleUserTests(moduleId, User.Identity.Name))
                return RedirectToAction("CalcModuleResults", new { id = moduleId });
            UserModule previousResult = _db.UserModule(moduleId, User.Identity.Name);            
          if (previousResult.EndTime.CompareTo(DateTime.Now) < 0)
               return RedirectToAction("CalcModuleResults", new { id = moduleId });
            return View("PassModule", NextNotShowedTest(moduleId, User.Identity.Name, previousResult.EndTime.Subtract(DateTime.Now)));
        }

        [HttpGet]
        public ActionResult DeletePassModule(Guid id)
        {
            _db.DeleteModuleTest(id, User.Identity.Name);
            return RedirectToAction("Index", "Teacher");
        }

        [HttpGet]
        public ActionResult CalcModuleResults(Guid id)
        {
            //  if (_db.IsAllShowedModuleUserAnswers(id, User.Identity.Name)) redirect..
            if (!_db.IsPassedTest(id, User.Identity.Name))
            {
                _db.MarkModuleTestsShowed(id, User.Identity.Name);
                _db.UpdateUserModule(id, User.Identity.Name, _db.CalcUserModuleResults(id, User.Identity.Name));
            }
            ViewBag.ModuleName = _db.ModuleName(id);
            return View(_db.UserModule(id, User.Identity.Name));

        }

        [HttpGet]
        public ActionResult DetailsModuleUserAnswers(Guid id, string username)
        {
            UserModule userResult=_db.UserModule(id, username);
            ViewBag.Username = username;
            if (userResult==null) return View((object)null);
            ViewBag.ModuleName=userResult.Module.Title;
            ViewBag.UserResult=userResult.Passed;
            
            List<DetailAnswersModel> model = new List<DetailAnswersModel>();
            List<ShowTest> showedTests = _db.ShowTestsInModule(id, username);
            List<Answer> answers=new List<Answer>();
            foreach (ShowTest test in showedTests)
            {
                answers.Clear();
                List<UserAnswer> userAnswers = _db.UserAnswersOnTest(test.TestId,id, username);
              /*  foreach (UserAnswer tmp in userAnswers)
                    if(tmp.AnswerId!=null) answers.Add(_db.Answer((Guid)tmp.AnswerId));*/
                model.Add(new DetailAnswersModel
                {
                    UserAnswers = userAnswers,
                    TestAndAnswers = new TestAndAnswersModel { Test = test.Test, AnswerList = _db.TestAnswers(test.TestId) },
                    IsRightAnswer = _db.IsTrueAnswer(userAnswers, test.TestId)
                });
            }
            return View(model);
        }

        private PassTestModel NextNotShowedTest(Guid moduleId, string username,TimeSpan timeToResolve)
        {
            List<Guid> testsId = _db.GetNotShowedTestsId(moduleId, username); // вибираємо всі тести з модуля що не показані користувачу
            if (testsId.Count == 0) throw new Exception();
            Random rand = new Random();
            Guid testIdForPass = testsId[rand.Next(testsId.Count)]; // вибираємо один з тестів
            _db.MarkShowedTest(testIdForPass, moduleId, User.Identity.Name); // позначаємо обраний тест як показаний
            return new PassTestModel { PassedModule = _db.Module(moduleId), TestAndAnswers = new TestAndAnswersModel { Test = _db.Test(testIdForPass), AnswerList = _db.TestAnswers(testIdForPass) }, TimeToResolve=timeToResolve  };
        }


    }
}
