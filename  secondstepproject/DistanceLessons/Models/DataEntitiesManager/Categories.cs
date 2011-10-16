using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DistanceLessons.Models
{
    public partial class DataEntitiesManager
    {
        public List<Category> GetCategoryList()
        {
            return _distancel.Categories.ToList<Category>();
        }

        public Category GetCategory(Guid id)
        {
            return _distancel.Categories.SingleOrDefault(c => c.CategoryId == id);
        }

        public void DeleteCategory(Guid id)
        {
            var cat = _distancel.Categories.SingleOrDefault(c => c.CategoryId == id);
            _distancel.DeleteObject(cat);
            _distancel.SaveChanges();
        }

        public void AddCategory(Category obj)
        {
            _distancel.Categories.AddObject(obj);
            _distancel.SaveChanges();
        }

        public List<RQCategorys> QCategorys()
        {
            var Query =
                (
                from cat in GetCategoryList()
                select new RQCategorys
                {
                    id = cat.CategoryId,
                    title = cat.Category1
                }
                ).ToList<RQCategorys>();

            List<RQCategorys> lst = new List<RQCategorys>();
            foreach (var i in Query)
                lst.Add(i);

            return lst;
        }
    }
}