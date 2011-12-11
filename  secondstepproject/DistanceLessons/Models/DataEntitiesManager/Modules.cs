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

        public List<Module> ModulesEndTestingInFiveDays()
        {
            TimeSpan FiveDays = new TimeSpan(5,0,0,1);
            List<Module> modul = (from modules in GetModuleList()
                                  where modules.DateEndTesting.CompareTo(DateTime.Now)>0 &&  modules.DateBeginTesting.CompareTo(DateTime.Now)<0 && modules.DateEndTesting.Subtract(DateTime.Now).TotalSeconds < FiveDays.TotalSeconds 
                                  select modules).ToList<Module>();
            return modul;
        }

        public bool ExistModule(Guid moduleId)
        {
            return (from modules in GetModuleList()
                    where modules.ModuleId == moduleId
                    select modules).Count() > 0 ? true : false;
        }
    }
}