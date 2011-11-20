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
            if ((module!=null) && (module.CountQuestions != null)) return (int)module.CountQuestions;
            return ModuleTests(moduleId).Count;
        }

     }
}