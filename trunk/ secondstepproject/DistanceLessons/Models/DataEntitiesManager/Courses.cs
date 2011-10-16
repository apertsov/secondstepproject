﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DistanceLessons.Models
{
    public partial class DataEntitiesManager
    {
        public List<Cours> GetCourseList()
        {
            return _distancel.Courses.ToList<Cours>();
        }

        public Cours GetCourse(Guid id)
        {
            return _distancel.Courses.SingleOrDefault(c => c.CourseId == id);
        }

        public void DeleteCourse(Guid id)
        {
            var cat = _distancel.Courses.SingleOrDefault(c => c.CourseId == id);
            _distancel.DeleteObject(cat);
            _distancel.SaveChanges();
        }

        public void AddCours(Cours obj)
        {
            _distancel.Courses.AddObject(obj);
            _distancel.SaveChanges();
        }

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
    }
}