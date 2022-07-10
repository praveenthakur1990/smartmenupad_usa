using SmartMenu.BAL.Services;
using SmartMenu.DAL.Common;
using SmartMenu.DAL.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SmartMenu.WEB.Controllers
{
    public abstract class MultitenantBaseController : Controller
    {
        protected int _pageSize = Convert.ToInt32(ConfigurationManager.AppSettings["PageSize"].ToString());
        protected TenantsVM tenantInfo { get; private set; }
        protected RestaurantModelVM restutantInfo { get; private set; }
        protected List<QuickPagesVM> quickLinks { get; private set; }
        protected List<SocialMediaModel> socialMediaLinks { get; private set; }
        protected override void OnActionExecuting(
            ActionExecutingContext filterContext)
        {
            string subDomainStr = (string)filterContext.RequestContext.RouteData.Values["subdomain"];
            if (string.IsNullOrEmpty(subDomainStr))
            {
                //filterContext.Result = new HttpUnauthorizedResult();
                //filterContext.Result = new RedirectResult("/Home/Index");
            }
            else
            {
                tenantInfo = CommonManager.getTenantConnection(string.Empty, string.Empty).ToList().Where(c => c.TenantDomain == subDomainStr).FirstOrDefault();
                if (new BusinessHoursBusiness().GetBusinessHours(tenantInfo.TenantConnection).Count() == 0)
                {
                    new BusinessHoursBusiness().GetBusinessHours(tenantInfo.TenantConnection);
                }
                restutantInfo = new RestaurantBusiness().GetRestaurantDetail(tenantInfo.TenantConnection);
                quickLinks = new QuickPagesBusiness().GetQuickPages(tenantInfo.TenantConnection).Where(c => c.IsActive == true).ToList();
                socialMediaLinks = new SocialMedialinksBusiness().GetSocialMediaLinks(tenantInfo.TenantConnection).Where(c => c.IsActive == true).ToList();
                ViewBag.IsClosed = restutantInfo.BusinessHours.IsClosed;
                ViewBag.RestaurantAddress = restutantInfo.Address;
                ViewBag.quickLinks = quickLinks;
                ViewBag.socialMediaLinks = socialMediaLinks;
                ViewBag.CurrencySymbol = restutantInfo.CurrencySymbol;
                ViewBag.CurrencyCode = restutantInfo.CountryCode == "IN" ? "INR" : restutantInfo.CountryCode == "ES" ? "EUR" : "USD";
                ViewBag.CountryMobileCode = restutantInfo.CountryCode == "IN" ? "+91" : restutantInfo.CountryCode == "ES" ? "+34" : "+1";
                ViewBag.PickUpStartDateTime = restutantInfo.PickUpStartDateTime?.ToString("yyyy-MM-dd HH:mm:ss");
                ViewBag.PickUpEndDateTime = restutantInfo.PickUpEndDateTime?.ToString("yyyy-MM-dd HH:mm:ss");

                ViewBag.PickUpDateArr = new List<PickUpDateVM>();
                if (restutantInfo.PickUpDateList.Count() > 0)
                {
                    foreach (var item in restutantInfo.PickUpDateList)
                    {
                        ViewBag.PickUpDateArr.Add(item);
                    }
                }

                ViewBag.PickUpTimeArr = new List<PickUpTimeVM>();
                if (restutantInfo.PickUpTimeList.Count() > 0)
                {
                    foreach (var item in restutantInfo.PickUpTimeList)
                    {
                        ViewBag.PickUpTimeArr.Add(item);
                    }
                }

                ViewBag.PickUpAddress = new List<string>();
                ViewBag.PickUpAddress.Add(restutantInfo.Address);


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
                ViewBag.IsShowCustomButton = restutantInfo.CustomLabel != null ? Convert.ToBoolean(restutantInfo.CustomLabel.IsActive) : false;
                ViewBag.DeliveryCharges = restutantInfo.DeliveryCharges;
                ViewBag.Tax = restutantInfo.TaxRate;
                ViewBag.CashOnDeliveryEnable = restutantInfo.CashOnDeliveryEnable;

                if (!string.IsNullOrEmpty(restutantInfo.PickupAddresses))
                {
                    var PickupAddresses = restutantInfo.PickupAddresses.Split('_').ToList();
                    foreach (var item in PickupAddresses)
                    {
                        ViewBag.PickUpAddress.Add(item);
                    }
                }

                if (restutantInfo.BusinessHours.IsClosed == true)
                {
                    Session.Abandon();
                    Session.Clear();
                }

                if (filterContext.HttpContext.Request.IsAjaxRequest())
                {
                    bool _isClosed = filterContext.RequestContext.RouteData.Values["action"].ToString() == "GetOrderInfo" ? false : true;
                    if (_isClosed)
                    {
                        if (restutantInfo.BusinessHours.IsClosed == true)
                        {
                            filterContext.Result = new JsonResult
                            {
                                Data = new
                                {
                                    IsClosed = _isClosed
                                },
                                JsonRequestBehavior = JsonRequestBehavior.AllowGet
                            };
                        }
                    }

                }
            }
            base.OnActionExecuting(filterContext);
        }
    }
}