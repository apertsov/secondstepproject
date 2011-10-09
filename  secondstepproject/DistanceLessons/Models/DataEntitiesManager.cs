using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace DistanceLessons.Models
{
    public class DataEntitiesManager
    {
        private dbEntities _distancel { get; set; }

        public DataEntitiesManager()
        {
            _distancel = new dbEntities();
        }

        //example of using this method's:

        //in namespace:
        //using DistanceLessons.Models;

        //in DataEntitesManager :
        //(CLASS-MODEL)
        //      public class UC
        //      {
        //          public Guid id { get; set; }
        //          public string user { get; set; }
        //          public string course { get; set; }
        //      }

        //in DataEntitiesManager :
        //(QUERY METHOD's)
        //      public List<UC> UCs2()
        //      {
        //          System.Guid g = new Guid("5d94f0c6-4fa4-472e-ab81-9ce251e35b73");
        //          var Query =
        //              (
        //              from uc in GetUserCourseList()
        //              from u in GetUserList()
        //              from c in GetCourseList()
        //              where uc.UserId == u.UserId
        //              select new UC
        //              {
        //                  id = uc.UserCourseId,
        //                  user = u.Login,
        //                  course = c.Title
        //              }
        //              ).ToList<UC>();

        //          List<UC> testik = new List<UC>();
        //          foreach (var i in Query)
        //              testik.Add(i);

        //          return testik;
        //      }

        //in controll :
        //            DataEntitiesManager _distancel = new DataEntitiesManager();
        //            var t = _distancel.UCs2();
        //            ViewBag.qwe = t;

        //in view : (ПЕРЕВІРИТИ ПРОСТІР ІМЕН ПЕРЕД КОПІПАСТОМ)
        //          <ol>
        //              @foreach (DistanceLesson.Models.DataEntitiesManager.UC k in ViewBag.qwe)
        //                  {<li>@k.id - @k.user - @k.course</li>}
        //          </ol>


        //===========================================
        //>>>CLASS-MODEL TABLE FOR RESULT QUERY
        //===========================================
        //Courses_+String(Category,User)
        public class RQCourses
        {
            public Guid id { get; set; }
            public string title { get; set; }
            public string description { get; set; }
            public string category { get; set; }
            public string teacher { get; set; }
            public int access { get; set; }
        }

        public class RQTeachers
        {
            public Guid id { get; set; }
            public string login { get; set; }
            public string first { get; set; }
            public string last { get; set; }
            public string mid { get; set; }
            public string course { get; set; }
        }

        public class RQLessons
        {
            public Guid id { get; set; }
            public string title { get; set; }
            public string text { get; set; }
        }

        //in progress... =)
        //===========================================
        //>>>QUERY METHOD's
        //===========================================
        
        //Всі курси - айді, Назва курсу, опис курсу, категорія, викладач(логін), доступність курсу
        public List<RQCourses> QCourses()
        {
            var Query =
                (
                from cou in GetCourseList()
                from u in GetUserList()
                from cat in GetCategoryList()
                where cou.CategoryId==cat.CategoryId && cou.UserId==u.UserId
                orderby cou.Title
                select new RQCourses
                {
                    id = cou.CourseId,
                    title = cou.Title,
                    description = cou.Description,
                    category = cat.Category1,
                    teacher = u.Login,
                    access = cou.Access
                }
                ).ToList<RQCourses>();

            List<RQCourses> lst = new List<RQCourses>();
            foreach (var i in Query)
                lst.Add(i);

            return lst;
        }

        //Всі викладачі - айді, логін, П.І.П, курс що читає
        public List<RQTeachers> QTeachers()
        {
            var Query =
                (
                from cou in GetCourseList()
                from u in GetUserList()
                from i in GetInfoList()
                where u.UserId==i.UserId && u.UserId==cou.UserId && u.Role==1
                orderby i.FirstName
                select new RQTeachers
                {
                    id=u.UserId,
                    login=u.Login,
                    first=i.FirstName,
                    last=i.LastName,
                    mid=i.MidName,
                    course=cou.Title
                }
                ).ToList<RQTeachers>();

            List<RQTeachers> lst = new List<RQTeachers>();
            foreach (var i in Query)
                lst.Add(i);

            return lst;
        }

        //Лекції певного курсу - айді, назва лекції, ссилка(текст) лекції
        public List<RQLessons> QLessons(string Course)
        {
            var Query =
                (
                from l in GetLessonList()
                from c in GetCourseList()
                where l.CourseId==c.CourseId && c.Title==Course
                orderby l.Title
                select new RQLessons
                {
                    id=l.LessonId,
                    title=l.Title,
                    text=l.Text
                }
                ).ToList<RQLessons>();

            List<RQLessons> lst = new List<RQLessons>();
            foreach (var i in Query)
                lst.Add(i);

            return lst;
        }


        //in progress... =)
        //===========================================
        //>>>METHOD's THAT RETURN LIST OF MODEL-TABLE
        //===========================================
        public List<Answer> GetAnswerList()
        {
            return _distancel.Answers.ToList<Answer>();
        }

        public List<Category> GetCategoryList()
        {
            return _distancel.Categories.ToList<Category>();
        }

        public List<Contact> GetContactList()
        {
            return _distancel.Contacts.ToList<Contact>();
        }

        public List<Cours> GetCourseList()
        {
            return _distancel.Courses.ToList<Cours>();
        }

        public List<Information> GetInfoList()
        {
            return _distancel.Informations.ToList<Information>();
        }

        public List<Lesson> GetLessonList()
        {
            return _distancel.Lessons.ToList<Lesson>();
        }

        public List<Message> GetMessageList()
        {
            return _distancel.Messages.ToList<Message>();
        }

        public List<ModuleLesson> GetModuleLessonList()
        {
            return _distancel.ModuleLessons.ToList<ModuleLesson>();
        }

        public List<Module> GetModuleList()
        {
            return _distancel.Modules.ToList<Module>();
        }

        public List<News> GetNewsList()
        {
            return _distancel.News.ToList<News>();
        }

        public List<Test> GetTestList()
        {
            return _distancel.Tests.ToList<Test>();
        }

        public List<UserCours> GetUserCourseList()
        {
            return _distancel.UserCourses.ToList<UserCours>();
        }

        public List<UserModule> GetUserModuleList()
        {
            return _distancel.UserModules.ToList<UserModule>();
        }

        public List<User> GetUserList()
        {
            return _distancel.Users.ToList<User>();
        }
    }
}