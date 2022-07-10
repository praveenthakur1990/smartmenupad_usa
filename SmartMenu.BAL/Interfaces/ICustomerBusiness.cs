using SmartMenu.DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartMenu.BAL.Interfaces
{
    public interface ICustomerBusiness
    {
        int AddUpdateCustomerAddress(CustomerAddressesViewModel model, string connectionStr);
        List<CustomerViewModel> GetCustomerList(int customerId, string mobileNumber, PaginationModel pageModel, string connectionStr);      
    }
}
