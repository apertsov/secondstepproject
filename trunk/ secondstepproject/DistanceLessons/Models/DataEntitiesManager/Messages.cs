using System;
using System.Collections.Generic;
using System.Linq;

namespace DistanceLessons.Models
{
    public partial class DataEntitiesManager
    {
        public List<Message> GetMessageList()
        {
            return _db.Messages.ToList<Message>();
        }

        public List<Message> GetMessageList_UserID(string username, byte type)
        {
            // type - 0 - вхідні, інше - вихідні
            Guid userId = GetUserId(username);
            if (type == 0)
            {
                var Query =
                    (
                    from m in GetMessageList()
                    where m.UserId_To == userId
                    orderby m.DateOfSender descending
                    select m
                    ).ToList<Message>();

                List<Message> lst = new List<Message>();
                foreach (var i in Query)
                    lst.Add(i);

                return lst;
            }
            else
            {
                var Query =
                 (
                 from m in GetMessageList()
                 where m.UserId_From == userId
                 orderby m.DateOfSender descending
                 select m
                 ).ToList<Message>();

                List<Message> lst = new List<Message>();
                foreach (var i in Query)
                    lst.Add(i);

                return lst;
            }
        }

        public void AddMessage(Message obj)
        {
            _db.Messages.AddObject(obj);
            Save();
        }

        public void DeleteMessageOwner(Message message)
        {
            /*if (message.Status != 3)
            {
                message.Status = 2;
                Save();
            }
            else
            {
                DeleteMessage(message.MessageId);
            }*/
            
        }

        public void DeleteMessageRecipient(Message message)
        {
            /*
            if (message.Status != 2)
            {
                message.Status = 3;
                Save();
            }
            else
            {
                DeleteMessage(message.MessageId);
            }
             * */
        }

        public void DeleteMessage(Guid id)
        {
            var cat = _db.Messages.SingleOrDefault(c => c.MessageId == id);
            //cat.Status = 2;
            _db.DeleteObject(cat);
            Save();
        }

        public void UpdateMessageStatus(Message message)
        {
            //message.Status = 0;

            Save();
        }
    }
}