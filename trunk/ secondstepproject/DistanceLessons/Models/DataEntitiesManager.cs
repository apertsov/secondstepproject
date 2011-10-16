using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace DistanceLessons.Models
{
    public partial class DataEntitiesManager
    {
        private dbEntities _db { get; set; }

        public DataEntitiesManager()
        {
            _db = new dbEntities();
        }

        public void Save()
        {
            _db.SaveChanges();
        }
    }
}