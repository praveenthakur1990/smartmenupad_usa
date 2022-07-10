using SmartMenu.BAL.Interfaces;
using SmartMenu.DAL.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace SmartMenu.BAL.Services
{
    public class SocialMedialinksBusiness : ISocialMedialinksBusiness
    {
        public int AddUpdateSocialMediaLinks(string SocialMediaLinkJsonStr, string createdBy, string connectionStr)
        {
            using (SqlConnection connection = new SqlConnection(connectionStr))
            {
                int response = 0;
                try
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand("Exec [dbo].[USPAddUpdateSocialMediaLinks] @SocialMediaLinksJsonStr, @CreatedBy, @Result", connection))
                    {
                        command.Parameters.AddWithValue("@SocialMediaLinksJsonStr", SocialMediaLinkJsonStr);
                        command.Parameters.AddWithValue("@CreatedBy", createdBy);
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

        public List<SocialMediaModel> GetSocialMediaLinks(string connectionStr)
        {
            using (SqlConnection connection = new SqlConnection(connectionStr))
            {
                List<SocialMediaModel> objList = new List<SocialMediaModel>();
                SocialMediaModel obj = null;
                try
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand("Exec [dbo].[USPGetSocialMediaLinks]", connection))
                    {
                        SqlDataReader reader = command.ExecuteReader();
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                obj = new SocialMediaModel();
                                obj.Id = Convert.ToInt32(reader["Id"].ToString());
                                obj.Name = reader["Name"].ToString();
                                obj.Icon = reader["Icon"].ToString();
                                obj.Link = reader["Link"].ToString();
                                obj.IsActive = Convert.ToBoolean(reader["IsActive"].ToString());
                                objList.Add(obj);
                            }
                        }
                    }
                    connection.Close();
                    return objList;
                }
                catch (Exception ex)
                {
                    return new List<SocialMediaModel>();
                }
            }
        }
    }
}
