using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

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
            var coursesInLessons=(from lesson in _db.Lessons
                       group lesson by lesson.CourseId into lessonGroups
                        select lessonGroups.Key);
            var validCourses = (from courses in _db.Courses
                     from couesesId in coursesInLessons 
                     join category in _db.Categories on courses.CategoryId equals category.CategoryId
                     orderby courses.Title
                     where courses.CourseId==couesesId
                                select new CategoryCourse { CourseTitle = courses.Title, CourseDescription = courses.Description, CategoryTitle = category.Category1 });
            return validCourses.ToList();
        }

        /*
        public List<RQCourses> QCourses()
        {
            var Query =
                (
                from cou in GetCourseList()
                from u in GetUserList()
                from cat in GetCategoryList()
                where cou.CategoryId == cat.CategoryId && cou.UserId == u.UserId
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

        public List<RQCourses> QCoursesSeak(string Course)
        {
            var Query =
                (
                from cou in GetCourseList()
                from u in GetUserList()
                from cat in GetCategoryList()
                where cou.CategoryId == cat.CategoryId && cou.UserId == u.UserId && cou.Title.ToLower().IndexOf(Course.ToLower()) > -1
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
        public List<RQCourses> GetCoursesByTeacherId(Guid UserId)
        {
            var Query =
                (
                from cou in GetCourseList()
                from u in GetUserList()
                from cat in GetCategoryList()
                where cou.CategoryId == cat.CategoryId && cou.UserId == u.UserId && cou.UserId == UserId
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
        }*/
    }
}