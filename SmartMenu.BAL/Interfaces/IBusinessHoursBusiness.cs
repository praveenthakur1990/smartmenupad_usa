using SmartMenu.DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartMenu.BAL.Interfaces
{
    public interface IBusinessHoursBusiness
    {
        List<BusinessHoursModelVM> GetBusinessHours(string connectionStr);
        int AddUpdateBusinessHours(string businessHourksonStr, string connectionStr);
        int AddUpdatePickupAddress(string addresses, string connectionStr);

        int AddCustomLabel(string jsonStr, string connectionStr);
    }
}
