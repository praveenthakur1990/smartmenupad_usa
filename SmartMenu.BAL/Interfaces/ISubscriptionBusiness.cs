using SmartMenu.DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartMenu.BAL.Interfaces
{
    public interface ISubscriptionBusiness
    {
        int AddUpDateSubscription(SubscriptionModel model, string connectionStr);
        SubscriptionInfoModel GetActiveSubscriptionInfo(string connectionStr);
        string GetStripeSessionId(string userId, string connectionStr);

        int UpdateSubscriptionCancel(bool IsCancelled, string connectionStr);

    }
}
