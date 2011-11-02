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
            return View(db.UserLessons(User.Identity.Name)); 
        }


        public ActionResult Lesson(Guid id_mod)
        {
            ViewBag.ModuleId = id_mod;
            return View(db.GetLessonsByModule(id_mod));
        }

        public ActionResult AllMyLesson()
        {
            return View(db.GetLessonsByTeacherId(db.GetUser(User.Identity.Name).UserId));
        }

        public ActionResult Modules(Guid courseId)
        {
            ViewBag.CourseId = courseId;
            return View(db.GetModulesByCourseId(courseId));
        }

        public ActionResult MyCourses()
        {
            return View(db.GetCoursesByTeacherId(db.GetUser(User.Identity.Name).UserId));
        }

        [HttpGet]
        public ActionResult CreateModule(Guid courseId)
        {
            ViewBag.CourseId = courseId;
            return View();
        }
        [HttpPost]
        public ActionResult CreateModule(Module obj, Guid courseId)
        {
            obj.ModuleId = Guid.NewGuid();
            obj.CourseId = courseId;
            db.AddModule(obj);
            return RedirectToAction("Modules", new { courseid = courseId });
        }

        [HttpGet]
        public ActionResult DetailsModule(Guid id)
        {
           return View(db.GetModulesByID(id));
        }

        [HttpGet]
        public ActionResult DetailsLesson(Guid id)
        {
            return View(db.GetLessonByID(id));
        }

              [HttpGet]
        public ActionResult CreateLesson(Guid mod_id, string path)
        {
            Lesson obj = new Lesson();
            obj.Text = path;

            ViewBag.ModuleId = mod_id;
            return View(obj);
        }
        [HttpPost]
        public ActionResult CreateLesson(Lesson obj, Guid mod_id)
        {
            obj.ModuleId = mod_id;
            obj.LessonId = Guid.NewGuid();
            obj.UserId = db.GetUser(User.Identity.Name).UserId;
           if (obj.UserId==db.GetCourse(db.GetModulesByID(obj.ModuleId).CourseId).UserId) obj.IsAcceptMainTeacher = true;
            obj.Publication = DateTime.Now;
            db.AddLesson(obj);
            return RedirectToAction("Lesson", new { id_mod = mod_id });
        }

        [HttpGet]

        public ActionResult DeleteLesson(Guid id)
        {

            Guid mod_id = db.GetLessonByID(id).ModuleId;
            db.DeleteLesson(id);
            return RedirectToAction("Lesson", new { id_mod = mod_id });
        }


        [HttpGet]
        public ActionResult EditLesson(Guid id)
        {
            ViewBag.Modules = db.GetModulesByCourseId(db.GetModulesByID(db.GetLessonByID(id).ModuleId).CourseId);
            return View(db.GetLessonByID(id));
        }

        [HttpPost]
        public ActionResult EditLesson(Lesson obj)
        {
            Lesson old = db.GetLessonByID(obj.LessonId);
            UpdateModel(old);
            db.Save();
            Guid mod_id = obj.ModuleId;
            return RedirectToAction("Lesson", new { id_mod = mod_id });
        }


        [HttpGet]
        public ActionResult DeleteModule(Guid id)
        {
            Guid course_id = db.GetModulesByID(id).CourseId;
            db.DeleteModule(id);
            return RedirectToAction("Modules", new { courseId = course_id });
        }


        [HttpGet]
        public ActionResult EditModule(Guid id)
        {
            return View(db.GetModulesByID(id));
        }

        [HttpPost]
        public ActionResult EditModule(Module obj)
        {
            Module old = db.GetModulesByID(obj.ModuleId);
            UpdateModel(old);
            db.Save();
            Guid course_id = db.GetModulesByID(obj.ModuleId).CourseId;
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
            string fileName="";
            string moduleId = Request.Form["mod_id"];
            ViewBag.ModuleId = moduleId;
            foreach (string inputTagName in Request.Files)
            {
                HttpPostedFileBase file = Request.Files[inputTagName];
                if (file.ContentLength > 0)
                {
                    string fileType = System.IO.Path.GetExtension(file.FileName);
                    fileName = Guid.NewGuid().ToString()+fileType;
                    string filePath = Path.Combine(HttpContext.Server.MapPath("../Uploads"), fileName);
                    file.SaveAs(filePath);
                    
                    return RedirectToAction("CreateLesson", new { mod_id = moduleId, path = fileName });
                }
            }
            return View();
        }
    }
}

