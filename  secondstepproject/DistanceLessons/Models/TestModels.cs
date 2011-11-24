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

    public class DetailModuleTestResultsModel
    {
        public string ModuleName { get; set; }
        public Guid ModuleId { get; set; }
        public List<TestResultModel> TestResults { get; set; }
    }

    public class TestResultModel
    {
        public string Login { get; set; }
        public double? Result { get; set; }
        public DateTime StartTesting { get; set; }
    }

    public class CourseResultModel
    {
        public string Login { get; set; }
        public Dictionary<Guid, string> PassedModules { get; set; }
        public Dictionary<Guid, int> ResultModules { get; set; }
        public int CourseResult { get; set; }
    }
}