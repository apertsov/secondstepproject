using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DistanceLessons.Models
{
    public partial class DataEntitiesManager
    {
        public List<Lesson> GetLessonList()
        {
            return _db.Lessons.ToList<Lesson>();
        }

        public List<RQLessons> QLessons(string Course)
        {
            var Query =
                (
                from l in GetLessonList()
                from c in GetCourseList()
                where l.CourseId == c.CourseId && c.Title == Course
                orderby l.Title
                select new RQLessons
                {
                    id = l.LessonId,
                    title = l.Title,
                    text = l.Text
                }
                ).ToList<RQLessons>();

            List<RQLessons> lst = new List<RQLessons>();
            foreach (var i in Query)
                lst.Add(i);

            return lst;
        }
    }
}