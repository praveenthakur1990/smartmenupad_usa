using SmartMenu.DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartMenu.BAL.Interfaces
{
    public interface IAPIKeyBusiness
    {
        int AddUpdateAPIKey(APIKeyModel model, string connectionStr);
        APIKeyModel GetAPIKey(string connectionStr);
    }
}
