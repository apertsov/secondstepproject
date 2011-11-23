using System;
using System.Collections.Generic;
using System.Linq;

namespace DistanceLessons.Models
{
    public partial class DataEntitiesManager
    {
        public void AddContact(Contact contact)
        {
            _db.AddToContacts(contact);
            _db.SaveChanges();
        }

        public List<Contact> GetContactList()
        {
            return _db.Contacts.ToList();
        }

        public Contact GetContact(Guid contactId)
        {
            var contact = (from c in GetContactList()
                           where contactId == c.ContactId
                           select c).First();
            return contact;
        }

        public bool ExistContact(string username)
        {
            var result = from u in GetUserList()
                         from c in GetContactList()
                         where u.Login.ToUpper() == username.ToUpper() && u.UserId == c.UserId
                         select c;
            return (result.Count()>0)?true:false;
        }

        public Contact GetUserContact(Guid userId)
        {
            var userContact = (from c in GetContactList()
                               where c.UserId == userId
                               select c).First();
            return userContact;
        }

        //Таня
        public Contact UserContacts(string username)
        {
            var userContacts = (from user in _db.Users
                                from contact in _db.Contacts
                                where user.Login == username && user.UserId == contact.UserId
                                select contact).FirstOrDefault();
            return userContacts;
        }

        public bool ExistContacts(string username)
        {
            var result = from user in GetUserList()
                         from contact in GetContactList()
                         where user.Login.ToUpper() == username.ToUpper() && user.UserId == contact.UserId
                         select contact;
            if (result.Count() > 0) return true;
            return false;
        }
    }
}