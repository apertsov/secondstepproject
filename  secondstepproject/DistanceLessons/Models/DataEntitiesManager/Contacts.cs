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
        public List<Contact> GetContactList()
        {
            return _db.Contacts.ToList<Contact>();
        }
    }
}