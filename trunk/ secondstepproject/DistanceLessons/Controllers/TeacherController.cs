using System;
using System.Web;
using System.Web.Mvc;
using DistanceLessons.Attributes;
using DistanceLessons.Models;
using System.IO;
using System.Collections.Generic;
using System.Linq;

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
            ViewBag.IsOwner = db.GetCourse(ViewBag.CourseId).UserId == db.GetUserId(User.Identity.Name) ? true : false;
            ViewBag.IsTeacher = db.GetUserRoleId(User.Identity.Name) == db.GetRoleId("Teacher") ? true : false; ;
         
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
            ViewBag.IsOwner= db.GetCourse(id).UserId==db.GetUserId(User.Identity.Name)?true:false;
            ViewBag.IsTeacher = db.GetUserRoleId(User.Identity.Name)==db.GetRoleId("Teacher") ? true : false;
            ViewBag.IsAdmin = db.GetUserRoleId(User.Identity.Name) == db.GetRoleId("Admin") ? true : false; 
            return View();
        }
         
        public ActionResult MyCourses()
        {
            return View(db.GetCoursesByTeacherId(db.GetUser(User.Identity.Name).UserId));
        }
       
        public ActionResult Courses()
        {
            return View(db.GetCourseList());
        }


        [HttpGet]
        public ActionResult CreateModule(Guid id)
        {
            ViewBag.CourseId = id;
            ViewBag.DateIsGood = true;
            return View();
        }
        [HttpPost]
        public ActionResult CreateModule(Module obj)
        {
            ViewBag.CourseId = obj.CourseId;
            obj.ModuleId = Guid.NewGuid();
            Guid courseId = obj.CourseId;
            bool dateOk=true;
            if (obj.DateBeginTesting > obj.DateEndTesting) dateOk = false;
            ViewBag.DateIsGood = dateOk;
            if (ModelState.IsValid && dateOk) db.AddModule(obj);

            else return View(obj);
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
            string[] filepart = path.Split('.');
            Guid res = Guid.NewGuid();
            Guid.TryParse(filepart[0], out res);
            obj.LessonId = res;
            obj.CourseId = courseId;
            if ((mod_id != null) && (mod_id != Guid.Empty)) obj.ModuleId = mod_id;
            ViewBag.ModuleId = mod_id;
            return View(obj);
        }
              [HttpPost]
              public ActionResult CreateLesson(Lesson obj)
              {

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
            else return RedirectToAction("EditLesson", new { id = old.LessonId });
     
            if (old.ModuleId!=null)
            return RedirectToAction("Lesson", new { id = old.ModuleId });
            else return RedirectToAction("Course", new { id = old.CourseId });
        }

        public ActionResult AcceptLesson(Guid id)
        {
            Lesson old = db.GetLessonByID(id);

            old.IsAcceptMainTeacher = true;
            db.Save();

            if (old.ModuleId != null)
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
            ViewBag.DateIsGood = true;
            return View(db.GetModulesByID(id));
        }

        [HttpPost]
        public ActionResult EditModule(Module obj)
        {
            Module old = db.GetModulesByID(obj.ModuleId);
            UpdateModel(old);
            bool dateOk = true;
            if (obj.DateBeginTesting > obj.DateEndTesting) dateOk = false;
            ViewBag.DateIsGood = dateOk;
 
            if (ModelState.IsValid && dateOk)
                db.Save();
            else return View(obj);
           
            return RedirectToAction("Course", new { id = obj.CourseId });
        }



        public ActionResult UploadLesson(Guid courseId, Guid moduleId,string act="create")
        {
            ViewBag.ModuleId = moduleId;
            ViewBag.CourseId = courseId;
            ViewBag.Action = act;
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
            string action = Request.Form["act"];
            ViewBag.ModuleId = moduleId;
            ViewBag.CourseId = course_Id;
            ViewBag.Action = action;
            foreach (string inputTagName in Request.Files)
            {
                HttpPostedFileBase file = Request.Files[inputTagName];
                ViewBag.Error = "Недопустимий розмір файлу (" + (file.ContentLength/1024/1024).ToString()+" Мб). Дозволений до " + (6000000/1024/1024).ToString()+" Мб";
                if ((file.ContentLength > 0) && (file.ContentLength < 6000000))
                {
                    string fileType = System.IO.Path.GetExtension(file.FileName);
                    ViewBag.Error = "Недопустиме розширення файлу " + fileType;

                    switch (fileType)
                    {
                        case ".pdf":
                        case ".doc":
                        case ".docx":
                        case ".txt":
                        case ".htm":
                        case ".html":
                        case ".mhtml":
                        case ".jpg":
                        case ".jpeg":
                        case ".bmp":
                        case ".rtf":
                        case ".xhtml":
                            {
                                Guid lesId;
                                string filePath;
                                if (Guid.TryParse(action, out lesId))
                                {
                                   Lesson les = db.GetLessonByID(lesId);
                                   fileName = lesId.ToString() + fileType;
                                   les.Text=fileName;
                                   db.Save();
                                   filePath = Path.Combine(HttpContext.Server.MapPath("../Uploads"), fileName);
                                   file.SaveAs(filePath);
                                   return RedirectToAction("EditLesson", new {id=lesId});
                                }
                                fileName = Guid.NewGuid().ToString() + fileType;
                                filePath = Path.Combine(HttpContext.Server.MapPath("../Uploads"), fileName);
                                file.SaveAs(filePath);
                                return RedirectToAction("CreateLesson", new { courseId = course_Id, mod_id = moduleId, path = fileName });

                            }
                    }
                }
             }
           
            return View();
        }
       
    }
}

