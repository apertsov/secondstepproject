using System;
using System.Collections.Generic;
using System.Linq;

namespace DistanceLessons.Models
{
    public partial class DataEntitiesManager
    {
        public List<Cours> GetCourseList()
        {
            return _db.Courses.ToList<Cours>();
        }

        public Cours GetCourse(Guid id)
        {
            return _db.Courses.SingleOrDefault(c => c.CourseId == id);
        }

        public void DeleteCourse(Guid id)
        {
            var cat = _db.Courses.SingleOrDefault(c => c.CourseId == id);
            _db.DeleteObject(cat);
            _db.SaveChanges();
        }

        public void AddCours(Cours obj)
        {
            _db.Courses.AddObject(obj);
            _db.SaveChanges();
        }

        public List<CategoryCourse> GetValidCourses()
        {
            var coursesInLessons = (from lesson in _db.Lessons
                                    group lesson by lesson.CourseId into lessonGroups
                                    select lessonGroups.Key);
            var validCourses = (from courses in _db.Courses
                                from couesesId in coursesInLessons
                                join category in _db.Categories on courses.CategoryId equals category.CategoryId
                                orderby courses.Title
                                where courses.CourseId == couesesId
                                select new CategoryCourse { CourseTitle = courses.Title, CourseDescription = courses.Description, CategoryTitle = category.Category1 });
            return validCourses.ToList();
        }

        public List<Cours> GetCoursesByTeacherId(Guid UserId)
        {

            return (from courses in GetCourseList()
                    where courses.UserId == UserId
                    select courses).ToList();
        }


        public Dictionary<Guid, string> CoursesDictionaryFromCategory(Guid categoryId)
        {
            var categoryCourses = (from courses in GetCourseList()
                                   where courses.CategoryId == categoryId
                                   select new { courseId = courses.CourseId, title = courses.Title });
            Dictionary<Guid, string> coursesFromCategory = new Dictionary<Guid, string>();
            foreach (var course in categoryCourses)
                coursesFromCategory.Add(course.courseId, course.title);
            return coursesFromCategory;
        }

        public Dictionary<Guid, string> CourseDictionary(Guid courseId)
        {
            var course = (from courses in GetCourseList()
                          where courses.CourseId == courseId
                          select new { courseId = courses.CourseId, title = courses.Title }).FirstOrDefault();
            if (course == null) return null;
            Dictionary<Guid, string> courseDictionary = new Dictionary<Guid, string>();
            courseDictionary.Add(course.courseId, course.title);
            return courseDictionary;
        }

        public Guid CategoryIdFromCourseId(Guid courseId)
        {
            return (from courses in GetCourseList()
                    where courses.CourseId == courseId
                    select courses.CategoryId).FirstOrDefault();
        }

        public bool IsTeacherCourse(Guid courseId, string username)
        {
            return (from courses in GetCourseList()
                    from users in GetUserList()
                    where courses.CourseId == courseId && users.Login == username && courses.UserId == users.UserId
                    select courses).Count() > 0 ? true : false;
        }

    }
}