using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DistanceLessons.Models
{

    public class TestAndAnswersModel
        {
            public Test Test { get; set; }
            public List<Answer> AnswerList { get; set; }
        }
/*
    public class TestsModel
    {
        public List<TestAndAnswersModel> TestList { get; set; }       
    }
*/
}