using SmartMenu.BAL.Interfaces;
using SmartMenu.DAL.Common;
using SmartMenu.DAL.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;

namespace SmartMenu.BAL.Services
{
    public class SubscriptionBusiness : ISubscriptionBusiness
    {
        public int AddUpDateSubscription(SubscriptionModel model, string connectionStr)
        {
            using (SqlConnection connection = new SqlConnection(connectionStr))
            {
                int response = 0;
                try
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand("Exec [dbo].[USPAddUpdateSubscription] @Id, @UserId, @CustomerId, @SubscriptionId, @SubscriptionStartDate, @SubscriptionEndDate, @Amount, @IsCaptured, @seller_message, @brand, @funding, @last4, @receipt_url, @status, @latest_invoice, @livemode, @payment_method, @CancellationReason, @CanceledAt, @CreatedDate, @Result", connection))
                    {
                        command.Parameters.AddWithValue("@Id", model.Id);
                        command.Parameters.AddWithValue("@UserId", model.UserId ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@CustomerId", model.CustomerId ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@SubscriptionId", model.SubscriptionId ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@SubscriptionStartDate", model.SubscriptionStartDate ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@SubscriptionEndDate", model.SubscriptionEndDate ?? (object)DBNull.Value);

                        command.Parameters.AddWithValue("@Amount", model.Amount ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@IsCaptured", model.IsCaptured == null ? false : model.IsCaptured);
                        command.Parameters.AddWithValue("@seller_message", model.seller_message ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@brand", model.brand ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@funding", model.funding ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@last4", model.last4 ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@receipt_url", model.receipt_url ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@status", model.status ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@latest_invoice", model.latest_invoice ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@livemode", model.livemode == null ? false : model.livemode);
                        command.Parameters.AddWithValue("@payment_method", model.payment_method ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@CancellationReason", model.CancellationReason ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@CanceledAt", model.CanceledAt ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@CreatedDate", model.CreatedDate ?? (object)DBNull.Value);
                        command.Parameters.Add("@Result", SqlDbType.Int);
                        command.Parameters["@Result"].Direction = ParameterDirection.Output;
                        response = Convert.ToInt32(command.ExecuteScalar());
                    }
                    connection.Close();
                    return response;
                }
                catch (Exception ex)
                {
                    CommonManager.LogError(MethodBase.GetCurrentMethod(), ex, model, connectionStr);
                    return 0;
                }
            }
        }

        public SubscriptionInfoModel GetActiveSubscriptionInfo(string connectionStr)
        {
            using (SqlConnection connection = new SqlConnection(connectionStr))
            {
                SubscriptionInfoModel obj = null;
                try
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand("Exec [dbo].[GetUserActiveSubscriptionInfo]", connection))
                    {
                        SqlDataReader reader = command.ExecuteReader();
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                PlanModel objPlanModel = new PlanBusiness().GetPlans(Convert.ToInt32(reader["PlanId"].ToString())).FirstOrDefault();
                                obj = new SubscriptionInfoModel();
                                if (reader["SubscriptionStartDate"] is DBNull)
                                {
                                    obj.SubscriptionStartDate = null;
                                }
                                else
                                {
                                    obj.SubscriptionStartDate = Convert.ToDateTime(reader["SubscriptionStartDate"]);
                                }
                                if (reader["SubscriptionEndDate"] is DBNull)
                                {
                                    obj.SubscriptionEndDate = null;
                                }
                                else
                                {
                                    obj.SubscriptionEndDate = Convert.ToDateTime(reader["SubscriptionEndDate"]);
                                }
                                obj.PlanName = objPlanModel.Name;
                                obj.PlanCost = objPlanModel.Price.Value;
                                obj.SubscriptionId = reader["SubscriptionId"].ToString();
                            }
                        }
                    }
                    connection.Close();
                    return obj;
                }
                catch (Exception ex)
                {
                    return new SubscriptionInfoModel();
                }
            }
        }

        public string GetStripeSessionId(string userId, string connectionStr)
        {
            using (SqlConnection connection = new SqlConnection(connectionStr))
            {
                string response = string.Empty;
                try
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand("Exec [dbo].[USPGetStripeSessionId] @UserId, @SessionResult", connection))
                    {
                        command.Parameters.AddWithValue("@UserId", userId);
                        command.Parameters.Add("@SessionResult", SqlDbType.NVarChar, 500);
                        command.Parameters["@SessionResult"].Direction = ParameterDirection.Output;
                        response = command.ExecuteScalar().ToString();
                    }
                    connection.Close();
                    return response;
                }
                catch (Exception ex)
                {
                    return "-1";
                }
            }
        }

        public int UpdateSubscriptionCancel(bool IsCancelled, string connectionStr)
        {
            using (SqlConnection connection = new SqlConnection(connectionStr))
            {
                int response = 0;
                try
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand("Exec [dbo].[USPCancelSubscription] @IsCancel, @Result", connection))
                    {
                        command.Parameters.AddWithValue("@IsCancel", IsCancelled);
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
    }
}
