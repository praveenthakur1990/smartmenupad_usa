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

namespace SmartMenu.WEB.Areas.admin.Controllers
{
    [Compress]
    [AdminAuthorize]
    public class ContactUsController : BaseController
    {
        readonly string apiBaseUrl = ConfigurationManager.AppSettings["ApiBaseUrl"].ToString();
        readonly int _pageSize = Convert.ToInt32(ConfigurationManager.AppSettings["PageSize"].ToString());
        public ActionResult Index(int page = 1, string searchStr = "")
        {
            using (var client = new HttpClient())
            {
                List<ContactUsVM> objList = new List<ContactUsVM>();
                client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", "Bearer " + Helpers.SessionManager.LoginResponse.AccessToken);
                string url = apiBaseUrl + MethodEnum.GetContactUsList.GetDescription().ToString() + "?userId=" + SessionManager.LoginResponse.UserId + "&pageNumber=" + page + "&pageSize=" + _pageSize + "&searchStr=" + searchStr;
                HttpResponseMessage messge = client.GetAsync(url).Result;
                string result = messge.Content.ReadAsStringAsync().Result;
                if (messge.IsSuccessStatusCode)
                {
                    objList = JsonConvert.DeserializeObject<List<ContactUsVM>>(result);
                }
                ViewBag.Pager = new PagerHelper(objList.Select(c => c.TotalRows).FirstOrDefault(), page, _pageSize);
                ViewBag.SearchStr = searchStr;
                return View(objList);
            }
        }
    }
}