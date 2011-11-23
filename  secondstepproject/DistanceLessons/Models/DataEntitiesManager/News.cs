using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.Security.Application;

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

            var Query = (from n in GetNewsList() orderby n.Publication descending select n).Skip(pageSize * numPage).Take(pageSize);

            List<News> lst = new List<News>();
            foreach (var i in Query)
                lst.Add(i);

            if (type == 2)
            {
                for (int i = 0; i < lst.Count; i++)
                {
                    if (lst[i].Title.Length > 50) lst[i].Title = lst[i].Title.Substring(0, 47) + "...";
                    if (lst[i].Text.Length > 200) lst[i].Text = lst[i].Text.Substring(0, 197) + "...";
                }
            }
            else if (type == 1)
            {
                for (int i = 0; i < lst.Count; i++)
                {
                    if (lst[i].Text.Length > 500) lst[i].Text = lst[i].Text.Substring(0, 497) + "...";
                }
            }

            for (int current_obj = 0; current_obj < lst.Count; current_obj++)
            {
                string[] temp = Regex.Split(lst[current_obj].Text, "( )|(\r\n)");

                string str = "";
                foreach (string current in temp)
                {
                    if (Regex.IsMatch(current, @"http\://[a-zA-Z0-9\-\.]+\.[a-zA-Z]{2,3}(/\S*)?"))
                        str += current.Replace(current, "<a href=\"" + current + "\" target=\"_new\">url</a>") + " ";
                    else if (Regex.IsMatch(current, @"\r\n"))
                        str += current.Replace(current, "<br />") + " ";
                    else str += current + " ";
                }

                lst[current_obj].Text = str;
            }

            foreach (var newse in lst)
                newse.Text = Sanitizer.GetSafeHtmlFragment(newse.Text);

            return lst;
        }

        public News GetNew_withTag(Guid id)
        {
            News current_obj = _db.News.SingleOrDefault(c => c.NewId == id);

            string[] temp = Regex.Split(current_obj.Text, "( )|(\r\n)");

            string str = "";
            foreach (string current in temp)
            {
                if (Regex.IsMatch(current, @"http\://[a-zA-Z0-9\-\.]+\.[a-zA-Z]{2,3}(/\S*)?"))
                    str += current.Replace(current, "[<a href=\"" + current + "\" target=\"_new\">url</a>]") + " ";
                else if (Regex.IsMatch(current, @"\r\n"))
                    str += current.Replace(current, "<br />") + " ";
                else str += current + " ";
            }

            current_obj.Text = Sanitizer.GetSafeHtmlFragment(str);

            return current_obj;
        }

        public News GetNew(Guid id)
        {
            News current_obj = _db.News.SingleOrDefault(c => c.NewId == id);
            return current_obj;
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