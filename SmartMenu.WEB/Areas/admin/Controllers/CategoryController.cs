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
    public class CategoryController : BaseController
    {
        readonly string apiBaseUrl = ConfigurationManager.AppSettings["ApiBaseUrl"].ToString();
        public ActionResult Index()
        {
            using (var client = new HttpClient())
            {
                List<CategoryModel> objList = new List<CategoryModel>();
                client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", "Bearer " + Helpers.SessionManager.LoginResponse.AccessToken);
                string url = apiBaseUrl + MethodEnum.GetCategories.GetDescription().ToString() + "?type='All'&userId=" + SessionManager.LoginResponse.UserId;
                HttpResponseMessage messge = client.GetAsync(url).Result;
                string result = messge.Content.ReadAsStringAsync().Result;
                if (messge.IsSuccessStatusCode)
                {
                    objList = JsonConvert.DeserializeObject<List<CategoryModel>>(result);
                }
                else
                {

                }

                return View(objList);
            }

        }

        public ActionResult AddUpdateCategory(int id)
        {
            if (id > 0)
            {
                using (var client = new HttpClient())
                {
                    CategoryModel obj = null;
                    client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", "Bearer " + Helpers.SessionManager.LoginResponse.AccessToken);
                    string url = apiBaseUrl + MethodEnum.GetCategoryById.GetDescription().ToString() + "?categoryId=" + id + "&userId=" + SessionManager.LoginResponse.UserId;
                    HttpResponseMessage messge = client.GetAsync(url).Result;
                    string result = messge.Content.ReadAsStringAsync().Result;
                    if (messge.IsSuccessStatusCode)
                    {
                        obj = JsonConvert.DeserializeObject<CategoryModel>(result);
                        return View(obj);
                    }
                }
            }
            return View(new CategoryModel());
        }

        [HttpPost]
        public int AddUpdateCategory(FormCollection frm)
        {
            try
            {
                string base64StringFSSI = string.Empty, directoryPath = string.Empty, fullPath = string.Empty, fileName = string.Empty;
                try
                {
                    base64StringFSSI = HttpContext.Request["UploadFile"] != string.Empty && HttpContext.Request["UploadFile"] != null ? HttpContext.Request["UploadFile"].Split(',')[1] : null;
                }
                catch (Exception ex)
                {
                    base64StringFSSI = null;
                }

                string hdnFSSAIFilePath = base64StringFSSI != null ? base64StringFSSI : HttpContext.Request["hdnUploadFile"];

                CategoryModel obj = new CategoryModel();
                obj.Id = Convert.ToInt32(frm["Id"].ToString());
                obj.Name = frm["Name"].ToString();
                obj.Description = frm["Description"].ToString();
                obj.ImagePath = hdnFSSAIFilePath;
                obj.PriorityIndex = Convert.ToInt32(frm["PriorityIndex"].ToString());
                obj.CreatedBy = SessionManager.LoginResponse.UserId;
                using (var client = new System.Net.Http.HttpClient())
                {
                    client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", "Bearer " + Helpers.SessionManager.LoginResponse.AccessToken);
                    string url = apiBaseUrl + MethodEnum.AddUpdateCategory.GetDescription().ToString();
                    var httpContent = Helpers.CommonManager.CreateHttpContent(obj);
                    System.Net.Http.HttpResponseMessage messge = client.PostAsync(url, httpContent).Result;
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

        [HttpPost]
        public int MarkAsDeleted(int id)
        {
            if (id > 0)
            {
                using (var client = new HttpClient())
                {
                    int res = 0;
                    client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", "Bearer " + Helpers.SessionManager.LoginResponse.AccessToken);
                    string url = apiBaseUrl + MethodEnum.MarkAsDeletedCategory.GetDescription().ToString() + "?categoryId=" + id + "&userId=" + SessionManager.LoginResponse.UserId;
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