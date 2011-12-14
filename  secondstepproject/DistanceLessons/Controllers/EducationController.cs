using System;
using System.Collections.Generic;
using System.Web.Mvc;
using DistanceLessons.Attributes;
using DistanceLessons.Models;

namespace DistanceLessons.Controllers
{
    [Localization]
    [Authorize(Roles = "Admin, Teacher, Student")]
    public class EducationController : Controller
    {
        private readonly DataEntitiesManager _db = new DataEntitiesManager();
        private int itemOnPage = 5;

        //
        // GET: /Education/

        public ActionResult Index()
        {
            return View(_db.GetCategoryListASC());
        }      

        public ActionResult category()
        {
            return PartialView("_Category", _db.GetCategoryListASC());
        }

        public ActionResult courses_list(Guid? id)
        {
            if ((id == null) || (!_db.ExistCategory((Guid)id)))
            {
                return new NotFoundMvc.NotFoundViewResult();
            }
            Session["CategoryId"] = (Guid)id;

            Session["page"] = 0;
            Session["itemsCount"] = _db.GetCourseByCategory_ListCount((Guid) Session["CategoryId"], 0).Count;
            Session["itemOnPage"] = itemOnPage;
            Session["withStatus"] = 0;

            return PartialView("_CoursesList",
                               _db.GetUserCourse((Guid) Session["CategoryId"], _db.GetUserId(User.Identity.Name), 0,
                                                 itemOnPage, 0));
        }

        public ActionResult courses(int withStatus, int numPage)
        {
            Session["page"] = numPage;
            Session["itemsCount"] = _db.GetCourseByCategory_ListCount((Guid) Session["CategoryId"], withStatus).Count;
            Session["itemOnPage"] = itemOnPage;
            Session["withStatus"] = withStatus;

            return PartialView("_Courses",
                               _db.GetUserCourse((Guid) Session["CategoryId"], _db.GetUserId(User.Identity.Name),
                                                 withStatus, itemOnPage, numPage));
        }

        public ActionResult SubscribeOnCourse(Guid? CourseId)
        {
            if ((CourseId == null) || (!_db.ExistCourse((Guid)CourseId)))
            {
                return new NotFoundMvc.NotFoundViewResult();
            }
            var subscribe = new UserCours();
            subscribe.UserCourseId = Guid.NewGuid();
            subscribe.CourseId = (Guid)CourseId;
            subscribe.UserId = _db.GetUserId(User.Identity.Name);
            _db.AddUserCourse(subscribe);
            _db.Save();

            return PartialView("_Courses",
                               _db.GetUserCourse((Guid) Session["CategoryId"], _db.GetUserId(User.Identity.Name),
                                                 (int) Session["withStatus"], itemOnPage, (int) Session["page"]));
        }

        public ActionResult DeleteSubscribeOnCourse(Guid? CourseId)
        {
            if ((CourseId == null) || (!_db.ExistCourse((Guid)CourseId)))
            {
                return new NotFoundMvc.NotFoundViewResult();
            }
            _db.DeleteUserCourse(_db.GetUserCourseId(_db.GetUserId(User.Identity.Name), (Guid)CourseId));
            return PartialView("_Courses",
                               _db.GetUserCourse((Guid) Session["CategoryId"], _db.GetUserId(User.Identity.Name),
                                                 (int) Session["withStatus"], itemOnPage, (int) Session["page"]));
        }

        public ActionResult ConfirmSubscribeOnCourse(Guid? CourseId, Guid? TeacherId)
        {
            if ((TeacherId==null) || (CourseId == null) || (!_db.ExistCourse((Guid)CourseId)) || (!_db.IsTeacherCourse((Guid)CourseId,(Guid)TeacherId)))
            {
                return new NotFoundMvc.NotFoundViewResult();
            }
            var confirm = new Message();
            confirm.MessageId = Guid.NewGuid();
            confirm.Message1 = "Користувач <a href=profile/info?user=" + User.Identity.Name + ">" +
                               _db.GetUser(User.Identity.Name).Login + "</a> подав заявку на підписання на Ваш курс - " +
                               _db.GetCourse((Guid)CourseId).Title + ".&nbsp;<a href=\"/profile/confirmsubscribe?CourId=" +
                               CourseId + "&amp;SubscribeUser=" + _db.GetUser(User.Identity.Name).UserId +
                               "&amp;MessId=" + confirm.MessageId +
                               "\">Затверджую</a>&nbsp;<a href=\"/profile/cancelsubscribe?CourId=" + CourseId +
                               "&amp;SubscribeUser=" + _db.GetUser(User.Identity.Name).UserId + "&amp;MessId=" +
                               confirm.MessageId + "\">Відмовляю</a>";
            confirm.DateOfSender = DateTime.Now;
            confirm.UserId_From = _db.GetUserId(User.Identity.Name);
            confirm.UserId_To = (Guid)TeacherId;
            confirm.Title = "Запит підписання на курс '" + _db.GetCourse((Guid)CourseId).Title + "'";

            _db.AddMessage(confirm);
            _db.AddMessage_status(_db.GetMessageStatus(confirm, _db.GetUser(confirm.UserId_To).Login), 10);
            _db.AddMessage_status(_db.GetMessageStatus(confirm, _db.GetUser(confirm.UserId_From).Login), 2);

            return PartialView("_Courses",
                               _db.GetUserCourse((Guid) Session["CategoryId"], _db.GetUserId(User.Identity.Name),
                                                 (int) Session["withStatus"], itemOnPage, (int) Session["page"]));
        }

        public ActionResult UserSubscribs()
        {
            var temp = new UserCourseAndCategoriesModel();
            temp.UserCourses = _db.GetUserCourseListByUser(_db.GetUserId(User.Identity.Name));
            temp.Categories = _db.GetCategoriesList(temp.UserCourses);
            temp.CoursesLessonsInModule = new List<Lesson>();
            temp.CoursesLessonsInNullModule = new List<Lesson>();
            List<Lesson> temp_lst = _db.GetLessonsListByCoursesList(temp.UserCourses);
            foreach (Lesson item in temp_lst)
            {
                if(item.ModuleId==null) temp.CoursesLessonsInNullModule.Add(item);
                else temp.CoursesLessonsInModule.Add(item);
            }
            temp.Modules = _db.GetModulesListByCoursesList(temp.CoursesLessonsInModule);

            return PartialView("_UserSubscribs", temp);
        }

        public ActionResult ModuleInfo(Guid? id)
        {
            if ((id == null) || (!_db.ExistModule((Guid)id)))
            {
                return new NotFoundMvc.NotFoundViewResult();
            }
            Module module = _db.Module((Guid)id);
            ViewBag.dbModuleQuestions = _db.ModuleTests((Guid)id).Count;
            if ((DateTime.Now < module.DateBeginTesting) || (DateTime.Now >= module.DateEndTesting))
                ViewBag.CanPass = false;
            else
                ViewBag.CanPass = true;
            return PartialView(_db.Module((Guid)id));
        }

        [HttpGet]
        public ActionResult ShowFile(string filename)
        {
            if (String.IsNullOrEmpty(filename))
            {
                return new NotFoundMvc.NotFoundViewResult();
            }
            FileRepository file = new FileRepository();
            FileDescription lect = new FileDescription();
            lect = file.GetFileByName(filename);
            string completeURL = "../Uploads/" + lect.WebPath;
            return Redirect(completeURL);
        }
    }
}