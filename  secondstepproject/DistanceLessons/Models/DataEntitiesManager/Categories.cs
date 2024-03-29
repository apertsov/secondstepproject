﻿using System;
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

        public List<Category> GetCategoryListASC()
        {
            return (from c in GetCategoryList()
                    orderby c.Category1 ascending
                    select c).ToList();
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


        public Dictionary<Guid, string> CategoriesDictionaryAll()
        {
            var categories = (from categorys in GetCategoryList()
                              select new { courseId = categorys.CategoryId, title = categorys.Category1 });
            Dictionary<Guid, string> allCategories = new Dictionary<Guid, string>();
            foreach (var category in categories)
                allCategories.Add(category.courseId, category.title);
            return allCategories;
        }

        public Dictionary<Guid, string> CategoryDictionary(Guid categoryId)
        {
            var category = (from categorys in GetCategoryList()
                            where categorys.CategoryId == categoryId
                            select new { courseId = categorys.CategoryId, title = categorys.Category1 }).FirstOrDefault();
            if (category == null) return null;
            Dictionary<Guid, string> categoryDictionary = new Dictionary<Guid, string>();
            categoryDictionary.Add(category.courseId, category.title);
            return categoryDictionary;
        }

        public List<Category> GetCategoriesList(List<UserCours> lst)
        {
            List<Category> temp = new List<Category>();
            foreach (var item in lst)
            {
                if ((temp.Find(m => m == item.Cours.Category) == null))
                    temp.Add(item.Cours.Category);
            }
            return temp;
        }

        public bool ExistCategory(Guid categoryId)
        {
            return (from categories in GetCategoryList()
                    where categories.CategoryId == categoryId
                    select categories).Count() > 0 ? true : false;
        }
    }
}