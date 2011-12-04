using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

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

        public bool ExistMessage(Guid MessageId)
        {
            var result = from m in GetMessageList()
                         where m.MessageId == MessageId
                         select m;
            if (result.Count() > 0) return true;
            return false;
        }

        public bool ExistConfirmCourseMessage(Guid UserId_From, Guid UserId_To, Guid CourseId)
        {
            var result = from m in GetMessageList()
                         where m.UserId_From == UserId_From && m.UserId_To == UserId_To
                         select m;
            if (result.Count() == 0) return false;
            else
            {
                foreach (var item in result)
                {
                    string[] temp = Regex.Split(item.Message1, "(&nbsp;)");
                    foreach (var str in temp)
                    {
                        if ((str == "<a href=\"/profile/confirmsubscribe?CourId=" + CourseId + "&amp;SubscribeUser=" + UserId_From + "&amp;MessId=" + item.MessageId + "\">Затверджую</a>") && (GetMessageStatById(item.MessageId, UserId_To).Status == 10))
                            return true;
                    }
                }
            }
            return false;
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

        public void SendMessage(Guid From, Guid To, String Tit, String Mes)
        {
            Message sender = new Message();
            sender.MessageId = Guid.NewGuid();
            sender.Title = Tit;
            sender.Message1 = Mes;
            sender.DateOfSender = DateTime.Now;
            sender.UserId_From = From;
            sender.UserId_To = To;

            AddMessage(sender);

            AddMessage_status(GetMessageStatus(sender, GetUser(sender.UserId_To).Login), 0);
            AddMessage_status(GetMessageStatus(sender, GetUser(sender.UserId_From).Login), 1);
            
        }


        public void SendMessageToUsers(List<Guid> usersId, Guid from, string title, string message)
        {
            if (usersId.Count == 0) return;
            foreach (Guid userId in usersId)
            {
                SendMessage(from, userId, title, message);
            }
        }

        public void SendMessagesToUsersWhichCanPassModuleInFiveDays()
        {
            string message = "Шановний студент до кінця здачі модуля {0}, з курсу {2}, залишилось {1} днів. Цей лист генерується автоматично від імені викладача курсу на який ви підписались.",
                   title = "Термін здачі модуля";
     
            List<Module> modulesInFiveDays = ModulesEndTestingInFiveDays();
            foreach (Module module in modulesInFiveDays)
            {
                List<Guid> users = SubscribedUsersForCourse(module.CourseId);
                foreach (Guid user in users)
                {
                    if (!IsStartModuleTest(module.ModuleId, user))
                    {

                      //  SendMessage(module.Cours.UserId, user, String.Format(title, module.Title, module.Cours.Title), String.Format(message, module.Title, module.DateEndTesting.Subtract(DateTime.Now).TotalDays));
                        Message sender = new Message();
                        sender.MessageId = Guid.NewGuid();
                        sender.Title = title;
                        sender.Message1 =  String.Format(message, module.Title, (int)module.DateEndTesting.Subtract(DateTime.Now).TotalDays,module.Cours.Title);
                        sender.DateOfSender = DateTime.Now;
                        sender.UserId_From = module.Cours.UserId;
                        sender.UserId_To = user;

                        _db.Messages.AddObject(sender);
                        _db.SaveChanges(); 

                        AddMessage_status(GetMessageStatus(sender, GetUser(sender.UserId_To).Login), 0);
                        AddMessage_status(GetMessageStatus(sender, GetUser(sender.UserId_From).Login), 1);
                    }
                }
            }
        }
    }
}