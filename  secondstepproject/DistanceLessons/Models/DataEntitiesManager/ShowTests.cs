using System;
using System.Collections.Generic;
using System.Linq;

namespace DistanceLessons.Models
{
    public partial class DataEntitiesManager
    {
        public List<ShowTest> GetShowTestsList()
        {
            return _db.ShowTests.ToList<ShowTest>();
        }

        public List<ShowTest> ShowTestsInModule(Guid ModuleId, string username)
        {
            return (from user in GetUserList()
                    from showTests in GetShowTestsList()
                    where user.Login == username && user.UserId == showTests.UserId && showTests.ModuleId == ModuleId
                    select showTests).ToList();
        }

        public bool IsAllShowedModuleUserTests(Guid ModuleId, string username)
        {
            return (from user in GetUserList()
                    from showTests in GetShowTestsList()
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
            return (from showTests in GetShowTestsList()
                    where showTests.ModuleId == moduleId && showTests.UserId == UserId && showTests.IsShowed == false
                    select showTests.TestId).ToList();
        }

        public void MarkShowedTest(Guid testId, Guid ModuleId, string username)
        {
            ShowTest showTest = (from user in GetUserList()
                                 from showTests in GetShowTestsList()
                                 where user.Login == username && user.UserId == showTests.UserId &&
                                 showTests.ModuleId == ModuleId && showTests.IsShowed == false && showTests.TestId == testId
                                 select showTests).FirstOrDefault();
            if (showTest != null)
            {
                showTest.IsShowed = true;
                Save();
            }
        }

        public void MarkModuleTestsShowed(Guid ModuleId, string username)
        {
            List<ShowTest> notShowedTests = (from user in GetUserList()
                                             from showTests in GetShowTestsList()
                                 where user.Login == username && user.UserId == showTests.UserId &&
                                 showTests.ModuleId == ModuleId && showTests.IsShowed == false
                                 select showTests).ToList();
            if (notShowedTests != null)
            {
                foreach (ShowTest showTest in notShowedTests)
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

        public void DeleteShowTestsByModuleId(Guid moduleId)
        {
            var showTests=(from _showTests in GetShowTestsList()
                            where _showTests.ModuleId==moduleId
                             select _showTests);
            foreach (var showTest in showTests)
                _db.DeleteObject(showTest);
            Save();
        }
    }
}