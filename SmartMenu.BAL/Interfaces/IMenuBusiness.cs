using SmartMenu.DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartMenu.BAL.Interfaces
{
    public interface IMenuBusiness
    {
        int AddUpdateMenu(MenuItemViewModel model, string connectionStr);
        List<MenuItemViewModel> GetAllMenu(int id, string connectionStr);

        int MarkMenuAsDeleted(int categoryId, string connectionStr);
    }
}
