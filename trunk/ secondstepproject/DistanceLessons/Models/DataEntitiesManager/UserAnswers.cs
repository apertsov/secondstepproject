using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DistanceLessons.Models
{
    public partial class DataEntitiesManager
    {
        public bool IsExistModuleUserAnswers(Guid ModuleId, string username)
        {
            return (from user in _db.Users
                    from answers in _db.UserAnswers
                    where user.Login==username && user.UserId==answers.UserId && answers.ModuleId==ModuleId
                    select answers).Count()>0?true:false;
        }

        public bool IsAllShowedModuleUserAnswers(Guid ModuleId, string username)
        {
            return (from user in _db.Users
                    from answers in _db.UserAnswers
                    where user.Login == username && user.UserId == answers.UserId && answers.ModuleId == ModuleId && answers.IsShowed==false
                    select answers).Count() > 0 ? false : true;
        }


        public void AddNullAnswersOnTests(List<Test> tests,Guid ModuleId ,string username)
        {
            Guid UserId = GetUserId(username);
            foreach (Test test in tests)
                _db.AddToUserAnswers(new UserAnswer { UserAnswerId = Guid.NewGuid(), UserId = UserId, TestId = test.TestId, IsShowed = false, AnswerId = null, ModuleId = ModuleId });
        }

        public List<UserAnswer> GetUserAnswersNotShowed(Guid ModuleId, string username)
        {
            Guid UserId = GetUserId(username);
            return (from userAnswers in _db.UserAnswers
                    where userAnswers.ModuleId == ModuleId && userAnswers.UserId == UserId && userAnswers.IsShowed == false
                    select userAnswers).ToList();
        }

    }
}