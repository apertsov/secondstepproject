using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace DistanceLessons.Models
{
    public partial class ChangePasswordModel
    {
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }
    }

    public partial class LogOnModel
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }

    public partial class RegisterModel
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }

    public class AllUserInformationModel
    {
    public Information Information{get; set;}
    public Contact Contact {get; set;}
    }

    public class AccountRoleService
    {
        private readonly MyRoleProvider _provider;

        public AccountRoleService()
            : this(null)
        {
        }

        public AccountRoleService(MyRoleProvider provider)
        {
            _provider = provider;
        }

        public bool AdminExists()
        {
            var users = _provider.GetUsersInRole("Admin");

            if ((users[0] == null) || (users[0] ==""))
                return false;

            return true;
        }

        public void RemoveUsersFromRoles(string[] usernames, string[] rolenames)
        {
            _provider.RemoveUsersFromRoles(usernames, rolenames);
        }

        public void CreateRole(string roleName)
        {
            _provider.CreateRole(roleName);
        }
    }
}
