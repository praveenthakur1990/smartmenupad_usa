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
    public class CategoryBusiness : ICategoryBusiness
    {
        public int AddUpdateCategory(CategoryModel model, string connectionStr)
        {
            using (SqlConnection connection = new SqlConnection(connectionStr))
            {
                int response = 0;
                try
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand("Exec [dbo].[USPAddUpdateCategory] @Id, @Name, @Description, @ImagePath, @PriorityIndex, @CreatedBy, @Result", connection))
                    {
                        command.Parameters.AddWithValue("@Id", model.Id);
                        command.Parameters.AddWithValue("@Name", model.Name);
                        command.Parameters.AddWithValue("@Description", model.Description);
                        command.Parameters.AddWithValue("@ImagePath", model.ImagePath);
                        command.Parameters.AddWithValue("@PriorityIndex", model.PriorityIndex);
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

        public List<CategoryModel> GetCategories(string type, string connectionStr)
        {
            using (SqlConnection connection = new SqlConnection(connectionStr))
            {
                List<CategoryModel> objCategoryList = new List<CategoryModel>();
                CategoryModel obj = null;
                try
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand("Exec [dbo].[USPGetCategories] @Type", connection))
                    {
                        command.Parameters.AddWithValue("@Type", type == null ? string.Empty : type);
                        SqlDataReader reader = command.ExecuteReader();
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                obj = new CategoryModel();
                                obj.Id = Convert.ToInt32(reader["Id"].ToString());
                                obj.Name = reader["Name"].ToString();
                                obj.Description = reader["Description"].ToString();

                                if (string.IsNullOrEmpty(reader["PriorityIndex"].ToString()))
                                {
                                    obj.PriorityIndex = 0;
                                }
                                else
                                {
                                    obj.PriorityIndex = Convert.ToInt32(reader["PriorityIndex"].ToString());
                                }
                                if (string.IsNullOrEmpty(reader["ImagePath"].ToString()))
                                {
                                    obj.ImagePath = string.Empty;
                                }
                                else
                                {
                                    obj.ImagePath = ConfigurationManager.AppSettings["APPBaseUrl"].ToString() + reader["ImagePath"].ToString();
                                }
                                obj.IsActive = Convert.ToBoolean(reader["IsActive"].ToString());
                                obj.CreatedDate = Convert.ToDateTime(reader["CreatedDate"].ToString());
                                if (reader["UpdatedDate"] is DBNull)
                                {
                                    obj.UpdatedDate = null;
                                }
                                else
                                {
                                    obj.UpdatedDate = Convert.ToDateTime(reader["UpdatedDate"].ToString());
                                }
                                objCategoryList.Add(obj);
                            }
                        }
                    }
                    connection.Close();
                    return objCategoryList;
                }
                catch (Exception ex)
                {
                    return new List<CategoryModel>();
                }
            }
        }

        public int MarkCategoryAsDeleted(int categoryId, string connectionStr)
        {
            using (SqlConnection connection = new SqlConnection(connectionStr))
            {
                int response = 0;
                try
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand("Exec [dbo].[USPSetIsDeletedCategory] @Id, @Result", connection))
                    {
                        command.Parameters.AddWithValue("@Id", categoryId);
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
