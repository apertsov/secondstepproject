using System;
using System.Collections.Generic;
using System.Linq;

namespace DistanceLessons.Models
{
    public partial class DataEntitiesManager
    {
        public List<UserModule> GetUserModuleList()
        {
            return _db.UserModules.ToList<UserModule>();
        }

        public UserModule UserModule(Guid ResultId)
        {
            return (from userResult in GetUserModuleList()
                    where userResult.UserId == ResultId
                    select userResult).FirstOrDefault();
        }

        public UserModule UserModule(Guid ModuleId, string username)
        {
            return (from userResult in GetUserModuleList()
                    from user in GetUserList()
                    where userResult.ModuleId == ModuleId && userResult.UserId == user.UserId && user.Login == username
                    select userResult).FirstOrDefault();
        }

        public List<UserModule> UserModules(Guid moduleId)
        {
            return (from userResult in GetUserModuleList()
                    where userResult.ModuleId == moduleId
                    select userResult).ToList<UserModule>();
        }

        public void AddUserModules(UserModule userResult)
        {
            _db.AddToUserModules(userResult);
            Save();
        }

        public void UpdateUserModule(UserModule userResult)
        {
            UserModule old = UserModule(userResult.UserModuleId);
            old.Passed = userResult.Passed;
            old.EndTime = userResult.EndTime;
            old.StartTime = userResult.StartTime;
            old.UserId = userResult.UserId;
            Save();
        }

        public void UpdateUserModule(Guid moduleId, string username, float result)
        {
            UserModule userResult = UserModule(moduleId, username);
            if (userResult != null)
            {
                userResult.Passed = result;
                Save();
            }
        }

        public bool IsStartModuleTest(Guid ModuleId, string username)
        {
            return (from userResults in GetUserModuleList()
                    from user in GetUserList()
                    where userResults.ModuleId == ModuleId && user.Login == username && user.UserId == userResults.UserId
                    select userResults).Count() == 0 ? false : true;
        }

        public bool IsPassedTest(Guid ModuleId, string username)
        {
            return (from userResults in GetUserModuleList()
                    from user in GetUserList()
                    where userResults.ModuleId == ModuleId && user.Login == username && user.UserId == userResults.UserId && userResults.Passed == null
                    select userResults).Count() == 0 ? true : false;
        }

        public void DeleteModuleTest(Guid moduleId, string username)
        {
            UserModule _userResult = (from userResult in GetUserModuleList()
                                      from user in GetUserList()
                                      where userResult.ModuleId == moduleId && user.Login == username && user.UserId == userResult.UserId
                                      select userResult).FirstOrDefault();
            if (_userResult != null)
            {
                DeleteShowTestsInUserModule(moduleId, username);
                DeleteAnswers(moduleId, username);
                _db.DeleteObject(_userResult);
                Save();
            }
        }

        public List<CourseResultModel> ResultsUsersCourse(Guid courseID)
        {
            List<Module> moduleList = GetModulesByCourseId(courseID);
            var userResults = (from _userResults in GetUserModuleList()
                               from modules in moduleList
                               //       join logins in GetUserList() on _userResults.UserId equals logins.UserId
                               where _userResults.ModuleId == modules.ModuleId
                               group _userResults by _userResults.UserId into userGroups
                               select userGroups);
            List<CourseResultModel> model = new List<CourseResultModel>();
            foreach (var userCourseResult in userResults)
            {
                CourseResultModel userResult = new CourseResultModel { PassedModules = new Dictionary<Guid, string>(), ResultModules = new Dictionary<int, int>() };
                userResult.Login = GetUsername(userCourseResult.Key);
                double courseResult = 0;
                foreach (var userModuleResult in userCourseResult)
                {
                    if ((userModuleResult.Passed != null) && (userModuleResult.ModuleId != null))
                    {
                        userResult.PassedModules.Add((Guid)userModuleResult.ModuleId, userModuleResult.Module.Title);
                        userResult.ResultModules.Add(userModuleResult.Module.MaxPoints, (int)CalcCourseResult((float)userModuleResult.Passed, userModuleResult.Module.MaxPoints));
                        courseResult += CalcCourseResult((float)userModuleResult.Passed, userModuleResult.Module.MaxPoints);
                    }
                    userResult.CourseResult = (int)courseResult;
                }
                model.Add(userResult);
            }
            return model;
        }

        public List<UserResultModel> ResultUserCourse(string username)
        {
            //     Guid userId = GetUserId(username);
            var userResults = (from _userResults in GetUserModuleList()
                               join courses in GetCourseList() on _userResults.Module.CourseId equals courses.CourseId
                               where _userResults.User.Login == username
                               group new { courses.Title, _userResults } by courses into coursesGroups
                               select coursesGroups);
            List<UserResultModel> model = new List<UserResultModel>();
            foreach (var userCourseResult in userResults)
            {
                UserResultModel userResult = new UserResultModel { ResultModules = new Dictionary<int, int>(), ModuleTitles = new List<string>() };
                userResult.CourseTitle = userCourseResult.Key.Title;
                double courseResult = 0;
                foreach (var userModuleResult in userCourseResult)
                {
                    if ((userModuleResult._userResults.Passed != null) && (userModuleResult._userResults.ModuleId != null))
                    {
                        userResult.ModuleTitles.Add(userModuleResult._userResults.Module.Title);
                        userResult.ResultModules.Add((int)userModuleResult._userResults.Module.MaxPoints, (int)CalcCourseResult((float)userModuleResult._userResults.Passed, userModuleResult._userResults.Module.MaxPoints));
                        courseResult += CalcCourseResult((float)userModuleResult._userResults.Passed,userModuleResult._userResults.Module.MaxPoints);
                    }
                    userResult.CourseResult = (int)courseResult;
                }
                model.Add(userResult);
            }
            return model;
        }

        public double CalcCourseResult(float passedPercent, int maxPoints)
        {
            return Math.Round((double) passedPercent/100 * maxPoints,(int)4);
        }
    }

}