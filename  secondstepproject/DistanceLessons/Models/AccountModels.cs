using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Web.Mvc;
using System.Web.Security;

namespace DistanceLessons.Models
{

    public class ChangePasswordModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Поточний пароль")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(30, ErrorMessage = "Пароль повинен бути менше {0} і більше ніж {2} символів", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Новий пароль")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Підтвердіть новий пароль")]
        [Compare("NewPassword", ErrorMessage = "Новий пароль і його підтвердження не співпадають!")]
        public string ConfirmPassword { get; set; }
    }

    public class LogOnModel
    {
        [Required]
        [Display(Name = "Логін користувача")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }

        [Display(Name = "Запам'ятати?")]
        public bool RememberMe { get; set; }
    }

    public class RegisterModel
    {
        [Required]
        [Display(Name = "Логін користувача")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Електронан пошта")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Підтвердження пароля")]
        [Compare("Password", ErrorMessage = "Пароль і підтвердження пароля не співпадають")]
        public string ConfirmPassword { get; set; }
    }

    public class AccountRoleService 
    {
        private readonly RoleProvider _provider;

        public AccountRoleService()
            : this(null)
        {
        }

        public AccountRoleService(RoleProvider provider)
        {
            _provider = provider;
        }

        public bool AdminExists()
        {
            var users = _provider.GetUsersInRole("Admin");

            if (users.Length == 0)
                return false;

            return true;
        }

        public void AddUsersToRoles(string[] usernames, string[] rolenames)
        {
            _provider.AddUsersToRoles(usernames, rolenames);
        }

        public void RemoveUsersFromRoles(string[] usernames, string[] rolenames)
        {
            _provider.RemoveUsersFromRoles(usernames, rolenames);
        }

        public void CreateRole(string roleName)
        {
            _provider.CreateRole(roleName);
        }
        public bool RoleExists(string roleName)
        {
            return _provider.RoleExists(roleName);
        }
    }
}
