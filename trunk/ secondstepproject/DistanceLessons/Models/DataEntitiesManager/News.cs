using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace DistanceLessons.Models
{
    public partial class DataEntitiesManager
    {
        public List<News> GetNewsList()
        {
            return _db.News.ToList<News>();
        }

        public List<News> GetNewsList_time(int pageSize, int numPage, byte type)
        {
            //type - 1 - 500char
            //type - 2 - 200char

            var Query =  (from n in GetNewsList() orderby n.Publication descending select n).Skip(pageSize*numPage).Take(pageSize);
           
            List<News> lst = new List<News>();
            foreach (var i in Query)
                lst.Add(i);
            
            if (type==2)
            {
                for (int i = 0; i < lst.Count; i++)
                {
                    if (lst[i].Text.Length>200) lst[i].Text = lst[i].Text.Substring(0, 197) + "...";
                }
            }
            else if (type==1)
                {
                    for (int i = 0; i < lst.Count; i++)
                    {
                        if (lst[i].Text.Length > 500) lst[i].Text = lst[i].Text.Substring(0, 497) + "...";
                    }
                }


            for (int i = 0; i < lst.Count;i++ )
            {
                lst[i].Text = Regex.Replace(lst[i].Text, @"http\://[a-zA-Z0-9\-\.]+\.[a-zA-Z]{2,3}(/\S*)?", "<a href='http://"+"'>[url]</a>");
            }
                return lst;
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