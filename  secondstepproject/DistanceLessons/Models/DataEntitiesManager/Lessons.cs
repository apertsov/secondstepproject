using System;
using System.Collections.Generic;
using System.Linq;

namespace DistanceLessons.Models
{
    public partial class DataEntitiesManager
    {
        public List<Lesson> GetLessonList()
        {
            return _db.Lessons.ToList();
        }

        public List<Lesson> UserLessons(string username)
        {
            var userId = new Guid(GetUserId(username).ToString());
            List<Lesson> result = (from l in GetLessonList()
                                   where l.UserId == userId
                                   select l).ToList<Lesson>();
            return result;
        }

        public Lesson GetLessonByID(Guid id)
        {
            return _db.Lessons.SingleOrDefault(c => c.LessonId == id);
        }


        public void DeleteLesson(Guid id)
        {
            Lesson cat = _db.Lessons.SingleOrDefault(c => c.LessonId == id);
            _db.DeleteObject(cat);
            Save();
        }

        public void AddLesson(Lesson obj)
        {
            _db.Lessons.AddObject(obj);
            Save();
        }

        public List<Lesson> GetLessonsByCourse(Guid courseId)
        {
            return (from lessons in _db.Lessons
                    where lessons.CourseId == courseId
                    orderby lessons.ModuleId , lessons.Title
                    select lessons).ToList();
        }

        public List<Lesson> GetOrphanLessonsByCourse(Guid courseId)
        {
            return (from lessons in _db.Lessons
                    where lessons.CourseId == courseId && lessons.ModuleId == null
                    select lessons).ToList();
        }

        public List<Lesson> GetLessonsByModule(Guid id_mod)
        {
            return (from lessons in _db.Lessons
                    where lessons.ModuleId == id_mod
                    select lessons).ToList();
        }

        public Lesson Lesson(Guid lessonId)
        {
            return (from lesson in GetLessonList()
                    where lesson.LessonId == lessonId
                    select lesson).FirstOrDefault();
        }

        public bool ExistLesson(Guid id)
        {
            return (from lesson in _db.Lessons
                    where lesson.LessonId == id
                    select lesson).FirstOrDefault() == null
                       ? false
                       : true;
        }

        public List<Lesson> GetLessonsListByCoursesList(List<UserCours> lst)
        {
            var temp = new List<Lesson>();
            List<Lesson> lessonsLst = GetLessonList();
            foreach (Lesson item in lessonsLst)
            {
                if ((lst.Find(m => m.CourseId == item.CourseId) != null))
                    temp.Add(item);
            }
            temp.Sort((a, b) => a.Title.CompareTo(b.Title));
            return temp;
        }

        public bool ExistLessonsInCategory(List<Lesson> lst, Guid courseId)
        {
            if ((lst.Find(m => m.CourseId == courseId) != null)) return true;
            return false;
        }
    }
}