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

        public List<CourseResultModel> ResultUserCourse(Guid courseID)
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
                CourseResultModel userResult = new CourseResultModel { PassedModules = new Dictionary<Guid, string>(), ResultModules = new Dictionary<Guid, int>() };
                userResult.Login = GetUsername(userCourseResult.Key);
                float courseResult = 0;
                foreach (var userModuleResult in userCourseResult)
                {
                    if ((userModuleResult.Passed != null) && (userModuleResult.ModuleId != null))
                    {
                        userResult.PassedModules.Add((Guid)userModuleResult.ModuleId, userModuleResult.Module.Title);
                        userResult.ResultModules.Add((Guid)userModuleResult.ModuleId, (int)userModuleResult.Passed);
                        courseResult += (float)userModuleResult.Passed;
                    }
                    userResult.CourseResult = (int)courseResult;
                }
                model.Add(userResult);
            }
            return model;
        }

        public List<RQUserModules> GetUserProgress()
        {

            var Query =
                (
                    from ui in GetInfoList()
                    group ui by new { ui.LastName, ui.FirstName, ui.UserId } into usrinf
                    from um in GetUserModuleList()
                    from m in GetModuleList()
                    from cou in GetCourseList()
                    where um.UserId == usrinf.Key.UserId && cou.CourseId == m.CourseId && um.ModuleId == m.ModuleId

                    orderby usrinf.Key.LastName
                    select new RQUserModules
                    {
                        id = um.UserModuleId,
                        firstname = usrinf.Key.FirstName,
                        lastname = usrinf.Key.LastName,
                        course = cou.Title,
                        module = m.Title,
                        progress = um.Passed
                    }




        ).ToList<RQUserModules>();

            List<RQUserModules> lst = new List<RQUserModules>();
            foreach (var i in Query)
                lst.Add(i);

            return lst;
        }
    }

}