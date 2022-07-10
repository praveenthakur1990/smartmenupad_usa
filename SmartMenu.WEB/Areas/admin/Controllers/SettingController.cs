using Newtonsoft.Json;
using SmartMenu.BAL.Interfaces;
using SmartMenu.DAL.Models;
using SmartMenu.WEB.Helpers;
using Stripe;
using Stripe.Checkout;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using static SmartMenu.DAL.Enums.EnumHelper;

namespace SmartMenu.WEB.Areas.admin.Controllers
{
    [Compress]
    [AdminAuthorize]
    public class SettingController : BaseController
    {
        #region "private members"
        readonly string apiBaseUrl = ConfigurationManager.AppSettings["ApiBaseUrl"].ToString();
        private IAPIKeyBusiness _apiKeyBusiness;
        private ISubscriptionBusiness _subscriptionBusiness;
        #endregion
        public SettingController(IAPIKeyBusiness apiKeyBusiness, ISubscriptionBusiness subscriptionBusiness)
        {
            _apiKeyBusiness = apiKeyBusiness;
            _subscriptionBusiness = subscriptionBusiness;
        }

        #region "business hours"
        public ActionResult AddUpdateBusinessHours()
        {
            using (var client = new HttpClient())
            {
                List<BusinessHoursModelVM> objList = new List<BusinessHoursModelVM>();
                client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", "Bearer " + Helpers.SessionManager.LoginResponse.AccessToken);
                string url = apiBaseUrl + MethodEnum.GetBusinessHours.GetDescription().ToString() + "?userId=" + SessionManager.LoginResponse.UserId;
                HttpResponseMessage messge = client.GetAsync(url).Result;
                string result = messge.Content.ReadAsStringAsync().Result;
                if (messge.IsSuccessStatusCode)
                {
                    objList = JsonConvert.DeserializeObject<List<BusinessHoursModelVM>>(result);
                }
                return View(objList);
            }
        }

        [HttpPost]
        public int AddUpdateBusinessHours(List<BusinessHoursModelVM> weekDays)
        {
            var jsonStr = JsonConvert.SerializeObject(weekDays);
            using (var client = new System.Net.Http.HttpClient())
            {
                client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", "Bearer " + Helpers.SessionManager.LoginResponse.AccessToken);
                string url = apiBaseUrl + MethodEnum.AddUpdateBusinessHour.GetDescription().ToString() + "?businessHourJsonStr=" + jsonStr + "&userId=" + SessionManager.LoginResponse.UserId;
                System.Net.Http.HttpResponseMessage messge = client.PostAsync(url, null).Result;
                string result = messge.Content.ReadAsStringAsync().Result;
                if (messge.IsSuccessStatusCode)
                {
                    return 1;
                }
                else
                {
                    return -1;
                }
            }
        }

        #endregion

        #region "quick pages"
        public ActionResult AddUpdateQuickPages()
        {
            using (var client = new HttpClient())
            {
                List<QuickPagesVM> objList = new List<QuickPagesVM>();
                client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", "Bearer " + Helpers.SessionManager.LoginResponse.AccessToken);
                string url = apiBaseUrl + MethodEnum.GetQuickPages.GetDescription().ToString() + "?userId=" + SessionManager.LoginResponse.UserId;
                HttpResponseMessage messge = client.GetAsync(url).Result;
                string result = messge.Content.ReadAsStringAsync().Result;
                ViewBag.UserId = SessionManager.LoginResponse.UserId;
                if (messge.IsSuccessStatusCode)
                {
                    objList = JsonConvert.DeserializeObject<List<QuickPagesVM>>(result);
                }
                return View(objList);
            }
        }

        [HttpPost]
        public int AddUpdateQuickPages(List<QuickPagesVM> model)
        {
            using (var client = new System.Net.Http.HttpClient())
            {
                client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", "Bearer " + Helpers.SessionManager.LoginResponse.AccessToken);
                var httpContent = CommonManager.CreateHttpContent(model);
                string url = apiBaseUrl + MethodEnum.AddUpdateQuickPages_v2.GetDescription().ToString();
                System.Net.Http.HttpResponseMessage messge = client.PostAsync(url, httpContent).Result;
                string result = messge.Content.ReadAsStringAsync().Result;
                if (messge.IsSuccessStatusCode)
                {
                    return 1;
                }
                else
                {
                    return -1;
                }
            }
        }
        #endregion

        #region "social links"
        public ActionResult AddUpdateSocialMediaLinks()
        {
            using (var client = new HttpClient())
            {
                List<SocialMediaModel> objList = new List<SocialMediaModel>();
                client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", "Bearer " + Helpers.SessionManager.LoginResponse.AccessToken);
                string url = apiBaseUrl + MethodEnum.GetSocialMedia.GetDescription().ToString() + "?userId=" + SessionManager.LoginResponse.UserId;
                HttpResponseMessage messge = client.GetAsync(url).Result;
                string result = messge.Content.ReadAsStringAsync().Result;
                if (messge.IsSuccessStatusCode)
                {
                    objList = JsonConvert.DeserializeObject<List<SocialMediaModel>>(result);
                }
                return View(objList);
            }
        }

        [HttpPost]
        public int AddUpdateSocialMedia(List<SocialMediaModel> model)
        {
            var jsonStr = JsonConvert.SerializeObject(model);
            using (var client = new System.Net.Http.HttpClient())
            {
                client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", "Bearer " + Helpers.SessionManager.LoginResponse.AccessToken);
                string url = apiBaseUrl + MethodEnum.AddUpdateSocialMedia.GetDescription().ToString() + "?socialMediaLinkJsonStr=" + jsonStr + "&userId=" + SessionManager.LoginResponse.UserId;
                System.Net.Http.HttpResponseMessage messge = client.PostAsync(url, null).Result;
                string result = messge.Content.ReadAsStringAsync().Result;
                if (messge.IsSuccessStatusCode)
                {
                    return 1;
                }
                else
                {
                    return -1;
                }
            }
        }
        #endregion

        #region "pickup address"
        [HttpPost]
        public int AddUpdatePickUpAddress(string pickupAddresses)
        {
            using (var client = new System.Net.Http.HttpClient())
            {
                client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", "Bearer " + Helpers.SessionManager.LoginResponse.AccessToken);
                string url = apiBaseUrl + MethodEnum.AddUpdatePickUpAddress.GetDescription().ToString() + "?pickUpAddress=" + pickupAddresses + "&userId=" + SessionManager.LoginResponse.UserId;
                System.Net.Http.HttpResponseMessage messge = client.PostAsync(url, null).Result;
                string result = messge.Content.ReadAsStringAsync().Result;
                if (messge.IsSuccessStatusCode)
                {
                    return 1;
                }
                else
                {
                    return -1;
                }
            }
        }
        #endregion

        #region "custom URL"
        [HttpPost]
        public int AddCustomUrl(CustomUrlModel model)
        {
            using (var client = new System.Net.Http.HttpClient())
            {
                client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", "Bearer " + Helpers.SessionManager.LoginResponse.AccessToken);
                string url = apiBaseUrl + MethodEnum.AddCustomLabel.GetDescription().ToString() + "?customLabelJsonStr=" + new JavaScriptSerializer().Serialize(model) + "&userId=" + SessionManager.LoginResponse.UserId;
                System.Net.Http.HttpResponseMessage messge = client.PostAsync(url, null).Result;
                string result = messge.Content.ReadAsStringAsync().Result;
                if (messge.IsSuccessStatusCode)
                {
                    return 1;
                }
                else
                {
                    return -1;
                }
            }
        }
        #endregion

        #region "subscription"
        public ActionResult Subscription(string sessionId = "")
        {
            bool IsPaymentInProcess = TempData["PaymentInprocess"] != null ? Convert.ToBoolean(TempData["PaymentInprocess"].ToString()) : false;
            if (IsPaymentInProcess)
            {
                ViewBag.PaymentInProcess = true;
                return View();
            }
            if (restutantInfo.ActivePlan == PlanTypeEnum.Free.ToString())
            {
                ViewBag.IsPaid = false;
                ViewBag.PlanInterval = "---";
                ViewBag.PlanName = "Free Plan";
                ViewBag.PlanCost = 0.00;
                ViewBag.SubscriptionStartDate = "---";
                ViewBag.SubscriptionEndDate = "---";
            }
            else
            {
                StripeConfiguration.ApiKey = ConfigurationManager.AppSettings["SecretKeySuper"].ToString();
                SubscriptionInfoModel objSubscriptionInfo = _subscriptionBusiness.GetActiveSubscriptionInfo(tenantsInfo.TenantConnection);
                PlanModel objPlanModel = GetPlans(restutantInfo.PlanId);
                if (objSubscriptionInfo != null && objSubscriptionInfo.PlanName != null)
                {
                    try
                    {
                        var service = new PriceService();
                        var plan = service.Get(GetPlans(restutantInfo.PlanId).PriceId);
                        ViewBag.PlanInterval = plan.Recurring.Interval == "month" ? "Monthly" : plan.Recurring.Interval == "day" ? "Daily" : "Annually";
                        ViewBag.PlanName = objSubscriptionInfo.PlanName;
                        ViewBag.PlanCost = objSubscriptionInfo.PlanCost;
                        ViewBag.SubscriptionStartDate = objSubscriptionInfo.SubscriptionStartDate;
                        ViewBag.SubscriptionEndDate = objSubscriptionInfo.SubscriptionEndDate;
                        ViewBag.SubscwriptionId = objSubscriptionInfo.SubscriptionId;
                        ViewBag.IsCancelled = restutantInfo.IsSubscriptionCancelled;
                        ViewBag.SubscriptionCancelledOn = restutantInfo.SubscriptionCancelledOn;
                    }
                    catch (Exception ex)
                    {

                    }
                    return View();
                }
                else
                {
                    ViewBag.IsPaid = true;
                    ViewBag.PlanCost = objPlanModel.Price;
                    ViewBag.Publishkey = ConfigurationManager.AppSettings["PublishableKeySuper"].ToString();
                    try
                    {

                        var options1 = new Stripe.Checkout.SessionCreateOptions
                        {
                            AllowPromotionCodes = false,
                            PaymentMethodTypes = new List<string>
                            {
                                "card"
                            },

                            LineItems = new List<SessionLineItemOptions>
                        {
                            new SessionLineItemOptions
                            {
                                Price=objPlanModel.PriceId,
                                Quantity = 1,
                            },
                        },
                            Mode = "subscription",
                            CustomerEmail = restutantInfo.Email,
                            SuccessUrl = ConfigurationManager.AppSettings["SubscriptionSuccessUrl"].ToString(),
                            CancelUrl = ConfigurationManager.AppSettings["SubscriptionCancelUrl"].ToString(),
                        };
                        var service1 = new Stripe.Checkout.SessionService();
                        Stripe.Checkout.Session session = service1.Create(options1);
                        ViewBag.sessionId = session.Id;
                        TempData["sessionId"] = session.Id;
                    }
                    catch (Exception ex)
                    {
                        ViewBag.error = ex.Message.ToString();
                    }
                }
            }
            return View();
        }


       
        [HttpPost]
        public ActionResult CancelSubscription(string subsid)
        {
            try
            {
                StripeConfiguration.ApiKey = ConfigurationManager.AppSettings["SecretKeySuper"].ToString();
                var service = new SubscriptionService();
                Subscription objSubscription = service.Cancel(subsid);
                _subscriptionBusiness.UpdateSubscriptionCancel(true, tenantsInfo.TenantConnection);
                return Content("1");
            }
            catch (Exception ex)
            {
                return Content("-1");
            }

        }

        #endregion

        public PlanModel GetPlans(int planId)
        {
            using (var client = new HttpClient())
            {
                PlanModel objList = new PlanModel();
                client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", "Bearer " + SessionManager.LoginResponse.AccessToken);
                string url = apiBaseUrl + MethodEnum.GetPlanById.GetDescription().ToString() + "?planId=" + planId;
                HttpResponseMessage messge = client.GetAsync(url).Result;
                string result = messge.Content.ReadAsStringAsync().Result;
                if (messge.IsSuccessStatusCode)
                {
                    objList = JsonConvert.DeserializeObject<PlanModel>(result);
                }
                return objList;
            }
        }
    }
}