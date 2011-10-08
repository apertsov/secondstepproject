using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Security.Cryptography;

namespace Distance_Lesson.Models
{
    public class UserRepository
    {

        private dbEntities _db;
        public dbEntities db
        {
            get
            {
                if (_db == null) _db = new dbEntities();
                return _db;
            }
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

        public MembershipUser CreateUser(string username, string password, string email)
        {
            using (dbEntities db = new dbEntities())
            {
                User user = new User();

                username = username.Trim();
                password = password.Trim();
                email = email.Trim();

                user.Login = username;
                user.Email = email;
                user.Role = 1;
                user.PasswordSalt = CreateSalt();
                user.CreatedData = DateTime.Now;
                user.IsActived = true;
                user.IsLockedOut = false;
                user.LastLoginData = DateTime.Now;
                user.Password = CreatePasswordHash(password, user.PasswordSalt);
                db.AddToUsers(user);
                db.SaveChanges();

                return GetUser(username);
            }
        }

        public string GetUserNameByEmail(string email)
        {
            
                var result = from u in db.Users where (u.Email == email) select u;

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

        public MembershipUser GetUser(string username)
        {
                var result = from u in db.Users where (u.Login == username) select u;

                if (result.Count() != 0)
                {
                    var dbuser = result.FirstOrDefault();

                    string _username = dbuser.Login;
                    Guid _providerUserKey = dbuser.UserId;
                    string _email = dbuser.Email;
                    int _permissions = dbuser.Role;
                    string _passwordQuestion = "";
                    string _comment = "";
                    bool _isApproved = dbuser.IsActived;
                    bool _isLockedOut = dbuser.IsLockedOut;
                    DateTime _creationDate = dbuser.CreatedData;
                    DateTime _lastLoginDate = dbuser.LastLoginData;
                    DateTime _lastActivityDate = DateTime.Now;
                    DateTime _lastPasswordChangedDate = DateTime.Now;
                    DateTime _lastLockedOutDate = DateTime.Now;

                    MembershipUser user = new MembershipUser("CustomMembershipProvider",
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
            var dbuser = db.Users.FirstOrDefault(x => x.Login == username);
            return dbuser != null && dbuser.Password == CreatePasswordHash(password, dbuser.PasswordSalt);
        }
    }
}