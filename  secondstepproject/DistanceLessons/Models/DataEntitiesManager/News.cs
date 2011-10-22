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
            return _db.News.ToList<News>();
        }

        public List<News> GetThreeNews()
        {
            List<News> temp = new List<News>();
            for (int i = GetNewsList().Count; i > GetNewsList().Count - 3; i--)
                temp.Add(GetNewsList()[i]);
            return temp;
        }

        public News GetNew(Guid id)
        {
            return _db.News.SingleOrDefault(c => c.NewId == id);
        }

        public void DeleteNew(Guid id)
        {
            var cat = _db.News.SingleOrDefault(c => c.NewId == id);
            _db.DeleteObject(cat);
            _db.SaveChanges();
        }

        public void AddNew(News obj)
        {
            _db.News.AddObject(obj);
            _db.SaveChanges();
        }
    }
}