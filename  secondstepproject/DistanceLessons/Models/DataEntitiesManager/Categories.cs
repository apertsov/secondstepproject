using System;
using System.Collections.Generic;
using System.Linq;

namespace DistanceLessons.Models
{
    public partial class DataEntitiesManager
    {
        public List<Category> GetCategoryList()
        {
            return _db.Categories.ToList<Category>();
        }

        public Category GetCategory(Guid id)
        {
            return _db.Categories.SingleOrDefault(c => c.CategoryId == id);
        }

        public void DeleteCategory(Guid id)
        {
            var cat = _db.Categories.SingleOrDefault(c => c.CategoryId == id);
            _db.DeleteObject(cat);
            _db.SaveChanges();
        }

        public void AddCategory(Category obj)
        {
            _db.Categories.AddObject(obj);
            _db.SaveChanges();
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