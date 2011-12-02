using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DistanceLessons.Attributes;
using DistanceLessons.Models;

namespace DistanceLessons.Controllers
{
    public class SearchController : Controller
    {
        //
        // GET: /Search/
        private DataEntitiesManager _db = new DataEntitiesManager();

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

           return View(usersList);
       }

       [HttpPost]
       public ActionResult UsersSearch(string login, string role)
       {
           var usersList = _db.GetUserList();

           if (!string.IsNullOrEmpty(login))
               usersList = usersList.Where(a => a.Login.ToUpper().Contains(login.ToUpper())).ToList();

           if (!string.IsNullOrEmpty(role))
               usersList = usersList.Where(a => a.Role.Name.ToUpper().Contains(role.ToUpper())).ToList();

           return View(usersList);
       }
    }
}
