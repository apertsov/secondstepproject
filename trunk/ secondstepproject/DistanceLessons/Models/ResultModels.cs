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
        public BetweenPartialModel Parameters { get; set; }
    }

    public class BetweenPartialModel//UserResultsModel
    {
        public Guid? pickedElement { get; set; }
        public ElementsType ElementType { get; set; }
        public string ControllerName { get; set; }
        public string DivIdToReplace { get; set; }
    }

    public class ModuleUserAnswers
    {
        public string Username { get; set; }
        public string ModuleTitle { get; set; }
        public double Result { get; set; }
        public float? ResultPercent { get; set; }
        public BetweenPartialModel Parameters { get; set; }
        public List<DetailAnswersModel> Answers { get; set; }
    }

    public class DetailModuleTestResultsModel
    {
        public string ModuleName { get; set; }
        public BetweenPartialModel Parameters { get; set; }
        public List<TestResultModel> TestResults { get; set; }
    }

    public class CourseResultModel
    {
        public string Login { get; set; }
        public Guid UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public Dictionary<Guid, string> PassedModules { get; set; }
        public List<KeyValuePair<int, int>> ResultModules { get; set; }
        public int CourseResult { get; set; }
    }

    public class UserResultModel
    {
        public KeyValuePair<Guid, string> CourseTitle { get; set; }
        public Dictionary<Guid, string> ModuleTitles { get; set; }
        public List<KeyValuePair<int,int>>  ResultModules { get; set; }
        public int CourseResult { get; set; }
    }

    public class StudentResultModel
    {
        public string CategoryTitle { get; set; }
        public List<UserResultModel> CourseResults { get; set; }
    }



}