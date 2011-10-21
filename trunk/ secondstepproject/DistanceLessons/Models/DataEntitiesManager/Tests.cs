using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DistanceLessons.Models
{
    public partial class DataEntitiesManager
    {
        public List<Test> GetTestList()
        {
            return _db.Tests.ToList<Test>();
        }

        public List<Test> LessonTests(Guid lessonId)
        {
            return (from tests in _db.Tests
                    where tests.LessonId == lessonId
                    select tests).ToList();
        }
    }
}