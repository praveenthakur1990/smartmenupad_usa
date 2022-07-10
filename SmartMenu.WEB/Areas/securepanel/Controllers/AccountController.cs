using Newtonsoft.Json;
using SmartMenu.DAL.Models;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using System.Web.Mvc;
using static SmartMenu.DAL.Enums.EnumHelper;
using SmartMenu.WEB.Helpers;
namespace SmartMenu.WEB.Areas.securepanel.Controllers
{
    public class AccountController : Controller
    {
        readonly string apiBaseUrl = ConfigurationManager.AppSettings["ApiBaseUrl"].ToString();
        public ActionResult Register()
        {
            return RedirectToAction("Login");
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Register(FormCollection frm)
        {
            //using (var client = new HttpClient())
            //{
            //    client.BaseAddress = new Uri(apiBaseUrl);
            //    //HTTP GET
            //    var responseTask = client.PostAsync("student", );
            //    responseTask.Wait();

            //    var result = responseTask.Result;
            //    if (result.IsSuccessStatusCode)
            //    {
            //        var readTask = result.Content.ReadAsAsync<IList<StudentViewModel>>();
            //        readTask.Wait();

            //        students = readTask.Result;
            //    }
            //    else //web api sent error response 
            //    {
            //        //log response status here..

            //        students = Enumerable.Empty<StudentViewModel>();

            //        ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
            //    }
            //}
            return View();
        }
        public ActionResult Login()
        {
            return View();
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Login(FormCollection frm)
        {
            var kvpList = new List<KeyValuePair<string, string>>
            {
            new KeyValuePair<string, string>("UserName", frm["email"].ToString()),
            new KeyValuePair<string, string>("Password", frm["password"].ToString()),
            new KeyValuePair<string, string>("grant_type", "password")
            };
            FormUrlEncodedContent rqstBody = new FormUrlEncodedContent(kvpList);
            using (var client = new HttpClient())
            {
                string url = apiBaseUrl + MethodEnum.Login.GetDescription().ToString();
                HttpResponseMessage messge = client.PostAsync(url, rqstBody).Result;
                string result = messge.Content.ReadAsStringAsync().Result;
                if (messge.IsSuccessStatusCode)
                {
                    LoginResponse response = JsonConvert.DeserializeObject<LoginResponse>(result);
                    SessionManager.LoginResponse = response;
                    return Content(response.RoleName.ToLower());
                }
                else
                {
                    Errorresponse response = JsonConvert.DeserializeObject<Errorresponse>(result);
                    return Content(response.error_description.ToString());
                }
            }
        }

        [HttpPost]
        public ActionResult Logout()
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", "Bearer " + SessionManager.LoginResponse.AccessToken);
                string url = apiBaseUrl + MethodEnum.Logout.GetDescription().ToString();
                HttpResponseMessage messge = client.PostAsync(url, null).Result;
                string result = messge.Content.ReadAsStringAsync().Result;
                SessionManager.LoginResponse = null;
                return Content("1");
            }
        }
    }
}

