using Newtonsoft.Json;
using SmartMenu.DAL.Common;
using SmartMenu.DAL.Models;
using SmartMenu.WEB.Helpers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Web.Mvc;
using static SmartMenu.DAL.Enums.EnumHelper;
using CommonManager = SmartMenu.WEB.Helpers.CommonManager;

namespace SmartMenu.WEB.Areas.admin.Controllers
{
    [Compress]
    [AdminAuthorize]
    public class OrdersController : BaseController
    {
        readonly string apiBaseUrl = ConfigurationManager.AppSettings["ApiBaseUrl"].ToString();
        readonly int _pageSize = Convert.ToInt32(ConfigurationManager.AppSettings["PageSize"].ToString());
        public ActionResult Index(int page = 1, string searchStr = "")
        {
            List<OrderViewModel> objOrders = GetOrders(0, string.Empty, string.Empty, false, page, _pageSize, searchStr);
            ViewBag.Pager = new PagerHelper(objOrders.Select(c => c.TotalRows).FirstOrDefault(), page, _pageSize);
            ViewBag.SearchStr = searchStr;
            return View(objOrders);
        }

        public List<OrderViewModel> GetOrders(int orderId, string orderNo, string status, bool IsCurrentDate, int pageNumber, int pageSize, string searchStr)
        {
            using (var client = new HttpClient())
            {
                List<OrderViewModel> objList = new List<OrderViewModel>();
                client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", "Bearer " + Helpers.SessionManager.LoginResponse.AccessToken);
                string url = apiBaseUrl + MethodEnum.GetOrders.GetDescription().ToString() + "?orderId=" + orderId + "&OrderNo=" + orderNo + "&status=" + status + "&IsCurrentDate=" + IsCurrentDate + "&userId=" + SessionManager.LoginResponse.UserId + "&pageNumber=" + pageNumber + "&pageSize=" + pageSize + "&searchStr=" + searchStr;
                HttpResponseMessage messge = client.GetAsync(url).Result;
                string result = messge.Content.ReadAsStringAsync().Result;
                if (messge.IsSuccessStatusCode)
                {
                    objList = JsonConvert.DeserializeObject<List<OrderViewModel>>(result);
                }
                return objList;
            }
        }

        [HttpPost]
        public PartialViewResult ViewOrderDetails(int OrderId)
        {
            OrderViewModel obj = GetOrders(OrderId, string.Empty, string.Empty, false, 1, _pageSize, string.Empty).FirstOrDefault();
            return PartialView("_viewOrderDetails", obj);
        }

        [HttpPost]
        public JsonResult UpdateStatus(OrderViewModel model)
        {
            using (var client = new HttpClient())
            {
                model.StatusChangedBy = SessionManager.LoginResponse.UserId;
                client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", "Bearer " + SessionManager.LoginResponse.AccessToken);
                string url = apiBaseUrl + MethodEnum.UpdateOrderStatus.GetDescription().ToString();
                var httpContent = CommonManager.CreateHttpContent(model);
                HttpResponseMessage messge = client.PostAsync(url, httpContent).Result;
                string result = messge.Content.ReadAsStringAsync().Result;
                if (messge.IsSuccessStatusCode)
                {
                    //sending order status in sms notification
                   // SMSManager.SendSMSNotification(ViewBag.CountryMobileCode, model.MobileNumber, ConfigurationManager.AppSettings["OrderPlacedMessage"].ToString());
                    return Json(new { res = 1 }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { res = -1 }, JsonRequestBehavior.AllowGet);
                }
            }
        }

        [HttpPost]
        public JsonResult ViewPrintOrder(int OrderId, bool IsCustomerCopy)
        {
            string jsonStr = string.Empty;
            try
            {
                OrderViewModel objOrderInfo = GetOrders(OrderId, string.Empty, string.Empty, false, 1, _pageSize, string.Empty).FirstOrDefault();
                objOrderInfo.IsCustomerBillCopy = IsCustomerCopy;
                ViewBag.billType = (objOrderInfo.IsCustomerBillCopy == false ? "(Kitchen Copy)" : "(Customer Copy)");
                //RestaurantModel business = GetRestaurantInfo();
                ViewBag.RestaurantName = restutantInfo.Name;
                ViewBag.Address = restutantInfo.Address;
                ViewBag.RestaurantPhone = restutantInfo.Mobile;
                jsonStr = RenderPartialViewToString("_orderPrint", objOrderInfo);
            }
            catch (Exception ex)
            {
                return Json(new { htmlStr = string.Empty, JsonRequestBehavior.AllowGet });
            }

            return Json(new { htmlStr = jsonStr, JsonRequestBehavior.AllowGet });
        }
        //public RestaurantModel GetRestaurantInfo()
        //{
        //    using (var client = new HttpClient())
        //    {
        //        RestaurantModel obj = new RestaurantModel();
        //        client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", "Bearer " + Helpers.SessionManager.LoginResponse.AccessToken);
        //        string url = apiBaseUrl + MethodEnum.GetRestaurant.GetDescription().ToString() + "?userId=" + SessionManager.LoginResponse.UserId;
        //        HttpResponseMessage messge = client.GetAsync(url).Result;
        //        string result = messge.Content.ReadAsStringAsync().Result;
        //        if (messge.IsSuccessStatusCode)
        //        {
        //            obj = JsonConvert.DeserializeObject<RestaurantModel>(result);
        //        }
        //        return obj;
        //    }
        //}
        public CustomerViewModel GetCustomerInfo(int customerId)
        {
            using (var client = new HttpClient())
            {
                List<CustomerViewModel> objList = new List<CustomerViewModel>();
                client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", "Bearer " + Helpers.SessionManager.LoginResponse.AccessToken);
                string url = apiBaseUrl + MethodEnum.GetCustomers.GetDescription().ToString() + "?customerId=" + customerId + "&userId=" + SessionManager.LoginResponse.UserId;
                HttpResponseMessage messge = client.GetAsync(url).Result;
                string result = messge.Content.ReadAsStringAsync().Result;
                if (messge.IsSuccessStatusCode)
                {
                    objList = JsonConvert.DeserializeObject<List<CustomerViewModel>>(result);
                }
                else
                {

                }

                return objList.Where(c => c.Id == customerId).FirstOrDefault();
            }
        }

        #region "helper"
        public virtual string RenderPartialViewToString(string viewName, object viewmodel)
        {
            if (string.IsNullOrEmpty(viewName))
            {
                viewName = this.ControllerContext.RouteData.GetRequiredString("action");
            }

            ViewData.Model = viewmodel;

            using (var sw = new StringWriter())
            {
                ViewEngineResult viewResult = System.Web.Mvc.ViewEngines.Engines.FindPartialView(this.ControllerContext, viewName);
                var viewContext = new ViewContext(this.ControllerContext, viewResult.View, this.ViewData, this.TempData, sw);
                viewResult.View.Render(viewContext, sw);
                viewResult.ViewEngine.ReleaseView(ControllerContext, viewResult.View);

                return sw.GetStringBuilder().ToString();
            }
        }
        #endregion
    }

    public class PrintText
    {
        public PrintText(string text, Font font) : this(text, font, new StringFormat()) { }

        public PrintText(string text, Font font, StringFormat stringFormat)
        {
            Text = text;
            Font = font;
            StringFormat = stringFormat;
        }

        public string Text { get; set; }

        public Font Font { get; set; }

        /// <summary> Default is horizontal string formatting </summary>
        public StringFormat StringFormat { get; set; }
    }
}