﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace DistanceLessons.Models
{
    public partial class DataEntitiesManager
    {
        public List<UserAnswer> GetUserAnswersList()
        {
            return _db.UserAnswers.ToList<UserAnswer>();
        }

        public UserAnswer UserAnswer(Guid UserAnswerId)
        {
            return (from userAnswer in GetUserAnswersList()
                    where userAnswer.UserAnswerId == UserAnswerId
                    select userAnswer).FirstOrDefault();
        }

        public List<UserAnswer> UserAnswersOnTest(Guid testId, Guid moduleId, string username)
        {
            return (from userAnswer in GetUserAnswersList()
                    from user in GetUserList()
                    where userAnswer.TestId == testId && userAnswer.ModuleId == moduleId &&
                    user.Login == username && user.UserId == userAnswer.UserId
                    select userAnswer).ToList();
        }

        public List<UserAnswer> UserAnswers(Guid moduleId, string username)
        {
            return (from userAnswer in GetUserAnswersList()
                    from user in GetUserList()
                    where userAnswer.ModuleId == moduleId &&
                    user.Login == username && user.UserId == userAnswer.UserId
                    select userAnswer).ToList();
        }

        public void AddUserAnswer(Guid testId, Guid moduleId, string username, Guid answerId)
        {
            _db.AddToUserAnswers(new UserAnswer { UserAnswerId = Guid.NewGuid(), UserId = GetUserId(username), TestId = testId, AnswerId = answerId, ModuleId = moduleId });
            Save();
        }

        public void UpdateUserAnswer(UserAnswer answer)
        {
            UserAnswer tmp = UserAnswer(answer.UserAnswerId);
            tmp.AnswerId = answer.AnswerId;
            tmp.ModuleId = answer.ModuleId;
            tmp.TestId = answer.TestId;
            tmp.UserId = answer.UserId;
            Save();
        }

        public void DeleteAnswers(Guid moduleId, string username)
        {
            List<UserAnswer> answers = UserAnswers(moduleId, username);
            if (answers.Count > 0)
            {
                foreach (UserAnswer answer in answers)
                    _db.DeleteObject(answer);
                Save();
            }
        }

        public void DeleteAnswers(Guid moduleId)
        {
            var answers = (from _answers in GetUserAnswersList()
                           where _answers.ModuleId == moduleId
                           select _answers);
            if (answers.Count() > 0)
            {
                foreach (UserAnswer answer in answers)
                    _db.DeleteObject(answer);
                Save();
            }
        }

        public float CalcUserModuleResults(Guid moduleId, string username)
        {
            List<UserAnswer> userAnswers = (from answers in GetUserAnswersList()
                                            from user in GetUserList()
                                            where answers.ModuleId == moduleId && user.Login == username && user.UserId == answers.UserId && answers.TestId!=null && answers.AnswerId!=null
                                            orderby answers.TestId
                                            select answers).ToList();
            return CalcGrade(userAnswers, CountModuleQuestions(moduleId));
        }



        private float CalcGrade(List<UserAnswer> userAnswers,int countQuestions)
        {
            int countTrueAnswers = 0;
            int indexOneTest, indexAnswer = 0;
            while (indexAnswer < userAnswers.Count)
            {
                indexOneTest = indexAnswer + 1;
                while ((indexOneTest < userAnswers.Count) && (userAnswers[indexAnswer].TestId == userAnswers[indexOneTest].TestId))   //
                {
                    indexOneTest++;
                }

                if (IsTrueAnswer(userAnswers.GetRange(indexAnswer, indexOneTest - indexAnswer), (Guid)userAnswers[indexAnswer].TestId)) 
                    countTrueAnswers++;
                indexAnswer = indexOneTest;
            }

            return 100 * countTrueAnswers / countQuestions;
        }

        public bool IsTrueAnswer(List<UserAnswer> userAnswers, Guid testId)
        {
            List<Answer> trueAnswers = TestTrueAnswers(testId);
            if (trueAnswers.Count == 0) return false;
            bool inTrueAnswers;
            foreach(UserAnswer userAnswer in userAnswers)
            {
                inTrueAnswers=false;
                foreach (Answer answer in trueAnswers)
                    if (userAnswer.Answer == answer)
                    {
                        inTrueAnswers = true;
                        trueAnswers.Remove(answer);
                        break;
                    }
                if (!inTrueAnswers) return false;
            }
            if (trueAnswers.Count > 0) return false;
            return true;
        }

        public bool ExistUserAnswers(Guid userAnswerId)
        {
            return (from userAnswers in GetUserAnswersList()
                    where userAnswers.UserAnswerId == userAnswerId
                    select userAnswers).Count() > 0 ? true : false;
        }


    }
}