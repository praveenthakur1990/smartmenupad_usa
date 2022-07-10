using SmartMenu.BAL.Services;
using SmartMenu.DAL.Common;
using SmartMenu.DAL.Models;
using SmartMenu.WEB.Helpers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CommonManager = SmartMenu.WEB.Helpers.CommonManager;

namespace SmartMenu.WEB.Areas.admin.Controllers
{
    public abstract class BaseController : Controller
    {
        protected bool isOnline { get; private set; }
        protected RestaurantModelVM restutantInfo { get; private set; }
        protected TenantsVM tenantsInfo { get; private set; }
        protected SubscriptionInfoModel subscriptionInfo { get; private set; }
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            tenantsInfo = DAL.Common.CommonManager.getTenantConnection(SessionManager.LoginResponse.UserId, string.Empty).ToList().FirstOrDefault();
            if (new BusinessHoursBusiness().GetBusinessHours(tenantsInfo.TenantConnection).Count() == 0)
            {
                new BusinessHoursBusiness().GetBusinessHours(tenantsInfo.TenantConnection);
            }
            restutantInfo = new RestaurantBusiness().GetRestaurantDetail(tenantsInfo.TenantConnection);
            subscriptionInfo = new SubscriptionBusiness().GetActiveSubscriptionInfo(tenantsInfo.TenantConnection);
            if (subscriptionInfo == null)
            {
                if (filterContext.RouteData.Values["controller"].ToString() == "Setting" && filterContext.RouteData.Values["action"].ToString() == "Subscription")
                {

                }
                else
                {
                    filterContext.Result = new RedirectResult("/Admin/Setting/Subscription");
                }
            }
            ViewBag.IsOnline = restutantInfo.IsOnline;
            ViewBag.RestutantLogoImg = restutantInfo.LogoPath;
            ViewBag.CurrencySymbol = restutantInfo.CurrencySymbol;
            ViewBag.CountryMobileCode = restutantInfo.CountryCode == "IN" ? "+91" : restutantInfo.CountryCode == "ES" ? "+34" : "+1";
            if (!string.IsNullOrEmpty(restutantInfo.PickupAddresses))
            {
                ViewBag.PickUpAddress = new List<string>();
                var listOfSpeakers = restutantInfo.PickupAddresses.Split('_').ToList();
                foreach (var speaker in listOfSpeakers)
                {
                    ViewBag.PickUpAddress.Add(speaker);
                }
            }

            ViewBag.BannerImg = restutantInfo.BannerImg == null || restutantInfo.BannerImg == string.Empty ? ConfigurationManager.AppSettings["APPBaseUrl"].ToString() + "/Content/menu/img/restaurant-bg.png" : ConfigurationManager.AppSettings["APPBaseUrl"].ToString() + restutantInfo.BannerImg;

            ViewBag.IsBannerImg = CommonManager.IsPhoto(restutantInfo.BannerImg == null || restutantInfo.BannerImg == string.Empty ? ConfigurationManager.AppSettings["APPBaseUrl"].ToString() + "/Content/menu/img/restaurant-bg.png" : ConfigurationManager.AppSettings["APPBaseUrl"].ToString() + restutantInfo.BannerImg);

            ViewBag.PromotionalImg = restutantInfo.PromotionalImg == null || restutantInfo.PromotionalImg == string.Empty ? ConfigurationManager.AppSettings["APPBaseUrl"].ToString() + "/Content/menu/img/Offer-1.png" : ConfigurationManager.AppSettings["APPBaseUrl"].ToString() + restutantInfo.PromotionalImg;

            ViewBag.IsPromotionalImg = CommonManager.IsPhoto(restutantInfo.PromotionalImg == null || restutantInfo.PromotionalImg == string.Empty ? ConfigurationManager.AppSettings["APPBaseUrl"].ToString() + "/Content/menu/img/Offer-1.png" : ConfigurationManager.AppSettings["APPBaseUrl"].ToString() + restutantInfo.PromotionalImg);

            ViewBag.EmptyCartImg = restutantInfo.EmptyCartImg == null || restutantInfo.EmptyCartImg == string.Empty ? ConfigurationManager.AppSettings["APPBaseUrl"].ToString() + "/Content/menu/img/cart-image.png" : ConfigurationManager.AppSettings["APPBaseUrl"].ToString() + restutantInfo.EmptyCartImg;
            ViewBag.OfflineImg = restutantInfo.OfflineImg == null || restutantInfo.OfflineImg == string.Empty ? ConfigurationManager.AppSettings["APPBaseUrl"].ToString() + "/Content/img/sorry-closed.png" : ConfigurationManager.AppSettings["APPBaseUrl"].ToString() + restutantInfo.OfflineImg;
            ViewBag.MinOrderImg = restutantInfo.MinOrderImg == null || restutantInfo.MinOrderImg == string.Empty ? ConfigurationManager.AppSettings["APPBaseUrl"].ToString() + "/Content/img/addMoreItem.png" : ConfigurationManager.AppSettings["APPBaseUrl"].ToString() + restutantInfo.MinOrderImg;
            ViewBag.LocationFarAwayImg = restutantInfo.LocationFarAwayImg == null || restutantInfo.LocationFarAwayImg == string.Empty ? ConfigurationManager.AppSettings["APPBaseUrl"].ToString() + "/Content/img/outOfDeliveryArea.png" : ConfigurationManager.AppSettings["APPBaseUrl"].ToString() + restutantInfo.LocationFarAwayImg;

            ViewBag.LabelText = restutantInfo.CustomLabel != null ? restutantInfo.CustomLabel.LabelText : string.Empty;
            ViewBag.UrlText = restutantInfo.CustomLabel != null ? restutantInfo.CustomLabel.UrlText : string.Empty;
            ViewBag.IsActive = restutantInfo.CustomLabel != null ? Convert.ToBoolean(restutantInfo.CustomLabel.IsActive) : false;
            ViewBag.DeliveryCharges = restutantInfo.DeliveryCharges;
            ViewBag.Tax = restutantInfo.TaxRate;
            ViewBag.CashOnDeliveryEnable = restutantInfo.CashOnDeliveryEnable;
            base.OnActionExecuting(filterContext);
        }
    }
}