using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DistanceLessons.Models
{
    public partial class DataEntitiesManager
    {
        public List<Message> GetMessageList()
        {
            return _db.Messages.ToList<Message>();
        }
    }
}