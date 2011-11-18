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
    }
}