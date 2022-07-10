using SmartMenu.DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartMenu.BAL.Interfaces
{
    public interface IQuickPagesBusiness
    {
        List<QuickPagesVM> GetQuickPages(string connectionStr);     
        int AddUpdateQuickPages(QuickPagesVM model, string connectionStr);
    }
}
