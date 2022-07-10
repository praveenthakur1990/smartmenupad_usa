using SmartMenu.BAL.Interfaces;
using SmartMenu.DAL.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace SmartMenu.BAL.Services
{
    public class BusinessHoursBusiness : IBusinessHoursBusiness
    {
        public int AddCustomLabel(string jsonStr, string connectionStr)
        {
            using (SqlConnection connection = new SqlConnection(connectionStr))
            {
                int response = 0;
                try
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand("Exec [dbo].[USPAddUpdateCustomUrl] @JsonStr, @Result", connection))
                    {
                        command.Parameters.AddWithValue("@JsonStr", jsonStr);
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

        public int AddUpdateBusinessHours(string businessHourksonStr, string connectionStr)
        {
            using (SqlConnection connection = new SqlConnection(connectionStr))
            {
                int response = 0;
                try
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand("Exec [dbo].[USPAddUpdateBusinessHours] @BusinessHourksonStr, @Result", connection))
                    {
                        command.Parameters.AddWithValue("@BusinessHourksonStr", businessHourksonStr);
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

        public int AddUpdatePickupAddress(string addresses, string connectionStr)
        {
            using (SqlConnection connection = new SqlConnection(connectionStr))
            {
                int response = 0;
                try
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand("Exec [dbo].[USPAddUpdatePickupAddress] @PickupAddress, @Result", connection))
                    {
                        command.Parameters.AddWithValue("@PickupAddress", addresses);
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

        public List<BusinessHoursModelVM> GetBusinessHours(string connectionStr)
        {
            using (SqlConnection connection = new SqlConnection(connectionStr))
            {
                List<BusinessHoursModelVM> objList = new List<BusinessHoursModelVM>();
                BusinessHoursModelVM obj = null;
                try
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand("Exec [dbo].[USPGetBusinessHours]", connection))
                    {
                        SqlDataReader reader = command.ExecuteReader();
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                obj = new BusinessHoursModelVM();
                                obj.Id = Convert.ToInt32(reader["Id"].ToString());
                                obj.WeekDayId = Convert.ToInt32(reader["WeekDayId"].ToString());
                                obj.OpenTime = reader["OpenTime"].ToString();
                                obj.CloseTime = reader["CloseTime"].ToString();
                                obj.OpenTime12Hour = reader["OpenTime12Hour"].ToString();
                                obj.CloseTime12Hour = reader["CloseTime12Hour"].ToString();
                                obj.IsClosed = Convert.ToBoolean(reader["IsClosed"].ToString());
                                objList.Add(obj);
                            }
                        }
                    }
                    connection.Close();
                    return objList;
                }
                catch (Exception ex)
                {
                    return new List<BusinessHoursModelVM>();
                }
            }
        }
    }
}
