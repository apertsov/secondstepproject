using System;
using System.Collections.Generic;
using System.Linq;

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
            var lessons = GetLessonsByModule(id);
            foreach (Lesson les in lessons)
            {
                les.ModuleId = null;
            }
            DeleteUserModulesByModuleId(id);
            DeleteShowTestsByModuleId(id);
            DeleteAnswers(id);
            var cat = _db.Modules.SingleOrDefault(c => c.ModuleId == id);
            _db.DeleteObject(cat);
            Save();
        }

        public void AddModule(Module obj)
        {
            _db.Modules.AddObject(obj);
            Save();
        }


        public List<Module> GetModulesByCourseId(Guid courseId)
        {
            return (from modules in _db.Modules
                    where modules.CourseId == courseId
                    select modules).ToList();

        }
        public Module Module(Guid ModuleId)
        {
            return (from module in _db.Modules
                    where module.ModuleId == ModuleId
                    select module).FirstOrDefault();
        }

        public string ModuleName(Guid moduleId)
        {
            Module mod= (from module in _db.Modules
                    where module.ModuleId == moduleId
                    select module).FirstOrDefault();
            if (mod == null) return String.Empty;
            return mod.Title;
        }

        public int CountModuleQuestions(Guid moduleId)
        {
            Module module = Module(moduleId);
            if ((module == null) || (module.CountQuestions == null)) return ModuleTests(moduleId).Count;
            return Math.Min((int)module.CountQuestions, ModuleTests(moduleId).Count);   
      //      return ModuleTests(moduleId).Count;
        }

        public Dictionary<Guid, string> ModulesDictionaryFromCourse(Guid courseId)
        {
            var _modules = (from modules in GetModuleList()
                                   where modules.CourseId == courseId
                                   select new { moduleId = modules.ModuleId, title = modules.Title });
            Dictionary<Guid, string> modulesFromCourse = new Dictionary<Guid, string>();
            foreach (var module in _modules)
                modulesFromCourse.Add(module.moduleId, module.title);
            return modulesFromCourse;
        }

        public Guid CategoryIdFromModuleId(Guid moduleId)
        {
            return CategoryIdFromCourseId(CourseIdFromModuleId(moduleId));
        }

        public Guid CourseIdFromModuleId(Guid moduleId)
        {
            return (from modules in GetModuleList()
                    where modules.ModuleId == moduleId
                    select modules.CourseId).FirstOrDefault();
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