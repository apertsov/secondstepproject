using System;
using System.Collections.Generic;

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


    public class TestResultModel
    {
        public string Login { get; set; }
        public double? Result { get; set; }
        public DateTime StartTesting { get; set; }
    }

    public class TestAndAnswersModel
    {
        public Test Test { get; set; }
        public List<Answer> AnswerList { get; set; }
    }

    public class LessonTestAndAnswersModel
    {
        public Lesson Lesson { get; set; }
        public List<TestAndAnswersModel> TestAndAnswers { get; set;}
    }

    public class ModuleTestAndAnswersModel
    {
        public Module Module { get; set; }
        public List<LessonTestAndAnswersModel> LessonsTestsAndAnswers { get; set; }
    }
}