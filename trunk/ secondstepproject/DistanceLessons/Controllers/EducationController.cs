using System;
using System.Web.Mvc;
using DistanceLessons.Attributes;
using DistanceLessons.Models;

namespace DistanceLessons.Controllers
{
    [Localization]
    public class EducationController : Controller
    {
        private int itemOnPage = 10;
        private DataEntitiesManager _db = new DataEntitiesManager();

        //
        // GET: /Education/

        public ActionResult Index()
        {
            return View(_db.GetCategoryListASC());
        }


        public ActionResult Courses()
        {
            return View(_db.GetValidCourses());
        }

        public ActionResult category()
        {
            return PartialView("_Category", _db.GetCategoryListASC());
        }

        public ActionResult courses_list(Guid id)
        {
            Session["CategoryId"] = id;

            Session["page"] = 0;
            Session["itemsCount"] = _db.GetCourseByCategory_ListCount((Guid)Session["CategoryId"], 0).Count;
            Session["itemOnPage"] = itemOnPage;
            Session["withStatus"] = 0;

            return PartialView("_CoursesList", _db.GetUserCourse((Guid)Session["CategoryId"], _db.GetUserId(User.Identity.Name), 0, itemOnPage, 0));
        }

        public ActionResult courses(int withStatus, int numPage)
        {
            Session["page"] = numPage;
            Session["itemsCount"] = _db.GetCourseByCategory_ListCount((Guid)Session["CategoryId"], withStatus).Count;
            Session["itemOnPage"] = itemOnPage;
            Session["withStatus"] = withStatus;

            return PartialView("_Courses",
                               _db.GetUserCourse((Guid)Session["CategoryId"], _db.GetUserId(User.Identity.Name), withStatus, itemOnPage, numPage));
        }

        public ActionResult SubscribeOnCourse(Guid CourseId)
        {
            UserCours subscribe = new UserCours();
            subscribe.UserCourseId = Guid.NewGuid();
            subscribe.CourseId = CourseId;
            subscribe.UserId = _db.GetUserId(User.Identity.Name);
            _db.AddUserCourse(subscribe);
            _db.Save();

            return PartialView("_Courses",
                               _db.GetUserCourse((Guid)Session["CategoryId"], _db.GetUserId(User.Identity.Name), (int)Session["withStatus"], itemOnPage, (int)Session["page"]));
        }

        public ActionResult DeleteSubscribeOnCourse(Guid CourseId)
        {
            _db.DeleteUserCourse(_db.GetUserCourseId(_db.GetUserId(User.Identity.Name), CourseId));
            return PartialView("_Courses",
                               _db.GetUserCourse((Guid)Session["CategoryId"], _db.GetUserId(User.Identity.Name), (int)Session["withStatus"], itemOnPage, (int)Session["page"]));
        }

        public ActionResult ConfirmSubscribeOnCourse(Guid CourseId, Guid TeacherId)
        {
            Message confirm = new Message();
            confirm.MessageId = Guid.NewGuid();
            confirm.Message1 = "Користувач <a href=profile/info?user=" + User.Identity.Name + ">" + _db.GetUser(User.Identity.Name).Login + "</a> подав заявку на підписання на Ваш курс - " + _db.GetCourse(CourseId).Title + ".&nbsp;<a href=\"/profile/confirmsubscribe?CourId=" + CourseId + "&amp;SubscribeUser=" + _db.GetUser(User.Identity.Name).UserId + "&amp;MessId=" + confirm.MessageId + "\">Затверджую</a>&nbsp;<a href=\"/profile/cancelsubscribe?CourId=" + CourseId + "&amp;SubscribeUser=" + _db.GetUser(User.Identity.Name).UserId + "&amp;MessId=" + confirm.MessageId + "\">Відмовляю</a>";
            confirm.DateOfSender = DateTime.Now;
            confirm.UserId_From = _db.GetUserId(User.Identity.Name);
            confirm.UserId_To = TeacherId;
            confirm.Title = "Запит підписання на курс '" + _db.GetCourse(CourseId).Title + "'";

            _db.AddMessage(confirm);
            _db.AddMessage_status(_db.GetMessageStatus(confirm, _db.GetUser(confirm.UserId_To).Login), 10);
            _db.AddMessage_status(_db.GetMessageStatus(confirm, _db.GetUser(confirm.UserId_From).Login), 2);

            return PartialView("_Courses",
                               _db.GetUserCourse((Guid)Session["CategoryId"], _db.GetUserId(User.Identity.Name), (int)Session["withStatus"], itemOnPage, (int)Session["page"]));
        }

        public ActionResult UserSubscribs()
        {
            UserCourseAndCategoriesModel temp = new UserCourseAndCategoriesModel();
            temp.UserCourses = _db.GetUserCourseListByUser(_db.GetUserId(User.Identity.Name));
            temp.Categories = _db.GetCategoriesList(temp.UserCourses);
            temp.CoursesLessons = _db.GetLessonsListByCoursesList(temp.UserCourses);

            return PartialView("_UserSubscribs", temp);
        }
    }
}
