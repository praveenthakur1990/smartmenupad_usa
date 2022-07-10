using SmartMenu.DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartMenu.BAL.Interfaces
{
    public interface IPaymentBusiness
    {
        int AddPaymentInfo(PaymentModel model, string connectionStr);
     }
}
