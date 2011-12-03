using System;
using System.Collections.Generic;
using System.Linq;

namespace DistanceLessons.Models
{
    public partial class DataEntitiesManager
    {
        public List<UserCours> GetUserCourseList()
        {
            return _db.UserCourses.ToList<UserCours>();
        }

        public bool UserSubscribe(Guid UserId, Guid CourseId)
        {
            var result = from uc in GetUserCourseList()
                         where uc.UserId == UserId && uc.CourseId == CourseId
                         select uc;
            if (result.Count() > 0) return true;
            return false;
        }

        public List<UserCourseModel> GetUserCourse_ListCount(Guid CategoryId, Guid userId, int type)
        {
            List<UserCourseModel> userCourses = new List<UserCourseModel>();
            List<Cours> Courses = GetCourseByCategory_ListCount(CategoryId, type);
            int temp;
            foreach (var item in Courses)
            {
                temp = (UserSubscribe(userId, item.CourseId)) ? 0 : 1;
                if (temp == 1) { temp = (ExistConfirmCourseMessage(userId, item.UserId, item.CourseId)) ? 2 : 1; }
                userCourses.Add(new UserCourseModel { Course = item, Subscribe = temp });
            }
            return userCourses;
        }

        public List<Cours> GetCourseByCategory_ListCount(Guid CategoryId, int type)
        {
            if (type > 0)
                return (from c in GetCourseList()
                        where c.Category.CategoryId == CategoryId && !c.IsSubscribing == false
                        orderby c.Title ascending
                        select c).ToList();
            else if (type < 0)
                return (from c in GetCourseList()
                        where c.Category.CategoryId == CategoryId && c.IsSubscribing == false
                        orderby c.Title ascending
                        select c).ToList();
            else
                return (from c in GetCourseList()
                        where c.Category.CategoryId == CategoryId
                        orderby c.Title ascending
                        select c).ToList();
        }

        public List<UserCourseModel> GetUserCourse(Guid CategoryId, Guid userId, int type, int pageSize, int numPage)
        {
            List<UserCourseModel> userCourses = new List<UserCourseModel>();
            List<Cours> Courses = GetCourseByCategory(CategoryId, type, pageSize, numPage);
            int temp;
            foreach (var item in Courses)
            {
                temp = (UserSubscribe(userId, item.CourseId)) ? 0 : 1;
                if (temp == 1) { temp = (ExistConfirmCourseMessage(userId, item.UserId, item.CourseId)) ? 2 : 1; }
                userCourses.Add(new UserCourseModel { Course = item, Subscribe = temp });
            }
            return userCourses;
        }

        public List<Cours> GetCourseByCategory(Guid CategoryId, int type, int pageSize, int numPage)
        {
            List<Cours> result = new List<Cours>();
            if (type > 0)
            {
                var Query = (from c in GetCourseList()
                             where c.Category.CategoryId == CategoryId && !c.IsSubscribing == false
                             orderby c.Title ascending
                             select c).Skip(pageSize * numPage).Take(pageSize);
                foreach (var i in Query)
                    result.Add(i);
            }
            else if (type < 0)
            {
                var Query = (from c in GetCourseList()
                             where c.Category.CategoryId == CategoryId && c.IsSubscribing == false
                             orderby c.Title ascending
                             select c).Skip(pageSize * numPage).Take(pageSize);
                foreach (var i in Query)
                    result.Add(i);
            }
            else
            {
                var Query = (from c in GetCourseList()
                             where c.Category.CategoryId == CategoryId
                             orderby c.Title ascending
                             select c).Skip(pageSize * numPage).Take(pageSize);
                foreach (var i in Query)
                    result.Add(i);
            }

            return result;
        }

        public void AddUserCourse(UserCours obj)
        {
            _db.UserCourses.AddObject(obj);
            _db.SaveChanges();
        }

        public Guid GetUserCourseId(Guid userId, Guid CourseId)
        {
            return (from uc in GetUserCourseList()
                    where uc.UserId == userId && uc.CourseId == CourseId
                    select uc.UserCourseId).FirstOrDefault();
        }

        public void DeleteUserCourse(Guid id)
        {
            _db.DeleteObject(_db.UserCourses.SingleOrDefault(c => c.UserCourseId == id));
            _db.SaveChanges();
        }

        public List<UserCours> GetUserCourseListByUser(Guid UserId)
        {
            return (from uc in GetUserCourseList()
                    where uc.UserId == UserId
                    select uc).ToList();
        }
    }
}