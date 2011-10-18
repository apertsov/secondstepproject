using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DistanceLessons.Models
{
    public partial class DataEntitiesManager
    {
        public void AddContact(Contact contact)
        {
            _db.AddToContacts(contact);
            _db.SaveChanges();
        }

        public Contact UserContacts(string username)
        {
            var userContacts = (from user in _db.Users
                            from info in _db.Contacts
                            where user.Login == username && user.UserId == info.UserId
                            select info).First();
            return userContacts;
        }
        public List<Contact> GetContactList()
        {
            return _db.Contacts.ToList<Contact>();
        }
    }
}