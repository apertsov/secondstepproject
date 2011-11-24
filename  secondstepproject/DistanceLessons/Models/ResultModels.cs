using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DistanceLessons.Models
{
    public class EducationElementsModel
    {
        public Dictionary<Guid, string> Modules { get; set; }
        public Dictionary<Guid, string> Courses { get; set; }
        public Dictionary<Guid, string> Categories { get; set; }
        public Guid? pickedElement { get; set; }
        public string ControllerName { get; set; }
        public int type { get; set; }
    }

    public class UserResultsModel
    {
        public Guid? pickedElement { get; set; }
        public int type { get; set; }
    }
    
}