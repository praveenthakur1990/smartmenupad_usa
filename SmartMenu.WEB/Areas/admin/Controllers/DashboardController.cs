using Newtonsoft.Json;
using SmartMenu.DAL.Common;
using SmartMenu.DAL.Models;
using SmartMenu.WEB.Helpers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using static SmartMenu.DAL.Enums.EnumHelper;
using CommonManager = SmartMenu.WEB.Helpers.CommonManager;

namespace SmartMenu.WEB.Areas.admin.Controllers
{
    [Compress]
    [AdminAuthorize]
    public class DashboardController : BaseController
    {
        readonly string apiBaseUrl = ConfigurationManager.AppSettings["ApiBaseUrl"].ToString();
        readonly int _pageSize = Convert.ToInt32(ConfigurationManager.AppSettings["PageSize"].ToString());
        public ActionResult Index(int page = 1, string searchStr = "")
        {
            AdminDashBoardViewModel obj = new AdminDashBoardViewModel();
            using (var client = new HttpClient())
            {
                List<OrderViewModel> objList = new List<OrderViewModel>();
                client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", "Bearer " + Helpers.SessionManager.LoginResponse.AccessToken);
                string url = apiBaseUrl + MethodEnum.GetOrders.GetDescription().ToString() + "?orderId=0&orderNo=" + string.Empty + "&status=" + ConfigurationManager.AppSettings["DashBoardOrderStatuses"].ToString() + "&IsCurrentDate=" + true + "&userId=" + SessionManager.LoginResponse.UserId + "&pageNumber=" + page + "&pageSize=" + _pageSize + "&searchStr=" + searchStr;
                HttpResponseMessage messge = client.GetAsync(url).Result;
                string result = messge.Content.ReadAsStringAsync().Result;
                if (messge.IsSuccessStatusCode)
                {
                    objList = JsonConvert.DeserializeObject<List<OrderViewModel>>(result);
                }
                ViewBag.Pager = new PagerHelper(objList.Select(c => c.TotalRows).FirstOrDefault(), page, _pageSize);
                ViewBag.SearchStr = searchStr;
                obj.LatestOrderList = objList;
                obj.StatsData = GetDashboardStatsData();
                return View(obj);
            }

        }

        public ActionResult Profile()
        {
            RestaurantModelVM obj = new RestaurantModelVM();
            ViewBag.States = new SelectList(Helpers.CommonManager.getStateList(), "value", "Text");
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", "Bearer " + Helpers.SessionManager.LoginResponse.AccessToken);
                string url = apiBaseUrl + MethodEnum.GetRestaurant.GetDescription().ToString() + "?userId=" + SessionManager.LoginResponse.UserId;
                HttpResponseMessage messge = client.GetAsync(url).Result;
                string result = messge.Content.ReadAsStringAsync().Result;
                if (messge.IsSuccessStatusCode)
                {
                    obj = JsonConvert.DeserializeObject<RestaurantModelVM>(result);
                }
                return View(obj);
            }
        }

        public JsonResult GetOrderGraphData(SalesOrderGraphRequestVM model)
        {
            using (var client = new HttpClient())
            {
                int cashOrder = 0, cardOrder = 0;
                string reportMonth = string.Empty, reportYear = string.Empty;
                model.UserId = SessionManager.LoginResponse.UserId;
                model.OrderStatus = "Delivered,Picked";
                List<SalesOrderGraphVM> objList = new List<SalesOrderGraphVM>();
                client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", "Bearer " + Helpers.SessionManager.LoginResponse.AccessToken);
                string url = apiBaseUrl + MethodEnum.GetOrderGraphData.GetDescription().ToString();
                var httpContent = Helpers.CommonManager.CreateHttpContent(model);
                System.Net.Http.HttpResponseMessage messge = client.PostAsync(url, httpContent).Result;
                string result = messge.Content.ReadAsStringAsync().Result;
                if (messge.IsSuccessStatusCode)
                {
                    objList = JsonConvert.DeserializeObject<List<SalesOrderGraphVM>>(result);
                }

                if (objList.Count() > 0)
                {
                    cashOrder = objList.Select(c => c.CashPaymentMode).Sum();
                    cardOrder = objList.Select(c => c.CardPaymentMode).Sum();
                    reportMonth = objList.Select(c => c.MonthName).FirstOrDefault();
                    reportYear = objList.Select(c => c.Year).FirstOrDefault();
                }
                return Json(new { data = objList, cashModeCounter = cashOrder, cardModeCounter = cardOrder, reportMonth = reportMonth, reportYear = reportYear, }, JsonRequestBehavior.AllowGet);
            }
        }

        public DashboardStatDataVM GetDashboardStatsData()
        {
            using (var client = new HttpClient())
            {
                DashboardStatDataVM obj = new DashboardStatDataVM();
                client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", "Bearer " + Helpers.SessionManager.LoginResponse.AccessToken);
                string url = apiBaseUrl + MethodEnum.GetDashBoardStatsData.GetDescription().ToString() + "?userId=" + SessionManager.LoginResponse.UserId;
                HttpResponseMessage messge = client.GetAsync(url).Result;
                string result = messge.Content.ReadAsStringAsync().Result;
                if (messge.IsSuccessStatusCode)
                {
                    obj = JsonConvert.DeserializeObject<DashboardStatDataVM>(result);
                }
                return obj;
            }
        }

        [HttpPost]
        public int SetIsOnlineOffline(bool IsOnline)
        {
            try
            {
                using (var client = new System.Net.Http.HttpClient())
                {
                    client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", "Bearer " + Helpers.SessionManager.LoginResponse.AccessToken);
                    string url = apiBaseUrl + MethodEnum.SetOnlineOfflineRestaurant.GetDescription().ToString() + "?IsOnline=" + IsOnline + "&userId=" + SessionManager.LoginResponse.UserId;
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
        public int SaveImages(List<Images> imagesArr)
        {
            WebSiteImages obj = new WebSiteImages();
            obj.UserId = SessionManager.LoginResponse.UserId;
            List<WebSiteImages> objList = new List<WebSiteImages>();
            for (int i = 0; i < imagesArr.Count(); i++)
            {

                if (imagesArr[i].Type == "banner")
                {
                    obj.BannerImg = new Images();
                    obj.BannerImg.Type = imagesArr[i].Type;
                    obj.BannerImg.Ext = imagesArr[i].Ext;
                    obj.BannerImg.ImagePath = GetImagePath(imagesArr[i].ImagePath);
                }
                if (imagesArr[i].Type == "promotional")
                {
                    obj.PromotionalImg = new Images();
                    obj.PromotionalImg.Type = imagesArr[i].Type;
                    obj.PromotionalImg.Ext = imagesArr[i].Ext;
                    obj.PromotionalImg.ImagePath = GetImagePath(imagesArr[i].ImagePath);
                }
                if (imagesArr[i].Type == "emptycart")
                {
                    obj.EmptyCartImg = new Images();
                    obj.EmptyCartImg.Type = imagesArr[i].Type;
                    obj.EmptyCartImg.Ext = imagesArr[i].Ext;
                    obj.EmptyCartImg.ImagePath = GetImagePath(imagesArr[i].ImagePath);
                }
                if (imagesArr[i].Type == "minOrder")
                {
                    obj.MinOrderImg = new Images();
                    obj.MinOrderImg.Type = imagesArr[i].Type;
                    obj.MinOrderImg.Ext = imagesArr[i].Ext;
                    obj.MinOrderImg.ImagePath = GetImagePath(imagesArr[i].ImagePath);
                }
                if (imagesArr[i].Type == "locationFar")
                {
                    obj.LocationFarAwayImg = new Images();
                    obj.LocationFarAwayImg.Type = imagesArr[i].Type;
                    obj.LocationFarAwayImg.Ext = imagesArr[i].Ext;
                    obj.LocationFarAwayImg.ImagePath = GetImagePath(imagesArr[i].ImagePath);
                }
                if (imagesArr[i].Type == "offline")
                {
                    obj.OfflineImg = new Images();
                    obj.OfflineImg.Type = imagesArr[i].Type;
                    obj.OfflineImg.Ext = imagesArr[i].Ext;
                    obj.OfflineImg.ImagePath = GetImagePath(imagesArr[i].ImagePath);
                }

                objList.Add(obj);
            }

            using (var client = new System.Net.Http.HttpClient())
            {
                client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", "Bearer " + Helpers.SessionManager.LoginResponse.AccessToken);
                var httpContent = CommonManager.CreateHttpContent(obj);
                string url = apiBaseUrl + MethodEnum.UpdateWebsiteImage.GetDescription().ToString();
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

        public string GetImagePath(string imagePath)
        {
            string returnImagePath = string.Empty;
            if (!string.IsNullOrEmpty(imagePath))
            {
                if (imagePath.Split(',').Length == 1)
                {
                    if (imagePath.IndexOf("/Content") >= 0)
                    {
                        returnImagePath = imagePath.Substring(imagePath.IndexOf("/Content"));
                    }
                }
                else
                {
                    if (CommonManager.IsBase64(imagePath.Split(',')[1].ToString()) == true)
                    {
                        returnImagePath = imagePath.Split(',')[1].ToString();
                    }
                    else
                    {
                        returnImagePath = imagePath.Substring(imagePath.IndexOf("/Content")).ToString();
                    }
                }
            }
            return returnImagePath;
        }
    }
}

