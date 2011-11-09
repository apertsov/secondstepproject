using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DistanceLessons.Models
{
    public partial class DataEntitiesManager
    {
        public List<Module> GetModuleList()
        {
            return _db.Modules.ToList<Module>();
        }
        public Module GetModulesByID(Guid id)
        {
            return _db.Modules.SingleOrDefault(c => c.ModuleId == id);
        }


        public void DeleteModule(Guid id)
        {
            var cat = _db.Modules.SingleOrDefault(c => c.ModuleId == id);
            _db.DeleteObject(cat);
            Save();
        }

        public void AddModule(Module obj)
        {
            _db.Modules.AddObject(obj);
            Save();
        }


        public List<RQModules> GetModulesByCourseId(Guid courseId)
        {
            var Query =
                (
                from m in GetModuleList()
                from c in GetCourseList()
                where m.CourseId == c.CourseId && c.CourseId == courseId
                orderby m.Title
                select new RQModules
                {
                    id = m.ModuleId,
                    title = m.Title,
                    course = c.Title
                }
                ).ToList<RQModules>();

            List<RQModules> lst = new List<RQModules>();
            foreach (var i in Query)
                lst.Add(i);

            return lst;
        }

        public Module Module(Guid ModuleId)
        {
            return (from module in _db.Modules
                    where module.ModuleId == ModuleId
                    select module).FirstOrDefault();
        }
        /*
                public List<Lesson> GetModulesByTeacherId(Guid UserId)
                {
                    var Query =
                        (
                        from m in GetModuleList()
                        from c in GetCourseList()
                        where c.UserId == UserId && m.CourseId == c.CourseId
                        orderby l.CourseId
                        select new Lesson
                        {
                            LessonId = l.LessonId,
                            Title = l.Title,
                            Publication = l.Publication,
                            CourseId = l.CourseId,
                            Text = l.Text,
                            UserId = l.UserId
                        }

                        ).ToList<Lesson>();

                    List<Lesson> lst = new List<Lesson>();
                    foreach (var i in Query)
                        lst.Add(i);

                    return lst;

                }*/
    }
}