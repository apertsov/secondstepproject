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

        public List<Lesson> UserLessons(string username)
        {
            Guid userId = GetUserId(username);
            return (from lessons in _db.Lessons
                   where lessons.UserId == userId
                   select lessons).ToList();
        }
        
        public Lesson GetLessonByID(Guid id)
        {
            return _db.Lessons.SingleOrDefault(c => c.LessonId == id);
        }


        public void DeleteLesson(Guid id)
        {
            var cat = _db.Lessons.SingleOrDefault(c => c.LessonId == id);
            _db.DeleteObject(cat);
            Save();
        }

        public void AddLesson(Lesson obj)
        {
            _db.Lessons.AddObject(obj);
            Save();
        }

        public List<RQLessons> QLessons(string Course)
        {
            var Query =
                (
                  from l in GetLessonList()
                  from c in GetCourseList()
                  from m in GetModuleList()
                  where c.Title == Course && m.CourseId == c.CategoryId && l.ModuleId == m.ModuleId
                  orderby l.Title
                  select new RQLessons
                  {
                      id = l.LessonId,
                      title = l.Title,
                      text = l.Text,
                      module = m.Title,
                      descripton = l.Description
                  }
                ).ToList<RQLessons>();

            List<RQLessons> lst = new List<RQLessons>();
            foreach (var i in Query)
                lst.Add(i);

            return lst;
        }

        public List<RQLessons> GetLessonsByCourse(string Course)
        {
            var Query =
                (
                  from l in GetLessonList()
                  from c in GetCourseList()
                  from m in GetModuleList()
                  where c.Title == Course && m.CourseId == c.CategoryId && l.ModuleId == m.ModuleId
                  orderby l.Title
                  select new RQLessons
                  {
                      id = l.LessonId,
                      title = l.Title,
                      text = l.Text,
                      module = m.Title,
                      descripton = l.Description
                  }
                ).ToList<RQLessons>();

            List<RQLessons> lst = new List<RQLessons>();
            foreach (var i in Query)
                lst.Add(i);

            return lst;
        }

        public List<Lesson> GetLessonsByModule(Guid id_mod)
        {
            var Query =
                (
                from l in GetLessonList()
                where l.ModuleId == id_mod
                orderby l.Title
                select new Lesson
                {
                    LessonId = l.LessonId,
                    Title = l.Title,
                    Text = l.Text,
                    Description = l.Description
                }
                ).ToList<Lesson>();

            List<Lesson> lst = new List<Lesson>();
            foreach (var i in Query)
                lst.Add(i);

            return lst;
        }

        public List<Lesson> GetLessonsByTeacherId(Guid UserId)
        {
            var Query =
                (
                from l in GetLessonList()
                from c in GetCourseList()
                from m in GetModuleList()
                where c.UserId == UserId && m.CourseId == c.CategoryId && l.ModuleId == m.ModuleId
                orderby l.ModuleId
                select new Lesson
                {
                    LessonId = l.LessonId,
                    Title = l.Title,
                    Publication = l.Publication,
                    ModuleId = l.ModuleId,
                    Text = l.Text,
                    Description = l.Description
                }

                ).ToList<Lesson>();

            List<Lesson> lst = new List<Lesson>();
            foreach (var i in Query)
                lst.Add(i);

            return lst;
        }

        public bool IsLesson(Guid id)
        {
            return (from lesson in _db.Lessons
                    where lesson.LessonId==id
                    select lesson).FirstOrDefault()==null?false:true;
        }
    }
}