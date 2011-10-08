using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Distance_Lesson.Models
{
    public class DataEntitiesManager
    {
        private dbEntities _distancel { get; set; }
        
        public DataEntitiesManager()
        {
            _distancel = new dbEntities();
        }

        //example of using this method's:
        
        //in controll:
        //ViewBag.qwe = UCs();

        //in view:
        //<ol>
        //    @foreach (Distance_Lesson.Models.DataEntitiesManager k in ViewBag.qwe)
        //    {<li>@k.id - @k.user - @k.course</li>}
        //</ol>


//===========================================
//>>>CLASS-MODEL TABLE FOR RESULT QUERY
//===========================================
        public class UC
        {
            public Guid id { get; set; }
            public string user { get; set; }
            public string course { get; set; }
        }

        //in progress... =)
//===========================================
//>>>QUERY METHOD's
//===========================================
        public List<UC> UCs()
        {
            var Query = 
                (
                from uc in _distancel.UserCourses
                from i in _distancel.Informations
                where uc.UserId == i.UserId
                select new UC { id = uc.UserCourseId,
                                user = i.LastName + " " + i.FirstName + " " + i.MidName,
                                course = uc.Cours.Title }
                ).ToList(); 
            
            return Query;
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