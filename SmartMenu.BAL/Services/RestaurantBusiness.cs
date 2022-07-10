using Newtonsoft.Json;
using SmartMenu.BAL.Interfaces;
using SmartMenu.DAL.Common;
using SmartMenu.DAL.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;

namespace SmartMenu.BAL.Services
{
    public class RestaurantBusiness : IRestaurantBusiness
    {
        public int AddUpdateDeliveryCharges(decimal tax, decimal deliveryCharges, bool IsCashOnDelivery, string connectionStr)
        {
            using (SqlConnection connection = new SqlConnection(connectionStr))
            {
                int response = 0;
                try
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand("Exec [dbo].[USPAddUpdateDeliveryCharges] @TaxRate, @DeliveryCharges, @CashOnDeliveryEnable, @Result", connection))
                    {
                        command.Parameters.AddWithValue("@TaxRate", tax);
                        command.Parameters.AddWithValue("@DeliveryCharges", deliveryCharges);
                        command.Parameters.AddWithValue("@CashOnDeliveryEnable", IsCashOnDelivery);
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

        public int AddUpdateRestaurant(RestaurantModel model, string connectionStr)
        {
            using (SqlConnection connection = new SqlConnection(connectionStr))
            {
                int response = 0;
                try
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand("Exec [dbo].[USPAddUpdateRestaurants] @Id, @Name, @Email, @Mobile, @Address, @Landmark, @State, @City, @ZipCode, @CountryCode, @Latitude, @Longitude, @ContactPersonName, @ContactNumber, @AverageCostTwoPerson, @TypeOfRestaurant, @RestaurantMealOption, @NumberOfTable, @Cuisnes, @LogoPath, @FSSAIRegistration, @FSSAIFilePath, @GSTRegistrationNumber, @GSTFilePath, @ActivePlan, @AnnualCost, @Commision, @PlanActiveDate, @CurrencySymbol, @TimeZone, @PlanId, @CreatedBy, @Result", connection))
                    {
                        command.Parameters.AddWithValue("@Id", model.Id);
                        command.Parameters.AddWithValue("@Name", model.Name);
                        command.Parameters.AddWithValue("@Email", model.Email);
                        command.Parameters.AddWithValue("@Mobile", model.Mobile);
                        command.Parameters.AddWithValue("@Address", model.Address);
                        command.Parameters.AddWithValue("@Landmark", model.Landmark == null ? string.Empty : model.Landmark);
                        command.Parameters.AddWithValue("@State", model.State);
                        command.Parameters.AddWithValue("@City", model.City);
                        command.Parameters.AddWithValue("@ZipCode", model.ZipCode);
                        command.Parameters.AddWithValue("@CountryCode", model.CountryCode);
                        command.Parameters.AddWithValue("@Latitude", model.Latitude);
                        command.Parameters.AddWithValue("@Longitude", model.Longitude);
                        command.Parameters.AddWithValue("@ContactPersonName", model.ContactPersonName);
                        command.Parameters.AddWithValue("@ContactNumber", model.ContactNumber);
                        command.Parameters.AddWithValue("@AverageCostTwoPerson", model.AverageCostTwoPerson);
                        command.Parameters.AddWithValue("@TypeOfRestaurant", model.TypeOfRestaurant);
                        command.Parameters.AddWithValue("@RestaurantMealOption", model.RestaurantMealOption);
                        command.Parameters.AddWithValue("@NumberOfTable", model.NumberOfTable);
                        command.Parameters.AddWithValue("@Cuisnes", model.Cuisnes);
                        command.Parameters.AddWithValue("@LogoPath", model.LogoPath);
                        command.Parameters.AddWithValue("@FSSAIRegistration", model.FSSAIRegistration);
                        command.Parameters.AddWithValue("@FSSAIFilePath", model.FSSAIFilePath);
                        command.Parameters.AddWithValue("@GSTRegistrationNumber", model.GSTRegistrationNumber);
                        command.Parameters.AddWithValue("@GSTFilePath", model.GSTFilePath);
                        command.Parameters.AddWithValue("@ActivePlan", model.ActivePlan);
                        command.Parameters.AddWithValue("@AnnualCost", model.AnnualCost);
                        command.Parameters.AddWithValue("@Commision", model.Commision);
                        command.Parameters.AddWithValue("@PlanActiveDate", model.PlanActiveDate);
                        command.Parameters.AddWithValue("@CurrencySymbol", model.CurrencySymbol);
                        command.Parameters.AddWithValue("@TimeZone", model.TimeZone);
                        command.Parameters.AddWithValue("@PlanId", model.PlanId);
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
                    CommonManager.LogError(MethodBase.GetCurrentMethod(), ex, model, connectionStr);
                    return 0;
                }
            }
        }
        public decimal CalculateDistanceBetweenRestaurantAndCustomer(decimal SourceLatitude, decimal SourceLongitude, decimal DestinationLatitude, decimal DestinationLongitude, string connectionStr)
        {
            using (SqlConnection connection = new SqlConnection(connectionStr))
            {
                decimal response = 0;
                try
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand("Exec [dbo].[USPGetCustomerAddressDistance] @SourceLatitude, @SourceLongitude, @DestinationLatitude, @DestinationLongitude, @Result", connection))
                    {
                        command.Parameters.AddWithValue("@SourceLatitude", SourceLatitude);
                        command.Parameters.AddWithValue("@SourceLongitude", SourceLongitude);
                        command.Parameters.AddWithValue("@DestinationLatitude", DestinationLatitude);
                        command.Parameters.AddWithValue("@DestinationLongitude", DestinationLongitude);
                        command.Parameters.Add("@Result", SqlDbType.Float);
                        command.Parameters["@Result"].Direction = ParameterDirection.Output;
                        response = Convert.ToInt32(command.ExecuteScalar());
                    }
                    connection.Close();
                    return response;
                }
                catch (Exception ex)
                {
                    return -1;
                }
            }
        }
        public RestaurantModelVM GetRestaurantDetail(string connectionStr)
        {
            using (SqlConnection connection = new SqlConnection(connectionStr))
            {
                RestaurantModelVM objRestaurant = null;
                try
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand("Exec [dbo].[USPGetRestaurant_v2]", connection))
                    {
                        SqlDataReader reader = command.ExecuteReader();
                        objRestaurant = new RestaurantModelVM();
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                objRestaurant.Id = Convert.ToInt32(reader["Id"].ToString());
                                objRestaurant.Name = reader["Name"].ToString();
                                objRestaurant.Email = reader["Email"].ToString();

                                objRestaurant.Mobile = reader["Mobile"].ToString();
                                objRestaurant.Address = reader["Address"].ToString();
                                if (reader["Landmark"] is DBNull)
                                {
                                    objRestaurant.Landmark = string.Empty;
                                }
                                else
                                {
                                    objRestaurant.Landmark = reader["Landmark"].ToString();
                                }
                                objRestaurant.State = reader["State"].ToString();
                                objRestaurant.City = reader["City"].ToString();
                                objRestaurant.ZipCode = reader["ZipCode"].ToString();
                                objRestaurant.CountryCode = reader["CountryCode"] == null ? string.Empty : reader["CountryCode"].ToString().Trim();
                                if (reader["Latitude"] is DBNull)
                                {
                                    objRestaurant.Latitude = string.Empty;
                                }
                                else
                                {
                                    objRestaurant.Latitude = reader["Latitude"].ToString();
                                }
                                if (reader["Longitude"] is DBNull)
                                {
                                    objRestaurant.Longitude = string.Empty;
                                }
                                else
                                {
                                    objRestaurant.Longitude = reader["Longitude"].ToString();
                                }
                                objRestaurant.ContactPersonName = reader["ContactPersonName"].ToString();
                                objRestaurant.ContactNumber = reader["ContactNumber"].ToString();

                                objRestaurant.AverageCostTwoPerson = Convert.ToDecimal(reader["AverageCostTwoPerson"].ToString());
                                objRestaurant.TypeOfRestaurant = reader["TypeOfRestaurant"].ToString();
                                objRestaurant.RestaurantMealOption = reader["RestaurantMealOption"].ToString();
                                objRestaurant.NumberOfTable = Convert.ToInt32(reader["NumberOfTable"].ToString());
                                objRestaurant.Cuisnes = reader["Cuisnes"].ToString();
                                if (reader["LogoPath"] is DBNull)
                                {
                                    objRestaurant.LogoPath = string.Empty;
                                }
                                else
                                {
                                    objRestaurant.LogoPath = ConfigurationManager.AppSettings["APPBaseUrl"] + reader["LogoPath"].ToString();
                                }

                                objRestaurant.FSSAIRegistration = reader["FSSAIRegistration"].ToString();
                                if (string.IsNullOrEmpty(reader["FSSAIFilePath"].ToString()))
                                {
                                    objRestaurant.FSSAIFilePath = string.Empty;
                                }
                                else
                                {
                                    objRestaurant.FSSAIFilePath = ConfigurationManager.AppSettings["APPBaseUrl"] + reader["FSSAIFilePath"].ToString();

                                }

                                // objRestaurant.FSSAIFilePath = reader["FSSAIFilePath"].ToString();
                                objRestaurant.GSTRegistrationNumber = reader["GSTRegistrationNumber"].ToString();
                                if (string.IsNullOrEmpty(reader["GSTFilePath"].ToString()))
                                {
                                    objRestaurant.GSTFilePath = string.Empty;
                                }
                                else
                                {
                                    objRestaurant.GSTFilePath = ConfigurationManager.AppSettings["APPBaseUrl"] + reader["GSTFilePath"].ToString();
                                }
                                // objRestaurant.GSTFilePath = reader["GSTFilePath"].ToString();
                                objRestaurant.ActivePlan = reader["ActivePlan"].ToString();
                                objRestaurant.AnnualCost = Convert.ToDecimal(reader["AnnualCost"].ToString());
                                objRestaurant.Commision = Convert.ToDecimal(reader["Commision"].ToString());
                                objRestaurant.PlanActiveDate = Convert.ToDateTime(reader["PlanActiveDate"].ToString());
                                objRestaurant.CurrencySymbol = reader["CurrencySymbol"] == null ? string.Empty : reader["CurrencySymbol"].ToString().Trim();
                                objRestaurant.TimeZone = reader["TimeZone"].ToString();
                                objRestaurant.MinOrderAmt = Convert.ToDecimal(reader["MinOrderAmt"].ToString());
                                objRestaurant.MaxDeliveryAreaInMiles = Convert.ToDecimal(reader["MaxDeliveryAreaInMiles"].ToString());
                                objRestaurant.IsOnline = Convert.ToBoolean(reader["IsOnline"].ToString());
                                objRestaurant.IsActive = Convert.ToBoolean(reader["IsActive"].ToString());
                                objRestaurant.CreatedDate = Convert.ToDateTime(reader["CreatedDate"].ToString());
                                objRestaurant.BusinessHours = new BusinessHoursModelVM();
                                objRestaurant.BusinessHours.WeekDayId = Convert.ToInt32(reader["WeekDayId"].ToString());
                                objRestaurant.BusinessHours.OpenTime = reader["OpenTime"].ToString();
                                objRestaurant.BusinessHours.CloseTime = reader["CloseTime"].ToString();
                                objRestaurant.BusinessHours.OpenTime12Hour = reader["OpenTime12Hour"].ToString();
                                objRestaurant.BusinessHours.CloseTime12Hour = reader["CloseTime12Hour"].ToString();
                                objRestaurant.BusinessHours.CurrentTime = reader["CurrentTime"].ToString();
                                objRestaurant.BusinessHours.IsClosed = Convert.ToBoolean(reader["IsClosed"].ToString());
                                if (reader["PickUpStartDateTime"] is DBNull)
                                {
                                    objRestaurant.PickUpStartDateTime = null;
                                }
                                else
                                {
                                    objRestaurant.PickUpStartDateTime = Convert.ToDateTime(reader["PickUpStartDateTime"].ToString());
                                }
                                if (reader["PickUpEndDateTime"] is DBNull)
                                {
                                    objRestaurant.PickUpEndDateTime = null;
                                }
                                else
                                {
                                    objRestaurant.PickUpEndDateTime = Convert.ToDateTime(reader["PickUpEndDateTime"].ToString());
                                }


                                objRestaurant.PickupAddresses = reader["PickupAddresses"] is DBNull ? string.Empty : reader["PickupAddresses"].ToString();
                                //objRestaurant.PickUpDateTimeStr = reader["PickUpDateTimeStr"] is DBNull ? string.Empty : reader["PickUpDateTimeStr"].ToString();

                                objRestaurant.PickUpDateJsonStr = reader["PickUpDateJsonStr"] is DBNull ? string.Empty : reader["PickUpDateJsonStr"].ToString();
                                objRestaurant.PickUpTimeJsonStr = reader["PickUpTimeJsonStr"] is DBNull ? string.Empty : reader["PickUpTimeJsonStr"].ToString();

                                objRestaurant.PickUpDateList = JsonConvert.DeserializeObject<List<PickUpDateVM>>("[" + reader["PickUpDateJsonStr"].ToString() + "]");

                                objRestaurant.PickUpTimeList = JsonConvert.DeserializeObject<List<PickUpTimeVM>>("[" + reader["PickUpTimeJsonStr"].ToString() + "]");

                                objRestaurant.BannerImg = reader["BannerImg"].ToString();
                                objRestaurant.PromotionalImg = reader["PromotionalImg"].ToString();
                                objRestaurant.EmptyCartImg = reader["EmptyCartImg"].ToString();
                                objRestaurant.OfflineImg = reader["OfflineImg"].ToString();
                                objRestaurant.MinOrderImg = reader["MinOrderImg"].ToString();
                                objRestaurant.LocationFarAwayImg = reader["LocationFarAwayImg"].ToString();
                                if (reader["CustomLabel"] is DBNull)
                                {
                                    objRestaurant.CustomLabel = null;
                                }
                                else
                                {
                                    objRestaurant.CustomLabel = JsonConvert.DeserializeObject<CustomUrlModel>(reader["CustomLabel"].ToString());
                                }
                                objRestaurant.TaxRate = Convert.ToDecimal(reader["TaxRate"].ToString());
                                objRestaurant.DeliveryCharges = Convert.ToDecimal(reader["DeliveryCharges"].ToString());
                                objRestaurant.CashOnDeliveryEnable = Convert.ToBoolean(reader["CashOnDeliveryEnable"].ToString());

                                objRestaurant.PlanId = reader["PlanId"] is DBNull ? 0 : Convert.ToInt32(reader["PlanId"].ToString());
                                objRestaurant.IsSubscriptionCancelled = reader["IsSubscriptionCancelled"] is DBNull ? false : Convert.ToBoolean(reader["IsSubscriptionCancelled"].ToString());
                                if (reader["SubscriptionCancelledOn"] is DBNull)
                                {
                                    objRestaurant.SubscriptionCancelledOn = null;
                                }
                                else
                                {
                                    objRestaurant.SubscriptionCancelledOn = Convert.ToDateTime(reader["SubscriptionCancelledOn"].ToString());
                                }
                                return objRestaurant;
                            }
                        }
                    }
                    connection.Close();
                    return objRestaurant;
                }
                catch (Exception ex)
                {
                    return new RestaurantModelVM();
                }
            }
        }
        public int SetOnlineOfflineRestaurant(bool IsOnline, string connectionStr)
        {
            using (SqlConnection connection = new SqlConnection(connectionStr))
            {
                int response = 0;
                try
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand("Exec [dbo].[USPSetOnlineOfflineRestaurant] @IsOnline", connection))
                    {
                        command.Parameters.AddWithValue("@IsOnline", IsOnline);
                        response = Convert.ToInt32(command.ExecuteScalar());
                        response = 1;
                    }
                    connection.Close();
                    return response;
                }
                catch (Exception ex)
                {
                    return -1;
                }
            }
        }
        public int UpdateDeliveryAreaSetting(decimal minOrderAmt, decimal maxDeliveryAreaInMiles, string connectionStr)
        {
            using (SqlConnection connection = new SqlConnection(connectionStr))
            {
                int response = 0;
                try
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand("Exec [dbo].[USPUpdateDeliveryAreaSetting] @MinOrderAmt, @MaxDeliveryAreaInMiles, @Result", connection))
                    {
                        command.Parameters.AddWithValue("@MinOrderAmt", minOrderAmt);
                        command.Parameters.AddWithValue("@MaxDeliveryAreaInMiles", maxDeliveryAreaInMiles);
                        command.Parameters.Add("@Result", SqlDbType.Int);
                        command.Parameters["@Result"].Direction = ParameterDirection.Output;
                        response = Convert.ToInt32(command.ExecuteScalar());
                    }
                    connection.Close();
                    return response;
                }
                catch (Exception ex)
                {
                    return -1;
                }
            }
        }
        public int UpdateImages(WebSiteImages model, string connectionStr)
        {
            using (SqlConnection connection = new SqlConnection(connectionStr))
            {
                int response = 0;
                try
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand("Exec [dbo].[USPAddUpdateWebsiteImages] @BannerImg, @PromotionalImg, @EmptyCartImg, @OfflineImg, @MinOrderImg, @LocationFarAwayImg, @Result", connection))
                    {
                        command.Parameters.AddWithValue("@BannerImg", !string.IsNullOrEmpty(model.BannerImg.ImagePath) ? model.BannerImg.ImagePath : string.Empty);
                        command.Parameters.AddWithValue("@PromotionalImg", !string.IsNullOrEmpty(model.PromotionalImg.ImagePath) ? model.PromotionalImg.ImagePath : string.Empty);
                        command.Parameters.AddWithValue("@EmptyCartImg", !string.IsNullOrEmpty(model.EmptyCartImg.ImagePath) ? model.EmptyCartImg.ImagePath : string.Empty);
                        command.Parameters.AddWithValue("@OfflineImg", !string.IsNullOrEmpty(model.OfflineImg.ImagePath) ? model.OfflineImg.ImagePath : string.Empty);
                        command.Parameters.AddWithValue("@MinOrderImg", !string.IsNullOrEmpty(model.MinOrderImg.ImagePath) ? model.MinOrderImg.ImagePath : string.Empty);
                        command.Parameters.AddWithValue("@LocationFarAwayImg", !string.IsNullOrEmpty(model.LocationFarAwayImg.ImagePath) ? model.LocationFarAwayImg.ImagePath : string.Empty);
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


