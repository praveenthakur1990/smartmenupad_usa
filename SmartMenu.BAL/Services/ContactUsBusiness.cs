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
    public class ContactUsBusiness : IContactUsBusiness
    {
        public List<ContactUsVM> GetContactUsList(PaginationModel model, string connectionStr)
        {
            using (SqlConnection connection = new SqlConnection(connectionStr))
            {
                List<ContactUsVM> objList = new List<ContactUsVM>();
                ContactUsVM obj = null;
                try
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand("Exec [dbo].[USPGetContactUsList] @PageNumber, @PageSize, @SearchStr", connection))
                    {

                        command.Parameters.AddWithValue("@PageNumber", model.PageNumber == 0 ? 1 : model.PageNumber);
                        command.Parameters.AddWithValue("@PageSize", model.PageSize);
                        command.Parameters.AddWithValue("@SearchStr", model.SearchStr == null ? string.Empty : model.SearchStr);
                        SqlDataReader reader = command.ExecuteReader();
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                obj = new ContactUsVM();
                                obj.EmailAddress = reader["EmailAddress"].ToString();
                                obj.AddedDate = Convert.ToDateTime(reader["AddedDate"].ToString());
                                obj.TotalRows = Convert.ToInt32(reader["TotalRows"].ToString());
                                objList.Add(obj);
                            }
                        }
                    }
                    connection.Close();
                    return objList;
                }
                catch (Exception ex)
                {
                    return new List<ContactUsVM>();
                }
            }
        }

        public List<NewsLetterSubscriberModel> NewsLetterSubscriberList()
        {
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                List<NewsLetterSubscriberModel> objList = new List<NewsLetterSubscriberModel>();
                NewsLetterSubscriberModel obj = null;
                try
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand("Exec [dbo].[USPNewsletterSubscriberList]", connection))
                    {
                        SqlDataReader reader = command.ExecuteReader();
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                obj = new NewsLetterSubscriberModel();
                                obj.EmailAddress = reader["EmailAddress"].ToString();
                                obj.CreatedDate = Convert.ToDateTime(reader["CreatedDate"].ToString());
                                objList.Add(obj);
                            }
                        }
                    }
                    connection.Close();
                    return objList;
                }
                catch (Exception ex)
                {
                    return new List<NewsLetterSubscriberModel>();
                }
            }
        }

        public List<RequestDemoModel> RequestForDemoList()
        {
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                List<RequestDemoModel> objList = new List<RequestDemoModel>();
                RequestDemoModel obj = null;
                try
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand("Exec [dbo].[USPGetRequestForDemoList]", connection))
                    {
                        SqlDataReader reader = command.ExecuteReader();
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                obj = new RequestDemoModel();
                                obj.FullName = reader["FullName"].ToString();
                                obj.PhoneNumber = reader["PhoneNumber"].ToString();
                                obj.EmailAddress = reader["EmailAddress"].ToString();
                                obj.RestaurantName = reader["RestaurantName"].ToString();
                                obj.NoOfLocation = Convert.ToInt32(reader["NoOfLocation"].ToString());
                                obj.CreatedDate = Convert.ToDateTime(reader["CreatedDate"].ToString());
                                objList.Add(obj);
                            }
                        }
                    }
                    connection.Close();
                    return objList;
                }
                catch (Exception ex)
                {
                    return new List<RequestDemoModel>();
                }
            }
        }

        public int SaveContactUs(string emailAddress, string connectionStr)
        {
            using (SqlConnection connection = new SqlConnection(connectionStr))
            {
                int response = 0;
                try
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand("Exec [dbo].[USPSaveContactUs] @EmailAddress, @Result", connection))
                    {
                        command.Parameters.AddWithValue("@EmailAddress", emailAddress);
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

        public int SaveNewsLetterSubscriber(string emailAddress)
        {
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                int response = 0;
                try
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand("Exec [dbo].[USPAddNewsLetterSubscriber] @EmailAddress, @Result", connection))
                    {
                        command.Parameters.AddWithValue("@EmailAddress", emailAddress);
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

        public int SaveRequestForDemo(RequestDemoModel model)
        {
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                int response = 0;
                try
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand("Exec [dbo].[USPAddRequestForDemo] @FullName, @PhoneNumber, @EmailAddress, @RestaurantName, @NoOfLocation, @Result", connection))
                    {
                        command.Parameters.AddWithValue("@FullName", model.FullName);
                        command.Parameters.AddWithValue("@PhoneNumber", model.PhoneNumber);
                        command.Parameters.AddWithValue("@EmailAddress", model.EmailAddress);
                        command.Parameters.AddWithValue("@RestaurantName", model.RestaurantName);
                        command.Parameters.AddWithValue("@NoOfLocation", model.NoOfLocation);
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

        public int SaveTalkToExpertRequest(TalkToExpertModel model)
        {
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                int response = 0;
                try
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand("Exec [dbo].[USPAddTalkToExpertRequest] @FirstName, @LastName, @PhoneNumber, @EmailAddress, @ChoosePlan, @Result", connection))
                    {
                        command.Parameters.AddWithValue("@FirstName", model.FirstName);
                        command.Parameters.AddWithValue("@LastName", model.LastName);
                        command.Parameters.AddWithValue("@PhoneNumber", model.PhoneNumber);
                        command.Parameters.AddWithValue("@EmailAddress", model.EmailAddress);
                        command.Parameters.AddWithValue("@ChoosePlan", model.ChoosePlan);
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

        public List<TalkToExpertModel> TalkToExpertRequestList()
        {
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                List<TalkToExpertModel> objList = new List<TalkToExpertModel>();
                TalkToExpertModel obj = null;
                try
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand("Exec [dbo].[USPGetTalkForDemoRequestList]", connection))
                    {
                        SqlDataReader reader = command.ExecuteReader();
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                obj = new TalkToExpertModel();
                                obj.FirstName = reader["FirstName"].ToString();
                                obj.LastName = reader["LastName"].ToString();
                                obj.PhoneNumber = reader["PhoneNumber"].ToString();
                                obj.EmailAddress = reader["EmailAddress"].ToString();
                                obj.ChoosePlan = reader["ChoosePlan"].ToString();
                                obj.CreatedDate = Convert.ToDateTime(reader["CreatedDate"].ToString());
                                objList.Add(obj);
                            }
                        }
                    }
                    connection.Close();
                    return objList;
                }
                catch (Exception ex)
                {
                    return new List<TalkToExpertModel>();
                }
            }
        }
    }
}
