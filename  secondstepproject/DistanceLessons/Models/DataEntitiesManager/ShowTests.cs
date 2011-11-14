using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DistanceLessons.Models
{
    public partial class DataEntitiesManager
    {
        public List<ShowTest> ShowTestsInModule(Guid ModuleId, string username)
        {
            return (from user in _db.Users
                    from showTests in _db.ShowTests
                    where user.Login == username && user.UserId == showTests.UserId && showTests.ModuleId == ModuleId
                    select showTests).ToList();
        }

        public bool IsAllShowedModuleUserTests(Guid ModuleId, string username)
        {
            return (from user in _db.Users
                    from showTests in _db.ShowTests
                    where user.Login == username && user.UserId == showTests.UserId
                    && showTests.ModuleId == ModuleId && showTests.IsShowed == false
                    select showTests).Count() == 0 ? true : false;
        }

        public void AddNotShowedTests(List<Test> tests, Guid ModuleId, string username)
        {
            Guid UserId = GetUserId(username);
            foreach (Test test in tests)
                _db.AddToShowTests(new ShowTest { ShowTestId = Guid.NewGuid(), UserId = UserId, TestId = test.TestId, IsShowed = false, ModuleId = ModuleId });
        }

        public List<Guid> GetNotShowedTestsId(Guid moduleId, string username)
        {
            Guid UserId = GetUserId(username);
            return (from showTests in _db.ShowTests
                    where showTests.ModuleId == moduleId && showTests.UserId == UserId && showTests.IsShowed == false
                    select showTests.TestId).ToList();
        }

        public void MarkShowedTest(Guid testId, Guid ModuleId, string username)
        {
            ShowTest showTest = (from user in _db.Users
                                 from showTests in _db.ShowTests
                                 where user.Login == username && user.UserId == showTests.UserId &&
                                 showTests.ModuleId == ModuleId && showTests.IsShowed == false && showTests.TestId == testId
                                 select showTests).FirstOrDefault();
            if (showTest != null)
            {
                showTest.IsShowed = true;
                Save();
            }
        }

        public void DeleteShowTestsInUserModule(Guid moduleId, string username)
        {
            List<ShowTest> showTests = ShowTestsInModule(moduleId, username);
            if (showTests.Count > 0)
            {
                foreach (ShowTest showTest in showTests)
                    _db.DeleteObject(showTest);
                Save();
            }
        }
    }
}