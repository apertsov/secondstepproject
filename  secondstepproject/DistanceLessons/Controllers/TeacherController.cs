using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DistanceLessons.Models;

namespace DistanceLessons.Controllers
{

    public class TeacherController : Controller
    {
        DataEntitiesManager repository;
        //
        // GET: /Teacher/
        public TeacherController()
            : base()
        {
             repository= new DataEntitiesManager();
        }

        public ActionResult Index()
        {
            return View(repository.UserLessons(User.Identity.Name));
        }
        [HttpGet]
        public ActionResult EditLessonTests(Guid lessonId)
        {
            List<TestAndAnswersModel> testAndAnswersList = new List<TestAndAnswersModel>();
            List<Test> testList = repository.LessonTests(lessonId);
            foreach (var test in testList)
                testAndAnswersList.Add(new TestAndAnswersModel { Test=test, AnswerList=repository.TestAnswers(test.TestId) });
            return View(testAndAnswersList);
        }

    }
}
