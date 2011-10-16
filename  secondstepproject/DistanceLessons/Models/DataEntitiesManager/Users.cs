using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DistanceLessons.Models
{
    public partial class DataEntitiesManager
    {
        public List<User> GetUserList()
        {
            return _distancel.Users.ToList<User>();
        }
        
        public User GetUser(Guid id)
        {
            return _distancel.Users.SingleOrDefault(c => c.UserId == id);
        }


        public void DeleteUser(Guid id)
        {
            var cat = _distancel.Users.SingleOrDefault(c => c.UserId == id);
            _distancel.DeleteObject(cat);
            _distancel.SaveChanges();
        }

        public void AddUser(User obj)
        {
            _distancel.Users.AddObject(obj);
            _distancel.SaveChanges();
        }

        public List<RQTeachers> QTeachers()
        {
            var ID=new System.Guid("3de24e0a-5ead-499a-9d4c-d20ad1651cc1");
            var Query =
                (
                from cou in GetCourseList()
                from u in GetUserList()
                from i in GetInfoList()
                where u.UserId == i.UserId && u.UserId == cou.UserId && u.Role.ToString()==ID.ToString()
                orderby i.FirstName
                select new RQTeachers
                {
                    id = u.UserId,
                    login = u.Login,
                    first = i.FirstName,
                    last = i.LastName,
                    mid = i.MidName,
                    course = cou.Title
                }
                ).ToList<RQTeachers>();

            List<RQTeachers> lst = new List<RQTeachers>();
            foreach (var i in Query)
                lst.Add(i);

            return lst;
        }
    }
}