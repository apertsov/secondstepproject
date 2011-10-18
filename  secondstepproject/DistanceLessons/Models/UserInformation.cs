using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DistanceLessons.Models
{
    public class UserInformation
    {
        private dbEntities db { get; set; }

        public UserInformation()
        {
            db = new dbEntities();
        }

        public void AddInfo(InformationModel info, ContactsModel contacts)
        {
            db.AddToInformations(new Information { InformationId = info.InformationId, FirstName = info.FirstName, LastName = info.LastName, MidName = info.LastName, DateOfBirth = info.DateOfBirth, About=info.About, UserId = info.UserId });
            db.AddToContacts(new Contact { ContactId = contacts.ContactId, ICQ_QIP = contacts.ICQ, Skype = contacts.Skype, Other = contacts.Other, Telephone = contacts.Phone, UserId = contacts.UserId });
            db.SaveChanges();
        }
    }
}