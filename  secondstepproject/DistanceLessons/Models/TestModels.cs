using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DistanceLessons.Models
{
    public class PassTestModel
    {
        public TestAndAnswersModel TestAndAnswers { get; set; }
        public Module PassedModule { get; set; }
        public TimeSpan TimeToResolve { get; set; }
    }

    public class DetailAnswersModel
    {
        public TestAndAnswersModel TestAndAnswers { get; set; }
        public List<UserAnswer> UserAnswers { get; set; }
        public bool IsRightAnswer { get; set; }
    }
}