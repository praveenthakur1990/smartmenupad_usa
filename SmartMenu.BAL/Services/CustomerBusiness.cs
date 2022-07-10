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
    public class CustomerBusiness : ICustomerBusiness
    {      
        public int AddUpdateCustomerAddress(CustomerAddressesViewModel model, string connectionStr)
        {
            using (SqlConnection connection = new SqlConnection(connectionStr))
            {
                int response = 0;
                try
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand("Exec [dbo].[USPAddUpdateCustomerAddress] @CustomerAddressId, @CustomerId, @FirstName, @LastName, @EmailAddress, @PhoneNumber, @Address1, @Address2, @City, @State, @ZipCode, @Latitude, @Longitude, @CreatedBy, @Result", connection))
                    {
                        command.Parameters.AddWithValue("@CustomerAddressId", model.CustomerAddressId);
                        command.Parameters.AddWithValue("@CustomerId", model.CustomerId);
                        command.Parameters.AddWithValue("@FirstName", model.FirstName==null ? string.Empty:model.FirstName);
                        command.Parameters.AddWithValue("@LastName", model.LastName == null ? string.Empty : model.LastName);
                        command.Parameters.AddWithValue("@EmailAddress", model.EmailAddress == null ? string.Empty : model.EmailAddress);
                        command.Parameters.AddWithValue("@PhoneNumber", model.PhoneNumber == null ? string.Empty : model.PhoneNumber);
                        command.Parameters.AddWithValue("@Address1", model.Address1 == null ? string.Empty : model.Address1);
                        command.Parameters.AddWithValue("@Address2", model.Address2 == null ? string.Empty : model.Address2);
                        command.Parameters.AddWithValue("@City", model.City == null ? string.Empty : model.City);
                        command.Parameters.AddWithValue("@State", model.State == null ? string.Empty : model.State);
                        command.Parameters.AddWithValue("@ZipCode", model.ZipCode == null ? string.Empty : model.ZipCode);
                        command.Parameters.AddWithValue("@Latitude", model.Latitude == null ? string.Empty : model.Latitude);
                        command.Parameters.AddWithValue("@Longitude", model.Longitude == null ? string.Empty : model.Longitude);
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
        public List<CustomerViewModel> GetCustomerList(int customerId, string mobileNumber, PaginationModel pageModel, string connectionStr)
        {
            using (SqlConnection connection = new SqlConnection(connectionStr))
            {
                List<CustomerViewModel> objCategoryList = new List<CustomerViewModel>();
                CustomerViewModel obj = null;
                try
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand("Exec [dbo].[USPGetCustomerList_v2] @CustomerId, @MobileNumber, @PageNumber, @PageSize, @SearchStr", connection))
                    {
                        command.Parameters.AddWithValue("@CustomerId", customerId);
                        command.Parameters.AddWithValue("@MobileNumber", (mobileNumber == null ? string.Empty : mobileNumber));
                        command.Parameters.AddWithValue("@PageNumber", pageModel.PageNumber == 0 ? 1 : pageModel.PageNumber);
                        command.Parameters.AddWithValue("@PageSize", pageModel.PageSize);
                        command.Parameters.AddWithValue("@SearchStr", pageModel.SearchStr == null ? string.Empty : pageModel.SearchStr);
                        SqlDataReader reader = command.ExecuteReader();
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                obj = new CustomerViewModel();
                                obj.Id = Convert.ToInt32(reader["Id"].ToString());
                                obj.FirstName = reader["FirstName"].ToString();
                                obj.LastName = reader["LastName"].ToString();
                                obj.MobileNumber = reader["MobileNumber"].ToString();
                                //obj.IsActive = Convert.ToBoolean(reader["IsActive"].ToString());
                                obj.CustomerAddresses = (reader["AddressesJsonStr"] is DBNull ? new List<CustomerAddressesModel>() : JsonConvert.DeserializeObject<List<CustomerAddressesModel>>("[" + reader["AddressesJsonStr"].ToString() + "]"));
                                if (reader["CreatedDate"] is DBNull)
                                {
                                    obj.CreatedDate = null;
                                }
                                else
                                {
                                    obj.CreatedDate = Convert.ToDateTime(reader["CreatedDate"].ToString());
                                }
                                obj.TotalRows = Convert.ToInt32(reader["TotalRows"].ToString());
                                //if (reader["UpdatedDate"] is DBNull)
                                //{
                                //    obj.UpdatedDate = null;
                                //}
                                //else
                                //{
                                //    obj.UpdatedDate = Convert.ToDateTime(reader["UpdatedDate"].ToString());
                                //}
                                objCategoryList.Add(obj);
                            }
                        }
                    }
                    connection.Close();
                    return objCategoryList;
                }
                catch (Exception ex)
                {
                    return new List<CustomerViewModel>();
                }
            }
        }
    }
}
