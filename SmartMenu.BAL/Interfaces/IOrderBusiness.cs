using SmartMenu.DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartMenu.BAL.Interfaces
{
    public interface IOrderBusiness
    {
        int AddUpdateOrder(OrderModel model, string connectionStr);
        List<OrderViewModel> GetOrderList(int orderId, string orderNo, string status, bool IsCurrentDate, PaginationModel pageModel, string connectionStr);
        string GenerateOrderNumber(string connectionStr);
        int UpdateOrderStatus(OrderViewModel model, string connectionStr);
        List<SalesOrderGraphVM> GetDashBoardGraphData(SalesOrderGraphRequestVM request, string connectionStr);
        DashboardStatDataVM GetDashBoardStatsData(string connectionStr);
        List<PickupAvailabilityModel> CheckPickupDateTimeAvailability(string pickupDateTime, string connectionStr);
    }
}
