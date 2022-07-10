using SmartMenu.BAL.Interfaces;
using SmartMenu.DAL.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace SmartMenu.BAL.Services
{
    public class APIKeyBusiness : IAPIKeyBusiness
    {
        public int AddUpdateAPIKey(APIKeyModel model, string connectionStr)
        {
            using (SqlConnection connection = new SqlConnection(connectionStr))
            {
                int response = 0;
                try
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand("Exec [dbo].[USPAddUpdateAPIKey] @Id, @Publishablekey, @Secretkey, @CreatedBy, @Result", connection))
                    {
                        command.Parameters.AddWithValue("@Id", model.Id);
                        command.Parameters.AddWithValue("@Publishablekey", model.Publishablekey);
                        command.Parameters.AddWithValue("@Secretkey", model.Secretkey);
                        command.Parameters.AddWithValue("@CreatedBy", model.CreatedBy);
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

        public APIKeyModel GetAPIKey(string connectionStr)
        {
            using (SqlConnection connection = new SqlConnection(connectionStr))
            {
                APIKeyModel obj = new APIKeyModel();
                try
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand("Exec [dbo].[USPGetAPIKey]", connection))
                    {
                        SqlDataReader reader = command.ExecuteReader();
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                obj.Id = Convert.ToInt32(reader["Id"].ToString());
                                obj.Publishablekey = reader["Publishablekey"].ToString();
                                obj.Secretkey = reader["Secretkey"].ToString();
                                obj.CreatedDate = Convert.ToDateTime(reader["CreatedDate"].ToString());
                                if (reader["UpdatedDate"] is DBNull)
                                {
                                    obj.UpdatedDate = null;
                                }
                                else
                                {
                                    obj.UpdatedDate = Convert.ToDateTime(reader["UpdatedDate"].ToString());
                                }                                
                            }
                        }
                    }
                    connection.Close();
                    return obj;
                }
                catch (Exception ex)
                {
                    return new APIKeyModel();
                }
            }
        }
    }
}
