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

    }
}
