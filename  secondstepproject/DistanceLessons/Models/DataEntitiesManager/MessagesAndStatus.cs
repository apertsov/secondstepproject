using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DistanceLessons.Models
{
    public partial class DataEntitiesManager
    {

        public List<Message> GetMessagesByUser(string username, byte type)
        {
            // type - >2 - все, 1 - вихідні, 2 - вихідні
            Guid userId = GetUserId(username);
            switch (type)
            {
                case 1:
                    return (from m in GetMessageList() where m.UserId_To == userId orderby m.DateOfSender descending select m).ToList();
                case 2:
                    return (from m in GetMessageList() where m.UserId_From == userId orderby m.DateOfSender descending select m).ToList();
                default:
                    return (from m in GetMessageList() where m.UserId_To == userId || m.UserId_From == userId orderby m.DateOfSender descending select m).ToList();
            }
        }

        public Message_status GetMessageStatus(Message obj, string User)
        {
            Message_status temp = new Message_status();
            temp.MessageStatusId = Guid.NewGuid();
            temp.UserId = GetUserId(User);
            temp.MessageId = obj.MessageId;
            return temp;
        }

        public void AddMessage_status(Message_status obj)
        {
            _db.Message_status.AddObject(obj);
            Save();
        }
    }
}