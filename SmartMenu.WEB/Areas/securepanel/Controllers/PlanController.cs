using Newtonsoft.Json;
using SmartMenu.DAL.Models;
using SmartMenu.WEB.Helpers;
using Stripe;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using System.Web.Mvc;
using static SmartMenu.DAL.Enums.EnumHelper;

namespace SmartMenu.WEB.Areas.securepanel.Controllers
{
    [Compress]
    [SuperAdminAuthorize]
    public class PlanController : Controller
    {
        readonly string apiBaseUrl = ConfigurationManager.AppSettings["ApiBaseUrl"].ToString();

        [SuperAdminOnlyAuthorize]
        public ActionResult Index()
        {
            return View(GetPlans());
        }

        [SuperAdminOnlyAuthorize]
        public ActionResult AddUpdatePlan(int id)
        {
            if (id > 0)
            {
                using (var client = new HttpClient())
                {
                    PlanModel obj = null;
                    client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", "Bearer " + SessionManager.LoginResponse.AccessToken);
                    string url = apiBaseUrl + MethodEnum.GetPlanById.GetDescription().ToString() + "?planId=" + id;
                    HttpResponseMessage messge = client.GetAsync(url).Result;
                    string result = messge.Content.ReadAsStringAsync().Result;
                    if (messge.IsSuccessStatusCode)
                    {
                        obj = JsonConvert.DeserializeObject<PlanModel>(result);
                        return View(obj);
                    }
                }
            }
            return View(new PlanModel());
        }

        [HttpPost]
        public int AddUpdatePlan(PlanModel model)
        {
            try
            {
                StripeConfiguration.ApiKey = ConfigurationManager.AppSettings["SecretKeySuper"].ToString();
                if (model.Id == 0)
                {
                    var productOption = new ProductCreateOptions
                    {
                        Name = model.Name
                    };
                    var productService = new ProductService();
                    Product objProduct = productService.Create(productOption);
                    model.ProductId = objProduct.Id;

                    var priceOption = new PriceCreateOptions
                    {
                        Currency = "USD",
                        UnitAmount = Convert.ToInt64(Convert.ToDecimal(model.Price * 100)),
                        Product = objProduct.Id,
                        Recurring = new PriceRecurringOptions()
                        {
                            Interval = model.Interval == "D" ? "day" : (model.Interval == "M" ? "month" : "year")
                        }
                    };
                    var priceService = new PriceService();
                    Price objPrice = priceService.Create(priceOption);
                    model.PriceId = objPrice.Id;
                }
                else
                {
                    var productService = new ProductService();
                    var productUpdateOption = new ProductUpdateOptions()
                    {
                        Name = model.Name
                    };
                    Product objProduct = productService.Update(model.ProductId, productUpdateOption);

                    //var priceOption = new PriceUpdateOptions
                    //{


                    //    Recurring = new PriceRecurringOptions()
                    //    {
                    //        Interval = model.Interval == "D" ? "day" : (model.Interval == "M" ? "month" : "year")
                    //    }
                    //};
                    //var priceService = new PriceService();
                    //Price objPrice = priceService.Update(model.PriceId, priceOption);
                }

                model.CreatedBy = Helpers.SessionManager.LoginResponse.UserId;
                using (var client = new System.Net.Http.HttpClient())
                {
                    client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", "Bearer " + Helpers.SessionManager.LoginResponse.AccessToken);
                    string url = apiBaseUrl + MethodEnum.AddUpdatePlan.GetDescription().ToString();
                    var httpContent = Helpers.CommonManager.CreateHttpContent(model);
                    HttpResponseMessage messge = client.PostAsync(url, httpContent).Result;
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
                return -1;
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