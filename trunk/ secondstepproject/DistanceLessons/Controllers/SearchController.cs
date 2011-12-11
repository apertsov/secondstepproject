using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DistanceLessons.Attributes;
using DistanceLessons.Models;


namespace DistanceLessons.Controllers
{
    [Authorize]
    [Localization]
    public class SearchController : Controller
    {
        //
        // GET: /Search/
        private DataEntitiesManager _db = new DataEntitiesManager();

        [HttpGet]
        public ActionResult Course()
        {
            var course = _db.GetCourseList();
            return View(course);
        }

       [HttpPost]
        public ActionResult Course(string title, string description)
        {
            var course = _db.GetCourseList();

            if (!string.IsNullOrEmpty(title))
                course = course.Where(a => a.Title.ToUpper().Contains(title.ToUpper())).ToList();

            if (!string.IsNullOrEmpty(description))
                course = course.Where(a => a.Description.ToUpper().Contains(description.ToUpper())).ToList();


            return View(course);
        }

        [HttpGet]
       public ActionResult Lesson()
       {
           var course = _db.GetLessonList();
           return View(course);
       }

       [HttpPost]
       public ActionResult Lesson(string title, string description)
       {
           var lesson = _db.GetLessonList();

           if (!string.IsNullOrEmpty(title))
               lesson = lesson.Where(a => a.Title.ToUpper().Contains(title.ToUpper())).ToList();

           if (!string.IsNullOrEmpty(description))
               lesson = lesson.Where(a => a.Description != null && a.Description.ToUpper().Contains(description.ToUpper())).ToList();


           return View(lesson);
       }

       [HttpGet]
       public ActionResult UsersSearch()
       {
           var usersList = _db.GetUserList();

           ViewBag.RoleList = _db.GetRoleList();

           //Information i = new Information();
           
           return View(usersList);
       }

       [HttpPost]
       public ActionResult UsersSearch(string login, Guid role, bool isKnown, string name, string sureName)
       {
           var usersList = _db.GetUserList();
           var infoList = _db.GetInfoList();

           ViewBag.RoleList = _db.GetRoleList();

           if (!string.IsNullOrEmpty(login))
               usersList = usersList.Where(a => a.Login.ToUpper().Contains(login.ToUpper())).ToList();

           //if (!string.IsNullOrEmpty(role))
           if (!isKnown)
               usersList = usersList.Where(a => a.Role.RoleId.Equals(role)).ToList();

           if (!string.IsNullOrEmpty(name))
           {
               var usList = new List<User>();

               //usersList = usersList.Where(a => a.Informations.Where(b => b.FirstName.ToUpper().Contains(name.ToUpper()))).ToList();
               foreach (Information info in infoList)
               {
                   if (info.FirstName.ToUpper().Contains(name.ToUpper())) usList.Add(_db.GetUser(info.UserId));
               }
               return View(usList);
           }

           if (!string.IsNullOrEmpty(sureName))
           {
               var usList = new List<User>();

               //usersList = usersList.Where(a => a.Informations.Where(b => b.FirstName.ToUpper().Contains(name.ToUpper()))).ToList();
               foreach (Information info in infoList)
               {
                   if (info.FirstName.ToUpper().Contains(sureName.ToUpper())) usList.Add(_db.GetUser(info.UserId));
               }
               return View(usList);
           }

               return View(usersList);
       }
    }
}
