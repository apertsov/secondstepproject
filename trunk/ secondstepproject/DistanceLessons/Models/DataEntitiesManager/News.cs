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

        public List<News> GetNewsList_time()
        {
            var Query =  from n in GetNewsList() orderby n.Publication descending select n;
           
            List<News> lst = new List<News>();
            foreach (var i in Query)
                lst.Add(i);

            return lst;
        }

        public List<News> GetNewsThree()
        {
            List<News> temp = new List<News>();

            int j = 1;
            foreach (News _new in GetNewsList_time())
            {
                if (j <= 3) { temp.Add(_new); j++; }
                else break;
            }

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