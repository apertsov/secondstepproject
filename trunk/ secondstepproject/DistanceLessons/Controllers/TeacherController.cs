using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DistanceLessons.Models;
using DistanceLessons.Controllers;
using System.IO;

namespace DistanceLessons.Controllers
{

    public class TeacherController : Controller
    {
        DataEntitiesManager db;
        //
        // GET: /Teacher/
        public TeacherController()
            : base()
        {
             db= new DataEntitiesManager();
        }

        public ActionResult Index()
        {
            return View(db.GetLessonsByTeacherId(db.GetUserId(User.Identity.Name)));
        }
        [HttpGet]
        public ActionResult EditLessonTests(Guid lessonId)
        {
            List<TestAndAnswersModel> testAndAnswersList = new List<TestAndAnswersModel>();
            List<Test> testList = db.LessonTests(lessonId);
            foreach (var test in testList)
                testAndAnswersList.Add(new TestAndAnswersModel { Test=test, AnswerList=db.TestAnswers(test.TestId) });
            return View(testAndAnswersList);
        }
        public ActionResult Lesson(Guid id_mod)
        {
            DataEntitiesManager A = new DataEntitiesManager();
            ViewBag.ModuleId = id_mod;
            return View(A.GetLessonsByModule(id_mod));
        }

        public ActionResult AllMyLesson()
        {
            DataEntitiesManager A = new DataEntitiesManager();
            User user = A.GetUser(User.Identity.Name);
            return View(A.GetLessonsByTeacherId(user.UserId));
        }
        public ActionResult Modules(Guid courseId)
        {
            DataEntitiesManager A = new DataEntitiesManager();
            ViewBag.CourseId = courseId;
            return View(A.GetModulesByCourseId(courseId));
        }

        public ActionResult MyCourses()
        {
            DataEntitiesManager A = new DataEntitiesManager();
            User user = A.GetUser(User.Identity.Name);
            return View(A.GetCoursesByTeacherId(user.UserId));

        }

        [HttpGet]
        public ActionResult CreateModule(Guid courseId)
        {
            DataEntitiesManager A = new DataEntitiesManager();
            ViewBag.CourseId = courseId;
            return View();
        }
        [HttpPost]
        public ActionResult CreateModule(Module obj, Guid courseId)
        {
            DataEntitiesManager A = new DataEntitiesManager();

            obj.ModuleId = Guid.NewGuid();
            obj.CourseId = courseId;
            A.AddModule(obj);
            return RedirectToAction("Modules", new { courseid = courseId });
        }

        [HttpGet]
        public ActionResult DetailsModule(Guid id)
        {
            DataEntitiesManager A = new DataEntitiesManager();
            return View(A.GetModulesByID(id));
        }

        [HttpGet]
        public ActionResult DetailsLesson(Guid id)
        {
            DataEntitiesManager A = new DataEntitiesManager();
            return View(A.GetLessonByID(id));
        }
        [HttpGet]
        public ActionResult CreateLesson(Guid mod_id)
        {
            DataEntitiesManager A = new DataEntitiesManager();
            ViewBag.ModuleId = mod_id;
            return View();
        }
        [HttpPost]
        public ActionResult CreateLesson(Lesson obj, Guid mod_id)
        {
            DataEntitiesManager A = new DataEntitiesManager();

            obj.LessonId = Guid.NewGuid();
            obj.Publication = DateTime.Now;

            obj.ModuleId = mod_id;
            A.AddLesson(obj);
            return RedirectToAction("Lesson", new { id_mod = mod_id });
        }

        [HttpGet]
        public ActionResult DeleteLesson(Guid id)
        {
            DataEntitiesManager A = new DataEntitiesManager();
            Guid mod_id = A.GetLessonByID(id).ModuleId;
            A.DeleteLesson(id);
            return RedirectToAction("Lesson", new { id_mod = mod_id });
        }


        [HttpGet]
        public ActionResult EditLesson(Guid id)
        {
            DataEntitiesManager A = new DataEntitiesManager();

            ViewBag.Modules = A.GetModulesByCourseId(A.GetModulesByID(A.GetLessonByID(id).ModuleId).CourseId);
            return View(A.GetLessonByID(id));
        }

        [HttpPost]
        public ActionResult EditLesson(Lesson obj)
        {
            DataEntitiesManager A = new DataEntitiesManager();
            Lesson old = A.GetLessonByID(obj.LessonId);
            UpdateModel(old);
            A.Save();
            Guid mod_id = obj.ModuleId;
            return RedirectToAction("Lesson", new { id_mod = mod_id });
        }


        [HttpGet]
        public ActionResult DeleteModule(Guid id)
        {
            DataEntitiesManager A = new DataEntitiesManager();
            Guid course_id = A.GetModulesByID(id).CourseId;
            A.DeleteModule(id);
            return RedirectToAction("Modules", new { courseId = course_id });
        }


        [HttpGet]
        public ActionResult EditModule(Guid id)
        {
            DataEntitiesManager A = new DataEntitiesManager();
            return View(A.GetModulesByID(id));
        }

        [HttpPost]
        public ActionResult EditModule(Module obj)
        {
            DataEntitiesManager A = new DataEntitiesManager();
            Module old = A.GetModulesByID(obj.ModuleId);
            UpdateModel(old);
            A.Save();
            Guid course_id = A.GetModulesByID(obj.ModuleId).CourseId;
            return RedirectToAction("Modules", new { courseId = course_id });
        }



        public ActionResult UploadLesson(Guid moduleId)
        {
            ViewBag.ModuleId = moduleId;
            return View();

        }

        public ActionResult ShowFile(string filename)
        {
            FileRepository file = new FileRepository();
            FileDescription lect = new FileDescription();
            lect = file.GetFileByName(filename);
            string completeURL = "../Uploads/" + lect.WebPath;

            return Redirect(completeURL);

        }
        public ActionResult Upload()
        {

            foreach (string inputTagName in Request.Files)
            {
                HttpPostedFileBase file = Request.Files[inputTagName];

                // хотіла змінити назву файлу, шоб вона була унікальна, але файл доступний лише для читання
                /*   string fileType = System.IO.Path.GetExtension(file.FileName);
                   Guid newName = Guid.NewGuid();
                   file.FileName = (newName.ToString() + fileType);*/
                if (file.ContentLength > 0)
                {
                    ViewBag.Path = Path.GetFileName(file.FileName);
                    string filePath = Path.Combine(HttpContext.Server.MapPath("../Uploads")
                        , Path.GetFileName(file.FileName));
                    file.SaveAs(filePath);

                }
            }

            ViewBag.ModuleId = Request.Form["mod_id"];
            return View();
        }
    }
}

