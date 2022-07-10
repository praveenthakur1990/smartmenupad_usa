using Newtonsoft.Json;
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

namespace SmartMenu.WEB.Areas.admin.Controllers
{
    [Compress]
    [AdminAuthorize]
    public class MenuController : BaseController
    {
        readonly string apiBaseUrl = ConfigurationManager.AppSettings["ApiBaseUrl"].ToString();
        public ActionResult Index()
        {
            return View(GetMenuItemList());
        }

        public ActionResult AddUpdateMenu(int id)
        {
            ViewBag.Categories = new SelectList(GetCategories(), "Id", "Name");
            //ViewBag.RecommeddationItem = new SelectList(GetMenuItemList().Where(c => c.Id != id).ToList(), "Id", "Name");
            if (id > 0)
            {
                using (var client = new HttpClient())
                {
                    MenuItemViewModel obj = null;
                    client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", "Bearer " + SessionManager.LoginResponse.AccessToken);
                    string url = apiBaseUrl + MethodEnum.GetMenuById.GetDescription().ToString() + "?menuId=" + id + "&userId=" + SessionManager.LoginResponse.UserId;
                    HttpResponseMessage messge = client.GetAsync(url).Result;
                    string result = messge.Content.ReadAsStringAsync().Result;
                    if (messge.IsSuccessStatusCode)
                    {
                        obj = JsonConvert.DeserializeObject<MenuItemViewModel>(result);
                        return View(obj);
                    }
                }
            }
            var data = new MenuItemViewModel();
            data.IsMultipleSize = false;
            data.IsAddOnsChoices = false;
            data.IsSeasonal = false;
            data.IsOnSelectedDays = false;
            data.IsPublish = true;

            data.MenuMultipleSizesList = new List<MenuMultipleSizesViewModel>();
            data.MenuAddOnsChoicesList = new List<MenuAddOnsChoicesViewModel>();
            return View(data);
        }

        [HttpPost]
        public int AddUpdateMenu(FormCollection frm)
        {
            ViewBag.Categories = new SelectList(GetCategories(), "Id", "Name");
            MenuItemViewModel obj = new MenuItemViewModel();
            obj.Id = Convert.ToInt32(frm["Id"].ToString());
            obj.Name = frm["Name"].ToString();
            obj.CategoryId = Convert.ToInt32(frm["CategoryId"].ToString());
            if (!string.IsNullOrEmpty(frm["ImagePath"].ToString()))
            {
                if (frm["ImagePath"].ToString().Split(',').Length == 1)
                {
                    if (frm["ImagePath"].ToString().IndexOf("/Upload") >= 0)
                    {
                        obj.ImagePath = frm["ImagePath"].ToString().Substring(frm["ImagePath"].ToString().IndexOf("/Upload"));
                    }
                }
                else
                {
                    if (CommonManager.IsBase64(frm["ImagePath"].ToString().Split(',')[1].ToString()) == true)
                    {
                        obj.ImagePath = frm["ImagePath"].ToString().Split(',')[1].ToString();
                    }
                    else
                    {
                        obj.ImagePath = frm["ImagePath"].ToString().Substring(frm["ImagePath"].ToString().IndexOf("/Upload"));
                    }
                }
            }

            if (!string.IsNullOrEmpty(frm["VideoPath"]))
            {
                obj.VideoPath = frm["VideoPath"].ToString();
            }

            obj.IsMultipleSize = Convert.ToBoolean(frm["IsMultipleSizeItem"].ToString());
            obj.MultipleSizesJsonStr = frm["MultipleSizeJsonStr"].ToString();

            obj.Price = Convert.ToDecimal(frm["Price"].ToString());
            obj.Description = frm["Description"].ToString();
            obj.ItemAs = frm["ItemAs"].ToString();
            obj.VegNonVeg = frm["VegNonVeg"].ToString();
            obj.IsPublish = Convert.ToBoolean(frm["IsPublish"].ToString());
            obj.RecommendedItems = frm["RecommendedItems"].ToString();

            obj.IsAddOnsChoices = Convert.ToBoolean(frm["IsAddOnsChoices"].ToString());
            obj.AddOnsJsonStr = frm["AddOnsChoicesItemJsonStr"].ToString();
           
            obj.IsOnSelectedDays = Convert.ToBoolean(frm["IsSelectedDays"].ToString());
            obj.SelectedDays = frm["SelectedWeekdays"].ToString();
            obj.IsSeasonal = Convert.ToBoolean(frm["IsSeasonalItem"].ToString());
            obj.SeasonalMonth = frm["SelectedMonths"].ToString();
            obj.CreatedBy = SessionManager.LoginResponse.UserId;
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", "Bearer " + SessionManager.LoginResponse.AccessToken);
                string url = apiBaseUrl + MethodEnum.AddUpdateMenu.GetDescription().ToString();
                var httpContent = CommonManager.CreateHttpContent(obj);
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

        public List<CategoryModel> GetCategories()
        {
            using (var client = new HttpClient())
            {
                List<CategoryModel> objList = new List<CategoryModel>();
                client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", "Bearer " + SessionManager.LoginResponse.AccessToken);
                string url = apiBaseUrl + MethodEnum.GetCategories.GetDescription().ToString() + "?type=All&userId=" + SessionManager.LoginResponse.UserId;
                HttpResponseMessage messge = client.GetAsync(url).Result;
                string result = messge.Content.ReadAsStringAsync().Result;
                if (messge.IsSuccessStatusCode)
                {
                    objList = JsonConvert.DeserializeObject<List<CategoryModel>>(result);
                }                
                return objList;
            }
        }

        public List<MenuItemViewModel> GetMenuItemList()
        {
            using (var client = new HttpClient())
            {
                List<MenuItemViewModel> objList = new List<MenuItemViewModel>();
                client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", "Bearer " + SessionManager.LoginResponse.AccessToken);
                string url = apiBaseUrl + MethodEnum.GetMenuItemList.GetDescription().ToString() + "?userId=" + SessionManager.LoginResponse.UserId;
                HttpResponseMessage messge = client.GetAsync(url).Result;
                string result = messge.Content.ReadAsStringAsync().Result;
                if (messge.IsSuccessStatusCode)
                {
                    objList = JsonConvert.DeserializeObject<List<MenuItemViewModel>>(result);
                }
                return objList;
            }
        }


        [HttpPost]
        public int MarkAsDeleted(int id)
        {
            if (id > 0)
            {
                using (var client = new HttpClient())
                {
                    int res = 0;
                    client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", "Bearer " + Helpers.SessionManager.LoginResponse.AccessToken);
                    string url = apiBaseUrl + MethodEnum.MarkAsDeletedMenu.GetDescription().ToString() + "?menuId=" + id + "&userId=" + SessionManager.LoginResponse.UserId;
                    HttpResponseMessage messge = client.PostAsync(url, null).Result;
                    string result = messge.Content.ReadAsStringAsync().Result;
                    if (messge.IsSuccessStatusCode)
                    {
                        res = JsonConvert.DeserializeObject<int>(result);
                        return res;
                    }
                }
            }
            return -1;
        }
    }
}