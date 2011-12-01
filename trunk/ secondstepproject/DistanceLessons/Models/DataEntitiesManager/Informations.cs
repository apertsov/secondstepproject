using System;
using System.Collections.Generic;
using System.Linq;

namespace DistanceLessons.Models
{
    public partial class DataEntitiesManager
    {
        public void AddInformation(Information info)
        {
            _db.AddToInformations(info);
            _db.SaveChanges();
        }

        public void UpdateInformation(Information newInfo)
        {
            Information oldInfo = UserInformation(newInfo.UserId);
            oldInfo.FirstName = newInfo.FirstName;
            oldInfo.LastName = newInfo.LastName;
            oldInfo.MidName = newInfo.MidName;
            oldInfo.DateOfBirth = newInfo.DateOfBirth;
            oldInfo.About = newInfo.About;
            oldInfo.Avatar = newInfo.Avatar;
            _db.SaveChanges();
        }

        public bool ExistInformation(string username)
        {
            var result = from user in GetUserList()
                         from info in GetInfoList()
                         where user.Login.ToUpper() == username.ToUpper() && user.UserId == info.UserId
                         select info;
            if (result.Count() > 0) return true;
            return false;
        }

        public Information UserInformation(string username)
        {
            var userInfo = (from user in GetUserList()
                            from info in GetInfoList()
                            where user.Login.ToUpper() == username.ToUpper() && user.UserId == info.UserId
                            select info).FirstOrDefault();
            return userInfo;
        }

        public Information GetInformation(Guid infoId)
        {
            var userInfo = (from info in GetInfoList()
                            where infoId == info.InformationId
                            select info).First();
            return userInfo;
        }

        public Information UserInformation(Guid userID)
        {
            var userInfo = (from info in GetInfoList()
                            where userID == info.UserId
                            select info).FirstOrDefault();
            return userInfo;
        }

        public List<Information> GetInfoList()
        {
            return _db.Informations.ToList<Information>();
        }
    }
}