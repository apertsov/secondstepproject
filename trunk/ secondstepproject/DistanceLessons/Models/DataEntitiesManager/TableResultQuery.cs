using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DistanceLessons.Models
{
    public partial class DataEntitiesManager
    {
        public class RQCategorys
        {
            public Guid id { get; set; }
            public string title { get; set; }
        }

        public class RQCourses
        {
            public Guid id { get; set; }
            public string title { get; set; }
            public string description { get; set; }
            public string category { get; set; }
            public string teacher { get; set; }
            public int access { get; set; }
        }

        public class RQTeachers
        {
            public Guid id { get; set; }
            public string login { get; set; }
            public string first { get; set; }
            public string last { get; set; }
            public string mid { get; set; }
            public string course { get; set; }
        }

        public class RQLessons
        {
            public Guid id { get; set; }
            public string title { get; set; }
            public string text { get; set; }
        }
		public class RQUserModules
        {
            public Guid id { get; set; }
            public string firstname { get; set; }
            public string lastname { get; set; }
            public string course { get; set; }
            public string module { get; set; }
            public double progress { get; set; }

        }
    }
}