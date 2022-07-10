using SmartMenu.DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartMenu.BAL.Interfaces
{
    public interface IRestaurantBusiness
    {
        int AddUpdateRestaurant(RestaurantModel model, string connectionStr);
        RestaurantModelVM GetRestaurantDetail(string connectionStr);
        int SetOnlineOfflineRestaurant(bool IsOnline, string connectionStr);
        decimal CalculateDistanceBetweenRestaurantAndCustomer(decimal SourceLatitude, decimal SourceLongitude, decimal DestinationLatitude, decimal DestinationLongitude, string connectionStr);
        int UpdateDeliveryAreaSetting(decimal minOrderAmt, decimal maxDeliveryAreaInMiles, string connectionStr);

        int UpdateImages(WebSiteImages model, string connectionStr);
        int AddUpdateDeliveryCharges(decimal tax, decimal deliveryCharges, bool IsCashOnDelivery, string connectionStr);
    }
}
