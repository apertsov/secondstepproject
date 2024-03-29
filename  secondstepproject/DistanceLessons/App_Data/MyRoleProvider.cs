﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Collections.Specialized;
using DistanceLessons.Attributes;
using DistanceLessons.Models;

namespace DistanceLessons
{
    [Localization]
    public class MyRoleProvider : RoleProvider
    {
        private DataEntitiesManager userRepo = new DataEntitiesManager();

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
            throw new NotImplementedException();
        }

        public void ChangeUserRole(string username, string rolename)
        {
            userRepo.ChangeUserRole(rolename, username);
        }

        public override void CreateRole(string roleName)
        {
            Role newRole = new Role { RoleId = Guid.NewGuid(), Name = roleName };
            userRepo.AddRole(newRole);
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
            return new string[] { userRepo.UserRole(username) };
        }

        public override string[] GetUsersInRole(string roleName)
        {
            var role = userRepo.GetRole(roleName);
            return (from u in userRepo.GetUserList()
                    where u.RoleId==role.RoleId
                    select u.Login
                        ).ToArray();
        }

        public override bool IsUserInRole(string username, string roleName)
        {
            var role = userRepo.GetRole(roleName);
            return ((from u in userRepo.GetUserList()
                     where u.Login == username && u.RoleId == role.RoleId
                     select u) == null ? false : true);
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