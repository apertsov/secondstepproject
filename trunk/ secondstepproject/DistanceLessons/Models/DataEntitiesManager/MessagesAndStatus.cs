using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Security.Application;

namespace DistanceLessons.Models
{
    public partial class DataEntitiesManager
    {
        public List<Message_status> GetMessageStatusList()
        {
            return _db.Message_status.ToList<Message_status>();
        }

        public Message_status GetMessageStatById(Guid MesId, Guid userId)
        {
            Message_status a = (from m in GetMessageStatusList()
                                where m.MessageId == MesId && m.UserId == userId
                                select m).FirstOrDefault();
            if (a != null) a.Message.Message1 = Sanitizer.GetSafeHtmlFragment(a.Message.Message1);
            return a;
        }

        public List<MessageStatusWithAuthorModel> GetMessagesByUser(string username, byte type)
        {
            // type - >2 - всі, 1 - вхідні, 2 - вихідні
            Guid userId = GetUserId(username);
            List<Message> MessageList = new List<Message>();
            switch (type)
            {
                case 1:
                    { MessageList = (from m in GetMessageList() where m.UserId_To == userId orderby m.DateOfSender descending select m).ToList(); break; }
                case 2:
                    { MessageList = (from m in GetMessageList() where m.UserId_From == userId orderby m.DateOfSender descending select m).ToList(); break; }
                default:
                    { MessageList = (from m in GetMessageList() where m.UserId_To == userId || m.UserId_From == userId orderby m.DateOfSender descending select m).ToList(); break; }
            }

            List<MessageStatusWithAuthorModel> result = new List<MessageStatusWithAuthorModel>();
            foreach (var item in MessageList)
            {
                MessageStatusWithAuthorModel temp = new MessageStatusWithAuthorModel();
                temp.Messages = GetMessageStatById(item.MessageId, userId);
                //temp.Messages.Message.Message1= Sanitizer.GetSafeHtmlFragment(item.Message1);
                temp.Author = (temp.Messages.Message.UserId_From == userId) ? true : false;
                temp.username = (temp.Author) ? (GetUser(temp.Messages.Message.UserId_To).Login) : (GetUser(temp.Messages.Message.UserId_From).Login);

                result.Add(temp);
            }

            return result;
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

        public void AddMessage_status(Message_status obj, byte status)
        {
            obj.Status = status;
            _db.Message_status.AddObject(obj);
            Save();
        }

        public List<MessageStatusWithAuthorModel> GetNewMessageForUser(string username)
        {
            List<MessageStatusWithAuthorModel> temp = new List<MessageStatusWithAuthorModel>();
            foreach (var item in GetMessagesByUser(username,1))
            {
                if((item.Messages.Status==0)||(item.Messages.Status==10))
                    temp.Add(item);
            }
            return temp;
        }
    }
}