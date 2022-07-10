using Newtonsoft.Json;
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
    public class MenuBusiness : IMenuBusiness
    {
        public int AddUpdateMenu(MenuItemViewModel model, string connectionStr)
        {
            using (SqlConnection connection = new SqlConnection(connectionStr))
            {
                int response = 0;
                try
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand("Exec [dbo].[USPAddUpdateMenu] @Id, @CategoryId, @Name, @Description, @ImagePath, @VideoPath, @IsMultipleSize, @MultipleSizesJsonStr, @IsAddOnsChoices, @AddOnsJsonStr, @Price, @RecommendedItems, @IsSeasonal, @SeasonalMonth, @IsOnSelectedDays, @SelectedDays, @ItemAs, @VegNonVeg, @IsPublish, @CreatedBy, @Result", connection))
                    {
                        command.Parameters.AddWithValue("@Id", model.Id);
                        command.Parameters.AddWithValue("@CategoryId", model.CategoryId);
                        command.Parameters.AddWithValue("@Name", model.Name);
                        command.Parameters.AddWithValue("@Description", model.Description == null ? DBNull.Value.ToString() : model.Description);

                        if (string.IsNullOrEmpty(model.ImagePath))
                        {
                            command.Parameters.AddWithValue("@ImagePath", DBNull.Value);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@ImagePath", model.ImagePath);
                        }

                        if (string.IsNullOrEmpty(model.VideoPath))
                        {
                            command.Parameters.AddWithValue("@VideoPath", DBNull.Value);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@VideoPath", model.VideoPath);
                        }

                        command.Parameters.AddWithValue("@IsMultipleSize", model.IsMultipleSize);
                        command.Parameters.AddWithValue("@MultipleSizesJsonStr", model.MultipleSizesJsonStr);
                        command.Parameters.AddWithValue("@IsAddOnsChoices", model.IsAddOnsChoices);
                        command.Parameters.AddWithValue("@AddOnsJsonStr", model.AddOnsJsonStr);

                        command.Parameters.AddWithValue("@Price", model.Price);

                        if (string.IsNullOrEmpty(model.RecommendedItems))
                        {
                            command.Parameters.AddWithValue("@RecommendedItems", DBNull.Value);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@RecommendedItems", model.RecommendedItems);
                        }

                        command.Parameters.AddWithValue("@IsSeasonal", model.IsSeasonal);
                        command.Parameters.AddWithValue("@SeasonalMonth", model.SeasonalMonth);
                        command.Parameters.AddWithValue("@IsOnSelectedDays", model.IsOnSelectedDays);
                        command.Parameters.AddWithValue("@SelectedDays", model.SelectedDays);

                        command.Parameters.AddWithValue("@ItemAs", model.ItemAs);
                        command.Parameters.AddWithValue("@VegNonVeg", model.VegNonVeg == null ? string.Empty : model.VegNonVeg);
                        command.Parameters.AddWithValue("@IsPublish", model.IsPublish);

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
        public List<MenuItemViewModel> GetAllMenu(int id, string connectionStr)
        {
            using (SqlConnection connection = new SqlConnection(connectionStr))
            {
                List<MenuItemViewModel> objMenuItemList = new List<MenuItemViewModel>();
                MenuItemViewModel obj = null;
                try
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand("Exec [dbo].[USPGetAllMenu_v1] @Id", connection))
                    {
                        command.Parameters.AddWithValue("@Id", id);
                        SqlDataReader reader = command.ExecuteReader();
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                obj = new MenuItemViewModel();
                                obj.Id = Convert.ToInt32(reader["Id"].ToString());
                                obj.CategoryId = Convert.ToInt32(reader["CategoryId"].ToString());
                                obj.Name = reader["Name"].ToString();
                                obj.CategoryName = reader["CategoryName"].ToString();

                                if (reader["ImagePath"] is DBNull)
                                {
                                    obj.ImagePath = "/Content/img/food-default-image.png";
                                }
                                else
                                {
                                    obj.ImagePath = ConfigurationManager.AppSettings["APPBaseUrl"].ToString() + reader["ImagePath"].ToString();
                                }

                                //if (reader["VideoPath"] is DBNull)
                                //{
                                //    obj.VideoPath = null;
                                //}
                                //else
                                //{
                                //    obj.VideoPath = reader["VideoPath"].ToString();
                                //}
                                obj.IsMultipleSize = Convert.ToBoolean(reader["IsMultipleSize"].ToString());
                                obj.Price = reader["Price"] is DBNull ? 0 : Convert.ToDecimal(reader["Price"].ToString());

                                obj.Description = reader["Description"] != DBNull.Value ? reader["Description"].ToString() : string.Empty;

                                if (reader["MultipleSizesJsonStr"] is DBNull || reader["MultipleSizesJsonStr"].ToString() == string.Empty)
                                {
                                    obj.MenuMultipleSizesList = new List<MenuMultipleSizesViewModel>();
                                }
                                else
                                {
                                    //var jsonStr = "[" + reader["MultipleSizesJsonStr"].ToString() + "]";
                                    obj.MenuMultipleSizesList = JsonConvert.DeserializeObject<List<MenuMultipleSizesViewModel>>(reader["MultipleSizesJsonStr"].ToString());
                                }

                                //obj.RecommendedItems = reader["RecommendedItems"].ToString();
                                //obj.IsDeleted = Convert.ToBoolean(reader["IsDeleted"].ToString());

                                obj.IsAddOnsChoices = Convert.ToBoolean(reader["IsAddOnsChoices"] is DBNull || reader["IsAddOnsChoices"].ToString() == string.Empty ? "false" : reader["IsAddOnsChoices"].ToString());

                                if (reader["AddOnsJsonStr"] is DBNull || reader["AddOnsJsonStr"].ToString() == string.Empty)
                                {
                                    obj.MenuAddOnsChoicesList = new List<MenuAddOnsChoicesViewModel>();
                                }
                                else
                                {
                                    obj.MenuAddOnsChoicesList = JsonConvert.DeserializeObject<List<MenuAddOnsChoicesViewModel>>(reader["AddOnsJsonStr"].ToString());
                                }

                                obj.IsOnSelectedDays = Convert.ToBoolean(reader["IsOnSelectedDays"].ToString() == string.Empty ? "false" : reader["IsOnSelectedDays"].ToString());
                                obj.SelectedDays = reader["SelectedDays"].ToString();

                                obj.IsSeasonal = Convert.ToBoolean(reader["IsSeasonal"].ToString() == string.Empty ? "false" : reader["IsSeasonal"].ToString());
                                obj.SeasonalMonth = reader["SeasonalMonth"].ToString();

                                obj.ItemAs = reader["ItemAs"].ToString();
                                obj.VegNonVeg = reader["VegNonVeg"].ToString();
                                obj.IsPublish = Convert.ToBoolean(reader["IsPublish"].ToString());
                                obj.CreatedDate = Convert.ToDateTime(reader["CreatedDate"].ToString());
                                obj.FinalPrice = Convert.ToDecimal(reader["FinalPrice"].ToString());
                                obj.CategoryImagePath = reader["CategoryImagePath"].ToString();
                                obj.IsShowOnCurrentMonth = Convert.ToBoolean(reader["IsShowOnCurrentMonth"].ToString());
                                obj.IsShowOnCurrentDay = Convert.ToBoolean(reader["IsShowOnCurrentDay"].ToString());
                                objMenuItemList.Add(obj);
                            }
                        }
                    }
                    connection.Close();
                    return objMenuItemList;
                }
                catch (Exception ex)
                {
                    return new List<MenuItemViewModel>();
                }
            }
        }

        public int MarkMenuAsDeleted(int categoryId, string connectionStr)
        {
            using (SqlConnection connection = new SqlConnection(connectionStr))
            {
                int response = 0;
                try
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand("Exec [dbo].[USPSetIsDeletedMenuItem] @Id, @Result", connection))
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
