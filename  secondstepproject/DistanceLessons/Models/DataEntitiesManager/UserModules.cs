using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DistanceLessons.Models
{
    public partial class DataEntitiesManager
    {
        public List<UserModule> GetUserModuleList()
        {
            return _db.UserModules.ToList<UserModule>();
        }

        public List<RQUserModules> GetUserProgress()
        {

            var Query =
                (
                  
                    from ui in GetInfoList()
                    group ui by new { ui.LastName, ui.FirstName, ui.UserId} into usrinf
                    from um in GetUserModuleList()
                    from m in GetModuleList()
                    from cou in GetCourseList()
                    where um.UserId == usrinf.Key.UserId && cou.CourseId == m.CourseId && um.ModuleId == m.ModuleId

                    orderby usrinf.Key.LastName
                    select new RQUserModules
                    {
                        id = um.UserModuleId,
                        firstname = usrinf.Key.FirstName,
                        lastname = usrinf.Key.LastName,
                        course = cou.Title,
                        module = m.Title,
                        progress = um.Passed
                    }




        ).ToList<RQUserModules>();

            List<RQUserModules> lst = new List<RQUserModules>();
            foreach (var i in Query)
                lst.Add(i);

            return lst;
        }
    }
}