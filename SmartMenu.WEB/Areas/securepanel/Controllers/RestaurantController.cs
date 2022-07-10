using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SmartMenu.DAL.Models;
using System.Configuration;
using static SmartMenu.DAL.Enums.EnumHelper;
using SmartMenu.DAL.Common;
using System.Net.Http;
using Newtonsoft.Json;
using SmartMenu.WEB.Helpers;
using System.Globalization;

namespace SmartMenu.WEB.Areas.securepanel.Controllers
{
    [Compress]
    [SuperAdminAuthorize]
    public class RestaurantController : Controller
    {
        readonly string apiBaseUrl = ConfigurationManager.AppSettings["ApiBaseUrl"].ToString();
        public ActionResult Index()
        {
            List<TenantsVM> objList = new List<TenantsVM>();
            if (SessionManager.LoginResponse.RoleName == RolesEnum.SuperAdmin.ToString())
            {
                objList = DAL.Common.CommonManager.getTenantConnection(string.Empty, string.Empty);
            }
            else
            {
                objList = DAL.Common.CommonManager.getTenantConnection(string.Empty, string.Empty).Where(c => c.CreatedBy == SessionManager.LoginResponse.UserId).ToList();
            }

            return View(objList);
        }

        public ActionResult AddUpdateRestaurant(string userId)
        {
            RestaurantModelVM response = new RestaurantModelVM();
            //ViewBag.States = new SelectList(Helpers.CommonManager.getStateList(), "value", "Text");
            ViewBag.Plans = new SelectList(GetPlans(), "Id", "NamePrice");

            if (!string.IsNullOrEmpty(userId))
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", "Bearer " + Helpers.SessionManager.LoginResponse.AccessToken);
                    string url = apiBaseUrl + MethodEnum.GetRestaurant.GetDescription().ToString() + "?userId=" + userId;
                    HttpResponseMessage messge = client.GetAsync(url).Result;
                    string result = messge.Content.ReadAsStringAsync().Result;
                    if (messge.IsSuccessStatusCode)
                    {
                        response = JsonConvert.DeserializeObject<RestaurantModelVM>(result);
                    }
                    else
                    {

                    }
                }
            }
            return View(response);
        }

        [HttpPost]
        public int AddUpdateRestaurant(FormCollection frm)
        {
            try
            {
                string base64StringFSSI = string.Empty, base64StringGST = string.Empty, base64StringLogo = string.Empty;
                string directoryPath = string.Empty;
                string fullPath = string.Empty;
                string fileName = string.Empty;
                try
                {
                    base64StringFSSI = HttpContext.Request["FSSAIFile"] != string.Empty && HttpContext.Request["FSSAIFile"] != null ? HttpContext.Request["FSSAIFile"].Split(',')[1] : null;
                }
                catch (Exception ex)
                {
                    //base64StringFSSI = null;
                    base64StringFSSI = HttpContext.Request["hdnFSSAIFile"].ToString().Substring(HttpContext.Request["hdnFSSAIFile"].ToString().IndexOf("/Upload"));
                }

                try
                {
                    base64StringGST = HttpContext.Request["GSTFile"] != string.Empty && HttpContext.Request["GSTFile"] != null ? HttpContext.Request["GSTFile"].Split(',')[1] : null;
                }
                catch (Exception ex)
                {
                    ///base64StringGST = null;
                    base64StringGST = HttpContext.Request["hdnGSTFile"].ToString().Substring(HttpContext.Request["hdnGSTFile"].ToString().IndexOf("/Upload"));
                }

                try
                {
                    base64StringLogo = HttpContext.Request["LogoFile"] != string.Empty && HttpContext.Request["LogoFile"] != null ? HttpContext.Request["LogoFile"].Split(',')[1] : null;
                }
                catch (Exception ex)
                {
                    // base64StringLogo = null;
                    base64StringLogo = HttpContext.Request["hdnLogoFile"].ToString().Substring(HttpContext.Request["hdnLogoFile"].ToString().IndexOf("/Upload"));
                }

                string hdnFSSAIFilePath = base64StringFSSI != null ? base64StringFSSI : HttpContext.Request["hdnFSSAIFile"];
                string hdnGSTFilePath = base64StringGST != null ? base64StringGST : HttpContext.Request["hdnGSTFile"];
                string hdnLogoFilePath = base64StringLogo != null ? base64StringLogo : HttpContext.Request["hdnLogoFile"];
                RestaurantModel obj = new RestaurantModel();
                obj.Id = Convert.ToInt32(frm["Id"].ToString());
                obj.Name = frm["Name"].ToString();
                obj.Email = frm["Email"].ToString();
                if (obj.Id == 0)
                {
                    obj.Email = string.Format("{0}_{1}", obj.Email, frm["subDomainName"].ToString());
                }
                obj.Mobile = frm["Mobile"].ToString();
                obj.Address = frm["Address"].ToString();
                obj.Landmark = string.Empty;
                obj.State = frm["state"].ToString();
                obj.City = frm["City"].ToString();
                obj.ZipCode = frm["ZipCode"].ToString();
                obj.CountryCode = frm["CountryCode"].ToString();
                obj.Latitude = frm["Latitude"].ToString();
                obj.Longitude = frm["Longitude"].ToString();
                obj.ContactPersonName = frm["ContactPersonName"].ToString();
                obj.ContactNumber = frm["ContactNumber"].ToString();

                obj.AverageCostTwoPerson = Convert.ToDecimal(frm["AverageCostTwoPerson"].ToString() == string.Empty ? "0" : frm["AverageCostTwoPerson"].ToString());
                obj.TypeOfRestaurant = frm["TypeOfRestaurant"].ToString();
                obj.RestaurantMealOption = frm["RestaurantMealOption"].ToString();
                obj.NumberOfTable = Convert.ToInt32(frm["NumberOfTable"].ToString() == string.Empty ? "0" : frm["NumberOfTable"].ToString());
                obj.Cuisnes = frm["Cuisnes"].ToString();
                obj.FSSAIRegistration = frm["FSSAIRegistration"].ToString();
                obj.GSTRegistrationNumber = frm["GSTRegistrationNumber"].ToString();
                obj.ActivePlan = frm["ActivePlan"].ToString();
                obj.AnnualCost = Convert.ToDecimal(frm["AnnualCost"].ToString() == string.Empty ? "0" : frm["AnnualCost"].ToString());
                obj.Commision = Convert.ToDecimal(frm["Commision"].ToString() == string.Empty ? "0" : frm["Commision"].ToString());
                obj.PlanActiveDate = Convert.ToDateTime(frm["PlanActiveDate"].ToString());
                obj.CurrencySymbol = frm["CurrencySymbol"].ToString();
                obj.TimeZone = frm["TimeZone"].ToString();
                obj.CreatedBy = Helpers.SessionManager.LoginResponse.UserId;
                obj.FSSAIFilePath = hdnFSSAIFilePath;
                obj.GSTFilePath = hdnGSTFilePath;
                obj.LogoPath = hdnLogoFilePath;

                obj.PlanId = obj.ActivePlan == "Free" ? 0 : !string.IsNullOrEmpty(frm["PlanId"].ToString()) ? Convert.ToInt32(frm["PlanId"].ToString()) : 0;
                using (var client = new System.Net.Http.HttpClient())
                {
                    client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", "Bearer " + Helpers.SessionManager.LoginResponse.AccessToken);
                    string url = apiBaseUrl + MethodEnum.AddUpdateRestaurant.GetDescription().ToString();
                    var httpContent = Helpers.CommonManager.CreateHttpContent(obj);
                    HttpResponseMessage messge = client.PostAsync(url, httpContent).Result;
                    string result = messge.Content.ReadAsStringAsync().Result;
                    if (messge.IsSuccessStatusCode)
                    {
                        return obj.Id > 0 ? 2 : 1;
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

        public List<PlanModel> GetPlans()
        {
            using (var client = new HttpClient())
            {
                List<PlanModel> objList = new List<PlanModel>();
                client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", "Bearer " + SessionManager.LoginResponse.AccessToken);
                string url = apiBaseUrl + MethodEnum.GetPlanList.GetDescription().ToString();
                HttpResponseMessage messge = client.GetAsync(url).Result;
                string result = messge.Content.ReadAsStringAsync().Result;
                if (messge.IsSuccessStatusCode)
                {
                    objList = JsonConvert.DeserializeObject<List<PlanModel>>(result);
                }
                return objList;
            }
        }
    }
}