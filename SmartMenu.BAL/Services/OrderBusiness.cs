using Newtonsoft.Json;
using SmartMenu.BAL.Interfaces;
using SmartMenu.DAL.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace SmartMenu.BAL.Services
{
    public class OrderBusiness : IOrderBusiness
    {
        public int AddUpdateOrder(OrderModel model, string connectionStr)
        {
            using (SqlConnection connection = new SqlConnection(connectionStr))
            {
                int response = 0;
                try
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand("Exec [dbo].[USPAddUpdateOrder] @CustomerAddressId, @OrderNo, @Mode, @Status, @OrderedType, @PickUpDateTime, @PickUpAddress, @OrderedDetails, @SpecialInstruction, @TaxRate, @TaxAmt, @DeliveryCharges, @Result", connection))
                    {
                        command.Parameters.AddWithValue("@Id", model.Id);
                        command.Parameters.AddWithValue("@CustomerAddressId", model.CustomerAddressId);
                        command.Parameters.AddWithValue("@OrderNo", model.OrderNo);
                        command.Parameters.AddWithValue("@Mode", model.Mode);
                        command.Parameters.AddWithValue("@Status", model.Status);
                        command.Parameters.AddWithValue("@OrderedType", model.OrderedType == null ? "D" : model.OrderedType);
                        if (model.PickUpDateTime == null)
                        {
                            command.Parameters.AddWithValue("@PickUpDateTime", DBNull.Value);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@PickUpDateTime", model.PickUpDateTime);
                        }
                        command.Parameters.AddWithValue("@PickUpAddress", model.PickUpAddress == null ? string.Empty : model.PickUpAddress);
                        command.Parameters.AddWithValue("@OrderedDetails", model.OrderedDetails);
                        command.Parameters.AddWithValue("@SpecialInstruction", model.SpecialInstruction);
                        command.Parameters.AddWithValue("@TaxRate", model.TaxRate);
                        command.Parameters.AddWithValue("@TaxAmt", model.TaxAmt);
                        command.Parameters.AddWithValue("@DeliveryCharges", model.DeliveryCharges);
                        command.Parameters.Add("@Result", SqlDbType.Int);
                        command.Parameters["@Result"].Direction = ParameterDirection.Output;
                        response = Convert.ToInt32(command.ExecuteScalar());
                    }
                    connection.Close();
                    return response;
                }
                catch (Exception ex)
                {
                    return 0;
                }
            }
        }
        public string GenerateOrderNumber(string connectionStr)
        {
            using (SqlConnection connection = new SqlConnection(connectionStr))
            {
                string response = string.Empty;
                try
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand("Exec [dbo].[USPGetOrderNumber] @NextOrderNumber", connection))
                    {
                        command.Parameters.Add("@NextOrderNumber", SqlDbType.NVarChar, 20);
                        command.Parameters["@NextOrderNumber"].Direction = ParameterDirection.Output;
                        response = command.ExecuteScalar().ToString();
                    }
                    connection.Close();
                    return response;
                }
                catch (Exception ex)
                {
                    return string.Empty;
                }
            }
        }
        public List<SalesOrderGraphVM> GetDashBoardGraphData(SalesOrderGraphRequestVM request, string connectionStr)
        {
            using (SqlConnection connection = new SqlConnection(connectionStr))
            {
                List<SalesOrderGraphVM> objList = new List<SalesOrderGraphVM>();
                SalesOrderGraphVM obj = null;
                try
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand("Exec [dbo].[USPCalculateOrderGraph_v1] @OrderStatus, @PaymentMode, @MonthId, @Year", connection))
                    {
                        command.Parameters.AddWithValue("@OrderStatus", request.OrderStatus == null ? string.Empty : request.OrderStatus);
                        command.Parameters.AddWithValue("@PaymentMode", request.PaymentMode == null ? string.Empty : request.PaymentMode);
                        command.Parameters.AddWithValue("@MonthId", request.MonthId);
                        command.Parameters.AddWithValue("@Year", request.Year);
                        SqlDataReader reader = command.ExecuteReader();
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                obj = new SalesOrderGraphVM();
                                obj.MonthName = reader["Month"].ToString();
                                obj.Year = reader["Year"].ToString();
                                obj.OrderDate = reader["OrderDate"].ToString();
                                obj.TotalSaleAmount = Convert.ToDecimal(reader["TotalSaleAmount"].ToString());
                                obj.TotalPickUpAmount = Convert.ToDecimal(reader["TotalPickUpAmount"].ToString());
                                obj.TotalDeliveredAmount = Convert.ToDecimal(reader["TotalDeliveredAmount"].ToString());
                                obj.TotalCount = Convert.ToInt32(reader["TotalCount"].ToString());
                                obj.TotalPickUpCount = Convert.ToInt32(reader["TotalPickUpCount"].ToString());
                                obj.TotalDeliveredCount = Convert.ToInt32(reader["TotalDeliveredCount"].ToString());
                                obj.CardPaymentMode = Convert.ToInt32(reader["CardPaymentMode"].ToString());
                                obj.CashPaymentMode = Convert.ToInt32(reader["CashPaymentMode"].ToString());
                                objList.Add(obj);
                            }
                        }
                    }
                    connection.Close();
                    return objList;
                }
                catch (Exception ex)
                {
                    return new List<SalesOrderGraphVM>();
                }
            }
        }
        public DashboardStatDataVM GetDashBoardStatsData(string connectionStr)
        {
            using (SqlConnection connection = new SqlConnection(connectionStr))
            {
                DashboardStatDataVM obj = new DashboardStatDataVM();
                try
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand("Exec [dbo].[GetDashBoardStatsData]", connection))
                    {
                        SqlDataReader reader = command.ExecuteReader();
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                obj = new DashboardStatDataVM();
                                obj.TotalOrder = Convert.ToInt32(reader["TotalOrder"].ToString());
                                obj.TotalCustomer = Convert.ToInt32(reader["TotalCutomer"].ToString());
                                obj.TotalMenuItem = Convert.ToInt32(reader["TotalMenuItem"].ToString());
                            }
                        }
                    }
                    connection.Close();
                    return obj;
                }
                catch (Exception ex)
                {
                    return new DashboardStatDataVM();
                }
            }
        }
        public List<OrderViewModel> GetOrderList(int orderId, string orderNo, string status, bool IsCurrentDate, PaginationModel pageModel, string connectionStr)
        {
            using (SqlConnection connection = new SqlConnection(connectionStr))
            {
                List<OrderViewModel> objOrderList = new List<OrderViewModel>();
                OrderViewModel obj = null;
                try
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand("Exec [dbo].[USPGetOrderList_v4] @OrderId, @OrderNo, @Status, @IsCurrentDate , @PageNumber, @PageSize, @SearchStr", connection))
                    {
                        command.Parameters.AddWithValue("@OrderId", orderId);
                        command.Parameters.AddWithValue("@OrderNo", orderNo == null ? string.Empty : orderNo);
                        command.Parameters.AddWithValue("@Status", status == null ? string.Empty : status);
                        command.Parameters.AddWithValue("@IsCurrentDate", IsCurrentDate);
                        command.Parameters.AddWithValue("@PageNumber", pageModel.PageNumber == 0 ? 1 : pageModel.PageNumber);
                        command.Parameters.AddWithValue("@PageSize", pageModel.PageSize);
                        command.Parameters.AddWithValue("@SearchStr", pageModel.SearchStr == null ? string.Empty : pageModel.SearchStr);
                        SqlDataReader reader = command.ExecuteReader();
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                obj = new OrderViewModel();
                                obj.Id = Convert.ToInt32(reader["Id"].ToString());
                                obj.CustomerId = Convert.ToInt32(reader["CustomerId"].ToString());
                                obj.CustomerName = reader["CustomerName"].ToString();
                                obj.CustomerEmail = reader["CustomerEmail"].ToString();
                                obj.MobileNumber = reader["CustomerMobile"].ToString();
                                obj.OrderNo = reader["OrderNo"].ToString();
                                obj.Mode = reader["Mode"].ToString();
                                obj.Status = reader["OrderStatus"].ToString();
                                obj.OrderedType = reader["OrderedType"].ToString();
                                if (reader["PickUpDateTime"] is DBNull)
                                {
                                    obj.PickUpDateTime = null;
                                }
                                else
                                {
                                    obj.PickUpDateTime = Convert.ToDateTime(reader["PickUpDateTime"].ToString());
                                }
                                obj.PickUpAddress = reader["PickUpAddress"] is DBNull ? string.Empty : reader["PickUpAddress"].ToString();
                                obj.OrderStatus = reader["OrderStatus"].ToString();
                                obj.OrderedDate = Convert.ToDateTime(reader["OrderedDate"].ToString());
                                obj.OrderedDetails = JsonConvert.DeserializeObject<List<MenuCartModel>>(reader["OrderedDetails"].ToString());
                                if (reader["SpecialInstruction"] is DBNull)
                                {
                                    obj.SpecialInstruction = string.Empty;
                                }
                                else
                                {
                                    obj.SpecialInstruction = reader["SpecialInstruction"].ToString();
                                }

                                obj.CapturedId = reader["CapturedId"].ToString();
                                obj.PaymentMethod = reader["PaymentMethod"].ToString();
                                if (reader["TransactionDate"] is DBNull)
                                {
                                    obj.TransactionDate = null;
                                }
                                else
                                {
                                    obj.TransactionDate = Convert.ToDateTime(reader["TransactionDate"].ToString());
                                }
                                obj.Funding = reader["Funding"].ToString();
                                obj.CapturedAmt = Convert.ToDecimal(reader["CapturedAmt"].ToString());
                                obj.PaymentStatus = reader["PaymentStatus"].ToString();
                                obj.CustomerAddress = reader["CustomerAddress"].ToString();

                                if (reader["LastUpdatedDate"] is DBNull)
                                {
                                    obj.LastUpdatedDate = null;
                                }
                                else
                                {
                                    obj.LastUpdatedDate = Convert.ToDateTime(reader["LastUpdatedDate"].ToString());
                                }
                                obj.LastUpdatedStatus = reader["LastUpdatedStatus"].ToString();
                                obj.OrderStatusLogList = (reader["OrderStatusLogJsonStr"] is DBNull ? new List<OrderStatusLogsVM>() : JsonConvert.DeserializeObject<List<OrderStatusLogsVM>>("[" + reader["OrderStatusLogJsonStr"].ToString() + "]"));
                                obj.TotalRows = Convert.ToInt32(reader["TotalRows"].ToString());

                                obj.TaxRate = Convert.ToDecimal(reader["TaxRate"].ToString());
                                obj.TaxAmt = Convert.ToDecimal(reader["TaxAmt"].ToString());
                                obj.DeliveryCharges = Convert.ToDecimal(reader["DeliveryCharges"].ToString());
                                objOrderList.Add(obj);
                            }
                        }
                    }
                    connection.Close();
                    return objOrderList;
                }
                catch (Exception ex)
                {
                    return new List<OrderViewModel>();
                }
            }
        }
        public int UpdateOrderStatus(OrderViewModel model, string connectionStr)
        {
            using (SqlConnection connection = new SqlConnection(connectionStr))
            {
                int response = 0;
                try
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand("Exec [dbo].[USPOrderStatusLogs] @OrderId, @Status, @ChangedBy, @Result", connection))
                    {
                        command.Parameters.AddWithValue("@OrderId", model.Id);
                        command.Parameters.AddWithValue("@Status", model.OrderStatus);
                        command.Parameters.AddWithValue("@ChangedBy", model.StatusChangedBy);
                        command.Parameters.Add("@Result", SqlDbType.Int);
                        command.Parameters["@Result"].Direction = ParameterDirection.Output;
                        response = Convert.ToInt32(command.ExecuteScalar());
                    }
                    connection.Close();
                    return response;
                }
                catch (Exception ex)
                {
                    return 0;
                }
            }
        }

        public List<PickupAvailabilityModel> CheckPickupDateTimeAvailability(string pickupDateTime, string connectionStr)
        {
            using (SqlConnection connection = new SqlConnection(connectionStr))
            {
                List<PickupAvailabilityModel> objList = new List<PickupAvailabilityModel>();
                PickupAvailabilityModel obj = null;
                try
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand("Exec [dbo].[USPCheckPickupAvailability] @PickupTime", connection))
                    {
                        command.Parameters.AddWithValue("@PickupTime", pickupDateTime);
                        SqlDataReader reader = command.ExecuteReader();
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                obj = new PickupAvailabilityModel();
                                obj.PickupDate = reader["PickupDate"].ToString();
                                obj.HourOpenTime = reader["HourOpenTime"].ToString();
                                obj.HourCloseTime = reader["HourCloseTime"].ToString();
                                obj.IsAvailable = Convert.ToBoolean(reader["IsAvailable"].ToString());
                                objList.Add(obj);
                            }
                        }
                    }
                    connection.Close();
                    return objList;
                }
                catch (Exception ex)
                {
                    return new List<PickupAvailabilityModel>();
                }
            }
        }
    }
}
