using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DistanceLessons.Models
{
    public partial class DataEntitiesManager
    {
        public List<Answer> GetAnswerList()
        {
            return _db.Answers.ToList<Answer>();
        }
    }
}