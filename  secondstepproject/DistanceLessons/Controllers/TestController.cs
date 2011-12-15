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
        public ActionResult ShowLessonTests(Guid? id, Guid? courseId)
        {
            if ((id == null) || (courseId == null) || (!_db.ExistLesson((Guid)id)) || (!_db.ExistCourse((Guid)courseId)))
            {
                return new NotFoundMvc.NotFoundViewResult();
            }
            ViewBag.CourseId = courseId;
            LessonTestAndAnswersModel model = new LessonTestAndAnswersModel { Lesson = _db.Lesson((Guid)id), TestAndAnswers = _db.LessonTestsAndAnswers((Guid)id) };
            return View("ShowLessonTests", model);
        }

        [HttpGet]
        public ActionResult ShowModuleTests(Guid? id)
        {
            if ((id == null) || (!_db.ExistModule((Guid)id)))
            {
                return new NotFoundMvc.NotFoundViewResult();
            }
            ModuleTestAndAnswersModel model = new ModuleTestAndAnswersModel { Module = _db.Module((Guid)id), LessonsTestsAndAnswers = _db.ModuleTestsAndAnswers((Guid)id) };
            return View(model);
        }


        [HttpGet]
        public ActionResult EditTest(Guid? id)
        {
            if ((id == null) || (!_db.ExistTest((Guid)id)))
            {
                return new NotFoundMvc.NotFoundViewResult();
            }
            TestAndAnswersModel model = new TestAndAnswersModel { Test = _db.Test((Guid)id), AnswerList = _db.TestAnswers((Guid)id) };
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
            if (!Request.IsAjaxRequest())
            {
                return RedirectToAction("ShowLessonTests", new { id=obj.Test.LessonId, courseId=obj.Test.Lesson.CourseId });
            }
            else
            {
                LessonTestAndAnswersModel model = new LessonTestAndAnswersModel { Lesson = _db.Lesson(obj.Test.LessonId), TestAndAnswers = _db.LessonTestsAndAnswers(obj.Test.LessonId) };
                return PartialView("QuestionsAndAnswersPartialPage", model);
            }
        }

        [HttpGet]
        public ActionResult AddAnswer(Guid? id)
        {
            if ((id == null) || (!_db.ExistTest((Guid)id)))
            {
                return new NotFoundMvc.NotFoundViewResult();
            }
            _db.AddAnswer(new Answer { AnswerId = Guid.NewGuid(), TestId = (Guid)id, Valid = false });
            if (!Request.IsAjaxRequest())
            {
                return View("EditTest", new TestAndAnswersModel { Test = _db.Test((Guid)id), AnswerList = _db.TestAnswers((Guid)id) });
            }
            else
            {
                return PartialView("EditTestPartial", new TestAndAnswersModel { Test = _db.Test((Guid)id), AnswerList = _db.TestAnswers((Guid)id) });
            }
        }

        [HttpGet]
        public ActionResult DeleteAnswer(Guid? id, Guid? testId)
        {
            if ((id == null) || (testId==null)||(!_db.ExistAnswer((Guid)id))||(!_db.ExistTest((Guid)testId)))
            {
                return new NotFoundMvc.NotFoundViewResult();
            }
                _db.DeleteAnswer((Guid)id);

            if (!Request.IsAjaxRequest())
            {
                return View("EditTest", new TestAndAnswersModel { Test = _db.Test((Guid)testId), AnswerList = _db.TestAnswers((Guid)testId) });
            }
            else
            {
                return PartialView("EditTestPartial", new TestAndAnswersModel { Test = _db.Test((Guid)testId), AnswerList = _db.TestAnswers((Guid)testId) });
            }
        }

        [HttpGet]
        public ActionResult AddTest(Guid? id)
        {
            if ((id == null) || (!_db.ExistLesson((Guid)id)))
            {
                return new NotFoundMvc.NotFoundViewResult();
            }
            if (!Request.IsAjaxRequest())
            {
                return View("EditTestPartial");
            }
            else
            {
                Test test = new Test { TestId = Guid.NewGuid(), LessonId = (Guid)id, Question = "" };
                List<Answer> answers = new List<Answer>();
                answers.Add(new Answer { AnswerId = Guid.NewGuid(), TestId = test.TestId, Valid = true });
                answers.Add(new Answer { AnswerId = Guid.NewGuid(), TestId = test.TestId, Valid = false });
                TestAndAnswersModel model = new TestAndAnswersModel { Test = test, AnswerList = answers };
                _db.AddTest(test);
                _db.AddAnswer(answers[0]);
                _db.AddAnswer(answers[1]);
                return PartialView("EditTestPartial",model);
            }
        }

        [HttpGet]
        public ActionResult DeleteTest(Guid? testId)
        {
            if ((testId == null) || (!_db.ExistTest((Guid)testId)))
            {
                return new NotFoundMvc.NotFoundViewResult();
            }
            Lesson lesson=_db.Lesson(_db.Test((Guid)testId).LessonId);
            _db.DeleteTestWithAnswers((Guid)testId);
            if (!Request.IsAjaxRequest())
            {
                return RedirectToAction("ShowLessonTests", new { id = lesson.LessonId, courseId = lesson.CourseId });
            }
            else
            {
                LessonTestAndAnswersModel model = new LessonTestAndAnswersModel { Lesson = _db.Lesson(lesson.LessonId), TestAndAnswers = _db.LessonTestsAndAnswers(lesson.LessonId) };
                return PartialView("QuestionsAndAnswersPartialPage", model);
            }
        }

        [HttpGet]
        public ActionResult PassModule(Guid? id)
        {
            if ((id == null) || (!_db.ExistModule((Guid)id)))
            {
                return new NotFoundMvc.NotFoundViewResult();
            }
            Module module = _db.Module((Guid)id);
            if ((DateTime.Now < module.DateBeginTesting) || (DateTime.Now >= module.DateEndTesting))
                return View("NotInTestTime", module);

            if (!_db.IsStartModuleTest((Guid)id, User.Identity.Name)) // чи користувач починав тестування з цього модуля 
            {// не починав
                List<Test> tests = _db.ModuleTests((Guid)id);
                if (tests.Count == 0) return View("TestingNotPrepared", module);
                if (module.CountQuestions != null) //якщо null то додаються всі тести
                {
                    TestHelper.FilterTests(ref tests, (int)module.CountQuestions);
                }
                _db.AddNotShowedTests(tests, (Guid)id, User.Identity.Name);
                DateTime endTime = new DateTime();
                endTime = DateTime.Now.AddMinutes(module.TimeForPassTest);
                _db.AddUserModules(new UserModule { UserModuleId = Guid.NewGuid(), ModuleId = id, UserId = _db.GetUserId(User.Identity.Name), Passed = null, EndTime = endTime, StartTime = DateTime.Now });
                return View(NextNotShowedTest((Guid)id, User.Identity.Name, endTime.Subtract(DateTime.Now)));
            }
            UserModule previousResult = _db.UserModule((Guid)id, User.Identity.Name);
            if (previousResult.Passed != null) // модуль вже зданий
                return View("CalcModuleResults", previousResult);
            // уже починав тестування і не закінчив його
            // якщо час закінчився то рахуємо результат
            if (previousResult.EndTime.CompareTo(DateTime.Now) < 0)
                return RedirectToAction("CalcModuleResults", new { id = id });
            return View(NextNotShowedTest((Guid)id, User.Identity.Name, previousResult.EndTime.Subtract(DateTime.Now)));
        }

        [HttpPost]
        public ActionResult PassModule(Guid? moduleId, Guid? testId)
        {
            if ((moduleId == null) || (testId == null) || (!_db.ExistModule((Guid)moduleId)) || (!_db.ExistTest((Guid)testId)))
            {
                return new NotFoundMvc.NotFoundViewResult();
            }
            // PassTestModel { PassedModule = _db.Module(moduleId), TestAndAnswers = new TestAndAnswersModel { Test = _db.Test((Guid)answerforPass.TestId), AnswerList = _db.TestAnswers((Guid)answerforPass.TestId) } };
            List<Answer> answers = _db.TestAnswers((Guid)testId);
            bool isChecked;
            foreach (Answer answer in answers)
                if ((Request.Form.GetValues(answer.AnswerId.ToString())[0] != null) && (Boolean.TryParse(Request.Form.GetValues(answer.AnswerId.ToString())[0], out isChecked) == true))
                {
                    if (isChecked) _db.AddUserAnswer((Guid)testId, (Guid)moduleId, User.Identity.Name, answer.AnswerId);
                }
            if (_db.IsAllShowedModuleUserTests((Guid)moduleId, User.Identity.Name))
                return RedirectToAction("CalcModuleResults", new { id = moduleId });
            UserModule previousResult = _db.UserModule((Guid)moduleId, User.Identity.Name);
            if (previousResult.EndTime.CompareTo(DateTime.Now) < 0)
                return RedirectToAction("CalcModuleResults", new { id = moduleId });
            return View("PassModule", NextNotShowedTest((Guid)moduleId, User.Identity.Name, previousResult.EndTime.Subtract(DateTime.Now)));
        }


        [HttpGet]
        public ActionResult CalcModuleResults(Guid? id)
        {
            if ((id == null) || (!_db.ExistModule((Guid)id)))
            {
                return new NotFoundMvc.NotFoundViewResult();
            }
            if (!_db.IsPassedTest((Guid)id, User.Identity.Name))
            {
                _db.MarkModuleTestsShowed((Guid)id, User.Identity.Name);
                _db.UpdateUserModule((Guid)id, User.Identity.Name, _db.CalcUserModuleResults((Guid)id, User.Identity.Name));
            }
            UserModule model = _db.UserModule((Guid)id, User.Identity.Name);
            ViewBag.ModuleName = _db.ModuleName((Guid)id);
            ViewBag.ModulePoints = (int)_db.CalcCourseResult((float)model.Passed,model.Module.MaxPoints);
            return View(model);
        }



        private PassTestModel NextNotShowedTest(Guid moduleId, string username, TimeSpan timeToResolve)
        {
            List<Guid> testsId = _db.GetNotShowedTestsId(moduleId, username); // вибираємо всі тести з модуля що не показані користувачу
            int countQuestions = testsId.Count;
            Random rand = new Random();
            Guid testIdForPass = testsId[rand.Next(testsId.Count)]; // вибираємо один з тестів
            _db.MarkShowedTest(testIdForPass, moduleId, User.Identity.Name); // позначаємо обраний тест як показаний
            return new PassTestModel { PassedModule = _db.Module(moduleId), 
                                       TestAndAnswers = new TestAndAnswersModel { Test = _db.Test(testIdForPass), AnswerList = _db.TestAnswers(testIdForPass) }, 
                                       TimeToResolve = timeToResolve, 
                                       CountQuestions=countQuestions };
        }


    }
}
