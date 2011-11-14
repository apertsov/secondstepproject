using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

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
            return (from userResult in _db.UserModules
                    where userResult.UserId == ResultId
                    select userResult).FirstOrDefault();
        }

        public UserModule UserModule(Guid ModuleId, string username)
        {
            return (from userResult in _db.UserModules
                    from user in _db.Users
                    where userResult.ModuleId == ModuleId && userResult.UserId == user.UserId && user.Login == username
                    select userResult).FirstOrDefault();
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
            old.PassedTime = userResult.PassedTime;
            old.SpendTime = userResult.SpendTime;
            old.UserId = userResult.UserId;
            Save();
        }

        public void UpdateUserModule(Guid moduleId, string username, float result, DateTime passedTime)
        {
            UserModule userResult = UserModule(moduleId, username);
            if (userResult != null)
            {
                userResult.Passed = result;
                userResult.PassedTime = passedTime;
                Save();
            }
        }

        public bool IsStartModuleTest(Guid ModuleId, string username)
        {
            return (from userResults in _db.UserModules
                    from user in _db.Users
                    where userResults.ModuleId == ModuleId && user.Login == username && user.UserId == userResults.UserId
                    select userResults).Count() == 0 ? false : true;
        }

        public void DeleteModuleTest(Guid moduleId, string username)
        {
            UserModule _userResult=(from userResult in _db.UserModules
                             from user in _db.Users
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