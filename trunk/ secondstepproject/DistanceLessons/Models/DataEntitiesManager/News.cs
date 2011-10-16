using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DistanceLessons.Models
{
    public partial class DataEntitiesManager
    {
        public List<News> GetNewsList()
        {
            return _distancel.News.ToList<News>();
        }
        
        public News GetNew(Guid id)
        {
            return _distancel.News.SingleOrDefault(c => c.NewId == id);
        }

        public void DeleteNew(Guid id)
        {
            var cat = _distancel.News.SingleOrDefault(c => c.NewId == id);
            _distancel.DeleteObject(cat);
            _distancel.SaveChanges();
        }

        public void AddNew(News obj)
        {
            _distancel.News.AddObject(obj);
            _distancel.SaveChanges();
        }
    }
}