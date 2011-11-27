using System;
using System.Collections.Generic;
using System.Linq;

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

        public List<TestAndAnswersModel> LessonTestsAndAnswers(Guid lessonId)
        {
            List<TestAndAnswersModel> testAndAnswersList = new List<TestAndAnswersModel>();
            List<Test> testList = LessonTests(lessonId);
            foreach (var test in testList)
                testAndAnswersList.Add(new TestAndAnswersModel { Test = test, AnswerList = TestAnswers(test.TestId) });
            return testAndAnswersList;
        }

        public List<LessonTestAndAnswersModel> ModuleTestsAndAnswers(Guid moduleId)
        {
            List<LessonTestAndAnswersModel> lessonTestsAndAnswersList = new List<LessonTestAndAnswersModel>();
            List<Lesson> lessons = GetLessonsByModule(moduleId);
            foreach (Lesson lesson in lessons)
            {
                lessonTestsAndAnswersList.Add(new LessonTestAndAnswersModel { Lesson = lesson, TestAndAnswers = LessonTestsAndAnswers(lesson.LessonId) });
            }
            return lessonTestsAndAnswersList;
        }

        public Test Test(Guid testId)
        {
            return (from test in _db.Tests
                    where test.TestId == testId
                    select test).FirstOrDefault();
        }

        public void AddTest(Test test)
        {
            if (test != null)
            {
                _db.AddToTests(test);
                Save();
            }
        }

        public void DeleteTestWithAnswers(Guid TestId)
        {
            Test test = (from _test in _db.Tests
                        where _test.TestId == TestId
                        select _test).FirstOrDefault();
            if (test != null)
            {
            List<Answer> answers = TestAnswers(TestId);
            foreach (Answer answer in answers)
                DeleteAnswer(answer.AnswerId);
             _db.DeleteObject(test);
             _db.SaveChanges();
            }
        }

        public void UpdateTest(Test test)
        {
                Test tmp = Test(test.TestId);
                tmp.Question = test.Question;
                Save();
        }

        public List<Test> ModuleTests(Guid ModuleId)
        {
            List<Test> testList = new List<Test>();
            List<Lesson> lessons=GetLessonsByModule(ModuleId);
            foreach (Lesson lesson in lessons)
                testList.AddRange(LessonTests(lesson.LessonId));
            return testList;
        }

        public bool IsTest(Guid id)
        {
            return (from test in _db.Tests
                    where test.TestId == id
                    select test).FirstOrDefault() == null ? false : true;
        }
    }
}