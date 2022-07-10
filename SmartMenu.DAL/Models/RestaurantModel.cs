using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SmartMenu.DAL.Models
{
    public class RestaurantModel
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string Address { get; set; }
        public string Landmark { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string ZipCode { get; set; }
        public string CountryCode { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string ContactPersonName { get; set; }
        public string ContactNumber { get; set; }
        public decimal AverageCostTwoPerson { get; set; }
        public string TypeOfRestaurant { get; set; }
        public string RestaurantMealOption { get; set; }
        public int NumberOfTable { get; set; }
        public string Cuisnes { get; set; }
        public string LogoPath { get; set; }
        public string FSSAIRegistration { get; set; }
        public string FSSAIFilePath { get; set; }
        public string GSTRegistrationNumber { get; set; }
        public string GSTFilePath { get; set; }
        public string ActivePlan { get; set; }
        public decimal AnnualCost { get; set; }
        public decimal Commision { get; set; }
        public DateTime? PlanActiveDate { get; set; }
        public string CurrencySymbol { get; set; }
        public string TimeZone { get; set; }
        public bool IsOnline { get; set; }
        public bool IsActive { get; set; }
        public decimal MinOrderAmt { get; set; }
        public decimal MaxDeliveryAreaInMiles { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DeletedDate { get; set; }
        public string CreatedBy { get; set; }
        public string PickupAddresses { get; set; }
        public string BannerImg { get; set; }
        public string PromotionalImg { get; set; }
        public string EmptyCartImg { get; set; }
        public string OfflineImg { get; set; }
        public string MinOrderImg { get; set; }
        public string LocationFarAwayImg { get; set; }
        public string CustomLabel { get; set; }

        public decimal TaxRate { get; set; }
        public decimal DeliveryCharges { get; set; }
        public bool CashOnDeliveryEnable { get; set; }       
        public int PlanId { get; set; }
        public bool IsSubscriptionCancelled { get; set; }
        public DateTime? SubscriptionCancelledOn { get; set; }
    }


    public class RestaurantModelVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string Address { get; set; }
        public string Landmark { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string ZipCode { get; set; }
        public string CountryCode { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string ContactPersonName { get; set; }
        public string ContactNumber { get; set; }
        public decimal AverageCostTwoPerson { get; set; }
        public string TypeOfRestaurant { get; set; }
        public string RestaurantMealOption { get; set; }
        public int NumberOfTable { get; set; }
        public string Cuisnes { get; set; }
        public string LogoPath { get; set; }
        public string FSSAIRegistration { get; set; }
        public string FSSAIFilePath { get; set; }
        public string GSTRegistrationNumber { get; set; }
        public string GSTFilePath { get; set; }
        public string ActivePlan { get; set; }
        public decimal AnnualCost { get; set; }
        public decimal Commision { get; set; }
        public DateTime? PlanActiveDate { get; set; }
        public string CurrencySymbol { get; set; }
        public string TimeZone { get; set; }
        public bool IsOnline { get; set; }
        public bool IsActive { get; set; }
        public decimal MinOrderAmt { get; set; }
        public decimal MaxDeliveryAreaInMiles { get; set; }
        public BusinessHoursModelVM BusinessHours { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DeletedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? PickUpStartDateTime { get; set; }
        public DateTime? PickUpEndDateTime { get; set; }
        public string PickupAddresses { get; set; }
        public string PickUpDateTimeStr { get; set; }

        public string PickUpDateJsonStr { get; set; }
        public string PickUpTimeJsonStr { get; set; }

        public List<PickUpDateVM> PickUpDateList { get; set; }

        public List<PickUpTimeVM> PickUpTimeList { get; set; }
        public string BannerImg { get; set; }
        public string PromotionalImg { get; set; }
        public string EmptyCartImg { get; set; }
        public string OfflineImg { get; set; }
        public string MinOrderImg { get; set; }
        public string LocationFarAwayImg { get; set; }
        public CustomUrlModel CustomLabel { get; set; }
        public decimal TaxRate { get; set; }
        public decimal DeliveryCharges { get; set; }
        public bool CashOnDeliveryEnable { get; set; }
        public int PlanId { get; set; }
        public bool IsSubscriptionCancelled { get; set; }
        public DateTime? SubscriptionCancelledOn { get; set; }
    }

    public class WebSiteImages
    {
        public Images BannerImg { get; set; }
        public Images PromotionalImg { get; set; }
        public Images EmptyCartImg { get; set; }
        public Images OfflineImg { get; set; }
        public Images MinOrderImg { get; set; }
        public Images LocationFarAwayImg { get; set; }
        public string UserId { get; set; }
    }

    public class Images
    {
        public string Type { get; set; }
        public string ImagePath { get; set; }
        public string Ext { get; set; }
    }
}
