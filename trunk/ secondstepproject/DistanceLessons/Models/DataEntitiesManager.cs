using System.Data.Entity;

namespace DistanceLessons.Models
{
    public partial class DataEntitiesManager : DbContext
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