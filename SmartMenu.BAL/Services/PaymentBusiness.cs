using SmartMenu.BAL.Interfaces;
using SmartMenu.DAL.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace SmartMenu.BAL.Services
{
    public class PaymentBusiness : IPaymentBusiness
    {
        public int AddPaymentInfo(PaymentModel model, string connectionStr)
        {
            using (SqlConnection connection = new SqlConnection(connectionStr))
            {
                int response = 0;
                try
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand("Exec [dbo].[USPAddPaymentInfo] @OrderId, @CapturedId, @CapturedAmt, @Currency, @Email_name, @NetWorkStatus, @SellerMessage, @Paid, @PaymentMethod, @Card_brand, @Funding, @Country, @Network, @Last4, @Exp_Month, @Exp_Year, @Status, @Receipt_url, @FailureCode, @FailureMessage, @TransactionDate, @Result", connection))
                    {
                        command.Parameters.AddWithValue("@OrderId", model.OrderId==null ? 0 :model.OrderId);

                        if (model.CapturedId == null)
                        {
                            var col = command.Parameters.Add("@CapturedId", SqlDbType.NVarChar, 100);
                            col.Value = DBNull.Value;
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@CapturedId", model.CapturedId);
                        }

                        if (model.CapturedAmt == null)
                        {
                            var col = command.Parameters.Add("@CapturedAmt", SqlDbType.Decimal);
                            col.Value = DBNull.Value;
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@CapturedAmt", model.CapturedAmt / 100);
                        }

                        if (model.Currency == null)
                        {
                            var col = command.Parameters.Add("@Currency", SqlDbType.Char, 3);
                            col.Value = DBNull.Value;
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@Currency", model.Currency);
                        }

                        if (model.Email_name == null)
                        {
                            var col = command.Parameters.Add("@Email_name", SqlDbType.NVarChar, 100);
                            col.Value = DBNull.Value;
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@Email_name", model.Email_name);
                        }

                        if (model.Funding == null)
                        {
                            var col = command.Parameters.Add("@Funding", SqlDbType.NVarChar, 50);
                            col.Value = DBNull.Value;
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@Funding", model.Funding);
                        }

                        if (model.NetWorkStatus == null)
                        {
                            var col = command.Parameters.Add("@NetWorkStatus", SqlDbType.NVarChar, 100);
                            col.Value = DBNull.Value;
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@NetWorkStatus", model.NetWorkStatus);
                        }

                        if (model.SellerMessage == null)
                        {
                            var col = command.Parameters.Add("@SellerMessage", SqlDbType.NVarChar, 100);
                            col.Value = DBNull.Value;
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@SellerMessage", model.SellerMessage);
                        }
                        command.Parameters.AddWithValue("@Paid", model.Paid == null ? false : model.Paid);
                        if (model.PaymentMethod == null)
                        {
                            var col = command.Parameters.Add("@PaymentMethod", SqlDbType.NVarChar, 100);
                            col.Value = DBNull.Value;
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@PaymentMethod", model.PaymentMethod);
                        }

                        if (model.Card_brand == null)
                        {
                            var col = command.Parameters.Add("@Card_brand", SqlDbType.NVarChar, 50);
                            col.Value = DBNull.Value;
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@Card_brand", model.Card_brand);
                        }

                        if (model.Country == null)
                        {
                            var col = command.Parameters.Add("@Country", SqlDbType.NVarChar, 50);
                            col.Value = DBNull.Value;
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@Country", model.Country);
                        }

                        if (model.Network == null)
                        {
                            var col = command.Parameters.Add("@Network", SqlDbType.NVarChar, 50);
                            col.Value = DBNull.Value;
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@Network", model.Network);
                        }

                        if (model.Last4 == null)
                        {
                            var col = command.Parameters.Add("@Last4", SqlDbType.NVarChar, 50);
                            col.Value = DBNull.Value;
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@Last4", model.Last4);
                        }

                        if (model.Exp_Month == null)
                        {
                            var col = command.Parameters.Add("@Exp_Month", SqlDbType.Int);
                            col.Value = DBNull.Value;
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@Exp_Month", model.Exp_Month);
                        }

                        if (model.Exp_Year == null)
                        {
                            var col = command.Parameters.Add("@Exp_Year", SqlDbType.Int);
                            col.Value = DBNull.Value;
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@Exp_Year", model.Exp_Year);
                        }

                        if (model.Status == null)
                        {
                            var col = command.Parameters.Add("@Status", SqlDbType.NVarChar, 50);
                            col.Value = DBNull.Value;
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@Status", model.Status);
                        }

                        if (model.Receipt_url == null)
                        {
                            var col = command.Parameters.Add("@Receipt_url", SqlDbType.NVarChar, 500);
                            col.Value = DBNull.Value;
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@Receipt_url", model.Receipt_url);
                        }

                        if (model.FailureCode == null)
                        {
                            var col = command.Parameters.Add("@FailureCode", SqlDbType.NVarChar, 50);
                            col.Value = DBNull.Value;
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@FailureCode", model.FailureCode);
                        }

                        if (model.FailureMessage == null)
                        {
                            var col = command.Parameters.Add("@FailureMessage", SqlDbType.NVarChar, 500);
                            col.Value = DBNull.Value;
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@FailureMessage", model.FailureMessage);
                        }
                        if (model.TransactionDate == null)
                        {
                            var col = command.Parameters.Add("@TransactionDate", SqlDbType.DateTime);
                            col.Value = DBNull.Value;
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@TransactionDate", model.TransactionDate);
                        }
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
