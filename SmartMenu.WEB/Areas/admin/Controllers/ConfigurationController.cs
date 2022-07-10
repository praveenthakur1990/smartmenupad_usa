using Newtonsoft.Json;
using SmartMenu.DAL.Models;
using SmartMenu.WEB.Helpers;
using System;
using System.Configuration;
using System.Net.Http;
using System.Web.Mvc;
using static SmartMenu.DAL.Enums.EnumHelper;

namespace SmartMenu.WEB.Areas.admin.Controllers
{
    [Compress]
    [AdminAuthorize]
    public class ConfigurationController : BaseController
    {
        readonly string apiBaseUrl = ConfigurationManager.AppSettings["ApiBaseUrl"].ToString();
        public ActionResult AddUpdateStripeKey()
        {
            using (var client = new HttpClient())
            {
                ViewBag.MaxDeliveryAreaMiles = Math.Round(restutantInfo.MaxDeliveryAreaInMiles);
                ViewBag.MinOrderAmt = restutantInfo.MinOrderAmt;
                APIKeyModel obj = new APIKeyModel();
                client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", "Bearer " + Helpers.SessionManager.LoginResponse.AccessToken);
                string url = apiBaseUrl + MethodEnum.GetAPIKey.GetDescription().ToString() + "?userId=" + SessionManager.LoginResponse.UserId;
                HttpResponseMessage messge = client.GetAsync(url).Result;
                string result = messge.Content.ReadAsStringAsync().Result;
                if (messge.IsSuccessStatusCode)
                {
                    obj = JsonConvert.DeserializeObject<APIKeyModel>(result);
                }
                ViewBag.Latitude = restutantInfo.Latitude;
                ViewBag.Longitude = restutantInfo.Longitude;
                return View(obj);
            }
        }

        [HttpPost]
        public int AddUpdateStripeKey(APIKeyModel model)
        {
            try
            {
                model.CreatedBy = SessionManager.LoginResponse.UserId;
                using (var client = new System.Net.Http.HttpClient())
                {
                    client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", "Bearer " + Helpers.SessionManager.LoginResponse.AccessToken);
                    string url = apiBaseUrl + MethodEnum.AddUpdateAPIKey.GetDescription().ToString();
                    var httpContent = Helpers.CommonManager.CreateHttpContent(model);
                    System.Net.Http.HttpResponseMessage messge = client.PostAsync(url, httpContent).Result;
                    string result = messge.Content.ReadAsStringAsync().Result;
                    if (messge.IsSuccessStatusCode)
                    {
                        return model.Id > 0 ? 2 : 1;
                    }
                    else
                    {
                        return -1;
                    }
                }
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        [HttpPost]
        public int AddUpdateDeliverAreaSetting(decimal minOrderAmt, decimal maxDeliveryAreaInMiles)
        {
            try
            {
                using (var client = new System.Net.Http.HttpClient())
                {
                    client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", "Bearer " + Helpers.SessionManager.LoginResponse.AccessToken);
                    string url = apiBaseUrl + MethodEnum.UpdateDeliverAreaSetting.GetDescription().ToString() + "?minOrderAmt=" + minOrderAmt + "&maxDeliveryAreaInMiles=" + maxDeliveryAreaInMiles + "&userId=" + SessionManager.LoginResponse.UserId;

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
            catch (Exception ex)
            {
                return 0;
            }
        }

        [HttpPost]
        public int AddUpdateTaxCharges(decimal tax, decimal charges, bool isCashOnDelivery)
        {
            try
            {
                using (var client = new System.Net.Http.HttpClient())
                {
                    client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", "Bearer " + Helpers.SessionManager.LoginResponse.AccessToken);
                    string url = apiBaseUrl + MethodEnum.UpdateDeliveryCharges.GetDescription().ToString() + "?tax=" + tax + "&deliveryCharges=" + charges + "&IsCashOnDelivery=" + isCashOnDelivery + "&userId=" + SessionManager.LoginResponse.UserId;
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
            catch (Exception ex)
            {
                return 0;
            }
        }
    }
}