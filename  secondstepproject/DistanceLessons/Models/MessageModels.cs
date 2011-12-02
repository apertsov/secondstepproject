using System;

namespace DistanceLessons.Models
{
    public partial class SendMessageModel
    {
        public Message Messages { get; set; }
        public String Name { get; set; }
    }

    public class MessageStatusWithAuthorModel
    {
        public Message_status Messages { get; set; }
        // true = from
        // false = to
        public bool Author { get; set; }
        public string username { get; set; }
    }
}