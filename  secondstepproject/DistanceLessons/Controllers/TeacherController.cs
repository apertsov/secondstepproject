using System;
using System.Web;
using System.Web.Mvc;
using DistanceLessons.Attributes;
using DistanceLessons.Models;
using System.IO;

namespace DistanceLessons.Controllers
{
    [Localization]
    [Authorize(Roles = "Admin, Teacher")]
    public class TeacherController : Controller
    {
        private DataEntitiesManager db;

        public TeacherController()
            : base()
        {
             db= new DataEntitiesManager();
        }

        public ActionResult Index()
        {
            return View(); 
        }


        public ActionResult Lesson(Guid id)
        {
            ViewBag.ModuleId = id;
            ViewBag.Module = db.GetModulesByID(id).Title;
            ViewBag.CourseId = db.GetModulesByID(id).CourseId;
            return View(db.GetLessonsByModule(id));
        }

        public ActionResult AllMyLesson()
        {
            return View(db.UserLessons(User.Identity.Name));
        }
      
       
        public ActionResult AllLessonsInCourse(Guid courseId)
        {
            ViewBag.CourseId = courseId;
            ViewBag.Course = db.GetCourse(courseId).Title;
            ViewBag.Lessons = db.GetLessonsByCourse(courseId);
            return View();
        }

        public ActionResult Course(Guid id)
        {
            ViewBag.CourseId = id;
            ViewBag.Course = db.GetCourse(id).Title;
            ViewBag.Modules = db.GetModulesByCourseId(id);
            ViewBag.OrphanLessons = db.GetOrphanLessonsByCourse(id);
            return View();
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
        public ActionResult CreateModule(Module obj)
        {
            obj.ModuleId = Guid.NewGuid();
            Guid courseId = obj.CourseId;
            if (ModelState.IsValid) db.AddModule(obj);
            else RedirectToAction("CreateModule", new { id = courseId });
            return RedirectToAction("Course", new { id = courseId });
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
        public ActionResult CreateLesson(Guid courseId, Guid mod_id, string path)
        {
            Lesson obj = new Lesson();
            obj.Text = path;
            obj.CourseId = courseId;
            if ((mod_id != null) && (mod_id != Guid.Empty)) obj.ModuleId = mod_id;
            ViewBag.ModuleId = mod_id;
            return View(obj);
        }
              [HttpPost]
              public ActionResult CreateLesson(Lesson obj)
              {

                  obj.LessonId = Guid.NewGuid();
                  obj.UserId = db.GetUser(User.Identity.Name).UserId;
                  if (obj.UserId == db.GetCourse(obj.CourseId).UserId) obj.IsAcceptMainTeacher = true;
                  obj.Publication = DateTime.Now;
                  if (ModelState.IsValid)
                      db.AddLesson(obj);
                  else
                  {
                      var mod = (obj.ModuleId != null) ? obj.ModuleId : Guid.Empty;
                      return RedirectToAction("CreateLesson", new { courseId = obj.CourseId, mod_id = mod, path = obj.Text });
                  }
                  if (obj.ModuleId==null)
                  return RedirectToAction("Course", new { id = obj.CourseId });
                  else return RedirectToAction("Lesson", new { id = obj.ModuleId });
              }

        [HttpGet]

        public ActionResult DeleteLesson(Guid id)
        {
            Lesson les=db.GetLessonByID(id);
            db.DeleteLesson(id);
            
            if( les.ModuleId!=null)
                return RedirectToAction("Lesson", new { id = les.ModuleId });
            else 
                return RedirectToAction("Course", new { id = les.CourseId });

        }


        [HttpGet]
        public ActionResult EditLesson(Guid id)
        {
           ViewBag.Modules = db.GetModulesByCourseId(db.GetLessonByID(id).CourseId);
           return View(db.GetLessonByID(id));
        }

        [HttpPost]
        public ActionResult EditLesson(Lesson obj)
        {
            Lesson old = db.GetLessonByID(obj.LessonId);
            UpdateModel(old);
            bool isChecked = false;
            if (Boolean.TryParse(Request.Form.GetValues("NoModule")[0], out isChecked) && isChecked)
                old.ModuleId = null;
            else
                old.ModuleId = obj.ModuleId;
            

            if (ModelState.IsValid)
            {
                db.Save();
            }
            else return RedirectToAction("EditModule", new { id = old.LessonId });
     
            if (old.ModuleId!=null)
            return RedirectToAction("Lesson", new { id = old.ModuleId });
            else return RedirectToAction("Course", new { id = old.CourseId });
        }

       
        [HttpGet]
        public ActionResult DeleteModule(Guid id)
        {
            Guid course_id = db.GetModulesByID(id).CourseId;
            db.DeleteModule(id);
            return RedirectToAction("Course", new { id = course_id });
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
            if (ModelState.IsValid)
                db.Save();
            else return View(obj.ModuleId);
           
            return RedirectToAction("Course", new { id = obj.CourseId });
        }



        public ActionResult UploadLesson(Guid courseId, Guid moduleId)
        {
            ViewBag.ModuleId = moduleId;
            ViewBag.CourseId = courseId;
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
            string fileName = "";
            string moduleId = Request.Form["mod_id"];
            string course_Id = Request.Form["course_id"];
            ViewBag.ModuleId = moduleId;
            ViewBag.CourseId = course_Id;
            foreach (string inputTagName in Request.Files)
            {
                HttpPostedFileBase file = Request.Files[inputTagName];
                if (file.ContentLength > 0)
                {
                    string fileType = System.IO.Path.GetExtension(file.FileName);
                    fileName = Guid.NewGuid().ToString() + fileType;
                    string filePath = Path.Combine(HttpContext.Server.MapPath("../Uploads"), fileName);
                    file.SaveAs(filePath);

                    return RedirectToAction("CreateLesson", new { courseId = course_Id, mod_id = moduleId, path = fileName });
                }
            }
            return View();
        }
       
    }
}

