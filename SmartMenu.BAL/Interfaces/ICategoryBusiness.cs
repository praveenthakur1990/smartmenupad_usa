using SmartMenu.DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartMenu.BAL.Interfaces
{
    public interface ICategoryBusiness
    {
        int AddUpdateCategory(CategoryModel model, string connectionStr);
        List<CategoryModel> GetCategories(string type, string connectionStr);
        int MarkCategoryAsDeleted(int categoryId, string connectionStr);
    }
}
