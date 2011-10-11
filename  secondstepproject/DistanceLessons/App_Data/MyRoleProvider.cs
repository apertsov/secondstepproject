using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Collections.Specialized;

namespace DistanceLessons.Models
{
    public class MyRoleProvider : RoleProvider
    {
        private UserRepository userRepo = new UserRepository();

        private string _ApplicationName;
        public override void Initialize(string name, NameValueCollection config)
        {
            if (config == null)
                throw new ArgumentNullException("config");

            if (name == null || name.Length == 0)
                name = "CustomRoleProvider";

            if (String.IsNullOrEmpty(config["description"]))
            {
                config.Remove("description");
                config.Add("description", "Custom Role Provider");
            }

            base.Initialize(name, config);

            _ApplicationName = GetConfigValue(config["applicationName"],
                          System.Web.Hosting.HostingEnvironment.ApplicationVirtualPath);

        }

        public override string ApplicationName
        {
            get { return _ApplicationName; }
            set { _ApplicationName = value; }
        }

        //
        //  Хелпер для получение значений из конфигурационного файла.

        private string GetConfigValue(string configValue, string defaultValue)
        {
            if (string.IsNullOrEmpty(configValue))
                return defaultValue;

            return configValue;
        }
        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            foreach (var username in usernames)
            {
                var user = userRepo.GetDBUser(username);
                if (user != null)
                {
                    foreach (var rolename in roleNames)
                    {
                        var role = userRepo.GetRole(rolename);
                        if (role != null)
                        {
                            user.RoleId = role.RoleId;
                        }
                    }
                    userRepo.db.SaveChanges();
                }
            }
        }
        public override void CreateRole(string roleName)
        {
            Role newRole = new Role { RoleId = Guid.NewGuid(), Name = roleName };
            userRepo.db.AddToRoles(newRole);
            userRepo.db.SaveChanges();
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            throw new NotImplementedException();
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            throw new NotImplementedException();
        }

        public override string[] GetAllRoles()
        {
            throw new NotImplementedException();
        }

        public override string[] GetRolesForUser(string username)
        {
             var user = userRepo.GetDBUser(username);
             return (userRepo.db.Roles.Where(x => x.RoleId==user.RoleId)).Select(x => x.Name).ToArray();
        }

        public override string[] GetUsersInRole(string roleName)
        {
            if (roleName == null) throw new ArgumentNullException("roleName", "Unknown role");
            var role = userRepo.GetRole(roleName);
            if (role == null) throw new ArgumentNullException("roleName", "Unknown role");
          /*  var usernames = userRepo.GetAllUsers()
                .Where(x => x.Role==role.IdRole)
                .Select(x => x.Login);*/
            return (from u in userRepo.GetAllUsers()
                    where u.RoleId==role.RoleId
                    select u.Login
                        ).ToArray();
        }

        public override bool IsUserInRole(string username, string roleName)
        {
            throw new NotImplementedException();
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override bool RoleExists(string roleName)
        {
            return (userRepo.GetRole(roleName)==null?false:true);
        }

        public bool IsExistUsersInRole(string roleName)
        {
            var users = this.GetUsersInRole(roleName);
            if (users.Count() == 0)
                return false;

            return true;
        }
    }
}