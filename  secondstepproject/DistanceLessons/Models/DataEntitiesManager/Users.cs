﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web.Security;

namespace DistanceLessons.Models
{
    public partial class DataEntitiesManager
    {
        public List<User> GetUserList()
        {
            return _db.Users.ToList<User>();
        }
        
        public User GetUser(Guid id)
        {
            return _db.Users.SingleOrDefault(c => c.UserId == id);
        }

        public string GetUsername(Guid userId)
        {

            return (from user in GetUserList()
                    where user.UserId == userId
                    select user.Login).FirstOrDefault(); 
        }

        public void DeleteUser(Guid id)
        {
            var cat = _db.Users.SingleOrDefault(c => c.UserId == id);
            _db.DeleteObject(cat);
            _db.SaveChanges();
        }

        public void AddUser(User obj)
        {
            _db.Users.AddObject(obj);
            _db.SaveChanges();
        }

	   public List<User> GetGetUserByRole(string role)
        {
            Guid roleId= GetRole(role).RoleId;;
            return (from users in _db.Users
                    where users.RoleId == roleId
                    select users).ToList();
        }
		
        public MembershipUser CreateUser(string username, string password, string email, bool IsApproved)
        {
            using (dbEntities db = new dbEntities())
            {
                User user = new User();

                username = username.Trim();
                password = password.Trim();
                email = email.Trim();
                user.UserId = Guid.NewGuid();
                user.Login = username;
                user.Email = email;
                user.RoleId = this.GetRole(UserRoles.Student.ToString()).RoleId;
                user.PasswordSalt = CreateSalt();
                user.CreatedDate = DateTime.Now;
                user.IsActived = IsApproved;
                user.IsLockedOut = false;
                user.LastLoginDate = DateTime.Now;
                user.Password = CreatePasswordHash(password, user.PasswordSalt);
                user.EmailKey = GenerateKey();
                db.AddToUsers(user);
                db.SaveChanges();
                string ActivationLink = "http://localhost:55555/Account/Activate/" +
                                      user.Login + "/" + user.EmailKey;

                Email.SendActivationLink(ActivationLink,user.Email);

                return GetMembershipUser(username);
            }
        }

        public string GetUserNameByEmail(string email)
        {
            
                var result = from u in _db.Users where (u.Email == email) select u;

                if (result.Count() != 0)
                {
                    var dbuser = result.FirstOrDefault();

                    return dbuser.Login;
                }
                else
                {
                    return "";
                }
            
        }

        public MembershipUser GetMembershipUser(string username)
        {
                var result = from u in _db.Users where (u.Login == username) select u;

                if (result.Count() != 0)
                {
                    var dbuser = result.FirstOrDefault();

                    string _username = dbuser.Login;
                    Guid _providerUserKey = dbuser.UserId;
                    string _email = dbuser.Email;
                    string _passwordQuestion = "";
                    string _comment = "";
                    bool _isApproved = dbuser.IsActived;
                    bool _isLockedOut = dbuser.IsLockedOut;
                    DateTime _creationDate = dbuser.CreatedDate;
                    DateTime _lastLoginDate = dbuser.LastLoginDate;
                    DateTime _lastActivityDate = DateTime.Now;
                    DateTime _lastPasswordChangedDate = DateTime.Now;
                    DateTime _lastLockedOutDate = DateTime.Now;

                    MembershipUser user = new MembershipUser("DefaultMembershipProvider",
                                              _username,
                                              _providerUserKey,
                                              _email,
                                              _passwordQuestion,
                                              _comment,
                                              _isApproved,
                                              _isLockedOut,
                                              _creationDate,
                                              _lastLoginDate,
                                              _lastActivityDate,
                                              _lastPasswordChangedDate,
                                              _lastLockedOutDate);

                    return user;
                }
                else
                {
                    return null;
                }
            
        }

        public bool ValidateUser(string username, string password)
        {
            var dbuser = _db.Users.FirstOrDefault(x => x.Login == username);
       //     if (!dbuser.IsActived) return false;
            return dbuser != null && (dbuser.IsActived) && (!dbuser.IsLockedOut) && dbuser.Password == CreatePasswordHash(password, dbuser.PasswordSalt);
        }

        public void SetUserLoginDate(string username)
        {
            User user = _db.Users.FirstOrDefault(x => x.Login == username);
            if (user != null)
            {
                user.LastLoginDate = DateTime.Now;
                Save();
            }
        }

        public User GetUser(string username)
        {
            return _db.Users.SingleOrDefault(x => x.Login == username);
        }

        public string UserRole(string username)
        {
            Role role = GetRole(GetUserRoleId(username));
            return (role==null?null:role.Name);
        }

        public Guid GetUserId(string username)
        {
            var userId = (from x in GetUserList()
                       where x.Login.ToUpper() == username.ToUpper()
                       select x.UserId).FirstOrDefault();
            return userId;
        }

        

        public Guid GetUserRoleId(string username)
        {
            return (from user in GetUserList()
                    where user.Login == username
                    select user.RoleId).FirstOrDefault();
        }

        public bool ExistUser(string username)
        {
            if ((from user in GetUserList()
                    where user.Login.ToLower()==username.ToLower()
                    select user).Count()==0)
                        return false;
            else return true;
        }

        public bool ExistUserWithOutRegister(string username)
        {
            if ((from user in GetUserList()
                 where user.Login == username
                 select user).Count() == 0)
                return false;
            else return true;
        }

        public bool ExistUser(Guid userId)
        {
            if ((from user in GetUserList()
                 where user.UserId == userId
                 select user).Count() == 0)
                return false;
            return true;
        }

        public void ChangeUserRole(string roleName, string userName)
        {
  
            GetUser(userName).RoleId = GetRoleId(roleName);
            _db.SaveChanges();
        }

        public bool ActivateUser(string username, string key)
        {
            var result = from u in GetUserList() where (u.Login == username) select u;

                if (result.Count() != 0)
                {
                    var dbuser = result.First();

                    if (dbuser.EmailKey == key)
                    {
                        dbuser.IsActived = true;
                        _db.SaveChanges();
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                }
                else
                {
                    return false;
                }
            }


        private static string GenerateKey()
        {
            Guid emailKey = Guid.NewGuid();

            return emailKey.ToString();
        }

         private static string CreateSalt()
        {
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            byte[] buff = new byte[32];
            rng.GetBytes(buff);

            return Convert.ToBase64String(buff);
        }

        private static string CreatePasswordHash(string pwd, string salt)
        {
            string saltAndPwd = String.Concat(pwd, salt);
            string hashedPwd =
                    FormsAuthentication.HashPasswordForStoringInConfigFile(
                    saltAndPwd, "md5");
            return hashedPwd;
        }

        public ProfileModel GetUserProfile(Guid userId)
        {
            ProfileModel profile = new ProfileModel();
            profile.user = (ExistUser(GetUser(userId).Login)) ? GetUser(userId).Login : null;
            profile.informationModel = (ExistInformation(GetUser(userId).Login)) ? UserInformation(userId) : null;
            profile.contactModel = (ExistContact(GetUser(userId).Login)) ? GetUserContact(userId) : null;

            return profile;
        }
    }
}