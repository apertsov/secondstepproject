using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace DistanceLessons.Models
{
    public partial class DataEntitiesManager
    {
        private dbEntities _distancel { get; set; }

        public DataEntitiesManager()
        {
            _distancel = new dbEntities();
        }

        public void Save()
        {
            _distancel.SaveChanges();
        }
    }
}