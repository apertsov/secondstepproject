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

        public void DeleteUserModulesByModuleId(Guid moduleId)
        {
            var userModules = (from _userModules in GetUserModuleList()
                               where _userModules.ModuleId == moduleId
                               select _userModules);
            foreach (var userModule in userModules)
            {
                _db.DeleteObject(userModule);
            }
            Save();
        }

        public bool IsStartModuleTest(Guid ModuleId, string username)
        {
            return (from userResults in GetUserModuleList()
                    from user in GetUserList()
                    where userResults.ModuleId == ModuleId && user.Login == username && user.UserId == userResults.UserId
                    select userResults).Count() == 0 ? false : true;
        }

        public bool IsStartModuleTest(Guid ModuleId, Guid userId)
        {
            return (from userResults in GetUserModuleList()
                    where userResults.ModuleId == ModuleId && userResults.UserId == userId
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
                CourseResultModel userResult = new CourseResultModel { PassedModules = new Dictionary<Guid, string>(), ResultModules = new List<KeyValuePair<int,int>>()};
                userResult.Login = GetUsername(userCourseResult.Key);
                userResult.UserId = userCourseResult.Key;
                if (ExistInformation(userResult.Login))
                {
                    Information userInfo = UserInformation(userResult.Login);
                    userResult.FirstName = userInfo.FirstName;
                    userResult.MiddleName = userInfo.MidName;
                    userResult.LastName = userInfo.LastName;
                }
                else
                {
                    userResult.FirstName = userResult.MiddleName = userResult.LastName = string.Empty;
                }
                double courseResult = 0;
                foreach (var userModuleResult in userCourseResult)
                {
                    if ((userModuleResult.Passed != null) && (userModuleResult.ModuleId != null))
                    {
                        userResult.PassedModules.Add((Guid)userModuleResult.ModuleId, userModuleResult.Module.Title);
                        userResult.ResultModules.Add(new KeyValuePair<int,int>(userModuleResult.Module.MaxPoints, (int)CalcCourseResult((float)userModuleResult.Passed, userModuleResult.Module.MaxPoints)));
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
                UserResultModel userResult = new UserResultModel { ResultModules = new List<KeyValuePair<int, int>>(), ModuleTitles = new Dictionary<Guid, string>() };
                userResult.CourseTitle = new KeyValuePair<Guid, string>(userCourseResult.Key.CourseId, userCourseResult.Key.Title);
                double courseResult = 0;
                foreach (var userModuleResult in userCourseResult)
                {
                    if ((userModuleResult._userResults.Passed != null) && (userModuleResult._userResults.ModuleId != null))
                    {
                        userResult.ModuleTitles.Add(userModuleResult._userResults.Module.ModuleId, userModuleResult._userResults.Module.Title);
                        userResult.ResultModules.Add(new KeyValuePair<int, int>((int)userModuleResult._userResults.Module.MaxPoints, (int)CalcCourseResult((float)userModuleResult._userResults.Passed, userModuleResult._userResults.Module.MaxPoints)));
                        courseResult += CalcCourseResult((float)userModuleResult._userResults.Passed, userModuleResult._userResults.Module.MaxPoints);
                    }
                }
                userResult.CourseResult = (int)courseResult;
                model.Add(userResult);
            }
            return model;
        }

        public List<StudentResultModel> StudentResult(string username)
        {
            var results = (from userResults in GetUserModuleList()
                           where userResults.User.Login == username
                           group userResults by userResults.Module.Cours into coursesGroups
                           group coursesGroups by coursesGroups.Key.Category.Category1 into categoryGroups
                           select categoryGroups);
            List<StudentResultModel> studentResults = new List<StudentResultModel>();
            foreach (var categoryResult in results)
            {
                StudentResultModel CourseResult = new StudentResultModel { CategoryTitle = categoryResult.Key, CourseResults = new List<UserResultModel>() };
                foreach (var courseResult in categoryResult)
                {
                    UserResultModel courseResultModel = new UserResultModel { CourseTitle = new KeyValuePair<Guid, string>(courseResult.Key.CourseId, courseResult.Key.Title), ModuleTitles = new Dictionary<Guid, string>(), ResultModules = new List<KeyValuePair<int, int>>() };
                    double coursePoints = 0;
                    foreach (var moduleResult in courseResult)
                    {
                        if ((moduleResult.Passed != null) && (moduleResult.ModuleId != null))
                        {
                            courseResultModel.ModuleTitles.Add((Guid)moduleResult.ModuleId, moduleResult.Module.Title);
                            courseResultModel.ResultModules.Add(new KeyValuePair<int, int>(moduleResult.Module.MaxPoints, (int)CalcCourseResult((float)moduleResult.Passed, moduleResult.Module.MaxPoints)));
                            coursePoints += CalcCourseResult((float)moduleResult.Passed, moduleResult.Module.MaxPoints);
                        }
                    }
                    courseResultModel.CourseResult = (int)coursePoints;
                    CourseResult.CourseResults.Add(courseResultModel);
                }
                studentResults.Add(CourseResult);
            }
            //  throw new Exception();
            return studentResults;

        }

        public double CalcCourseResult(float passedPercent, int maxPoints)
        {
            return Math.Round((double)passedPercent / 100 * maxPoints, (int)4);
        }


    }

}