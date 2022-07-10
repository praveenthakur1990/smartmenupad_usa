using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using SmartMenu.DAL.Models;
using SmartMenu.DAL.Enums;
using System.Configuration;
using static SmartMenu.DAL.Enums.EnumHelper;
using Newtonsoft.Json;
using SmartMenu.BAL.Interfaces;
using SmartMenu.DAL.Common;
using System.IO;
using System.Linq;

namespace SmartMenu.API.Controllers
{
    [Authorize]
    [RoutePrefix("api/Restaurant")]
    public class RestaurantController : ApiController
    {
        readonly string appPhysicalPath = ConfigurationManager.AppSettings["APPPhysicalPath"].ToString();
        private IRestaurantBusiness _restaurantBusiness;
        public RestaurantController(IRestaurantBusiness restaurantBusiness)
        {
            _restaurantBusiness = restaurantBusiness;
        }
        [Route("AddUpdateRestaurant")]
        [HttpPost]
        public HttpResponseMessage AddUpdateRestaurant(RestaurantModel obj)
        {
            string emailAddress = string.Empty, subdomain = string.Empty;
            if (obj.Id == 0)
            {
                emailAddress = obj.Email.Split('_')[0].ToString();
                subdomain = obj.Email.Split('_')[1].ToString();
            }
            else
            {
                emailAddress = obj.Email;
                subdomain = string.Empty;
            }
            try
            {
                var kvpList = new List<KeyValuePair<string, string>>
            {
            new KeyValuePair<string, string>("Email", emailAddress),
            new KeyValuePair<string, string>("Password", "@Password1"),
            new KeyValuePair<string, string>("ConfirmPassword", "@Password1"),
              new KeyValuePair<string, string>("RoleName", "admin"),
              new KeyValuePair<string, string>("SubDomainName",subdomain.Replace(" ", "").ToLower()),
               new KeyValuePair<string, string>("CreatedBy",obj.CreatedBy)

            };
                FormUrlEncodedContent rqstBody = new FormUrlEncodedContent(kvpList);
                using (var client = new HttpClient())
                {
                    HttpResponseMessage messge = null;
                    string result = string.Empty;
                    string url = ConfigurationManager.AppSettings["ApiBaseUrl"].ToString() + MethodEnum.Register.GetDescription().ToString();
                    if (obj.Id == 0)
                    {
                        messge = client.PostAsync(url, rqstBody).Result;
                        result = messge.Content.ReadAsStringAsync().Result;
                    }
                    else
                    {
                        messge = new HttpResponseMessage();
                        messge = Request.CreateResponse(HttpStatusCode.OK);
                    }

                    if (messge.IsSuccessStatusCode)
                    {
                        #region "Saving uploaded document files"
                        string fileName = string.Empty, fullPath = string.Empty, directoryPath = string.Empty, fileUploadPath = string.Empty;
                        if (!string.IsNullOrEmpty(obj.FSSAIFilePath) && CommonManager.IsBase64(obj.FSSAIFilePath) == true)
                        {
                            byte[] imageBytes = Convert.FromBase64String(obj.FSSAIFilePath);
                            fileUploadPath = DirectoryPathEnum.Upload.ToString() + "/" + emailAddress + "/" + DirectoryPathEnum.RestaurantDoc.ToString() + "/" + DirectoryPathEnum.FSSIDoc.ToString() + "/";
                            directoryPath = CreatePathIfMissing(appPhysicalPath + "/" + fileUploadPath);
                            string[] files = Directory.GetFiles(directoryPath);
                            foreach (string file in files)
                            {
                                File.Delete(file);
                            }
                            fileName = CreateUniqueFileName(ExtensionEnum.PdfExtension.GetDescription().ToString());
                            fullPath = directoryPath + fileName;
                            File.WriteAllBytes(fullPath, imageBytes);
                            obj.FSSAIFilePath = "/" + fileUploadPath + fileName;
                        }

                        if (!string.IsNullOrEmpty(obj.GSTFilePath) && CommonManager.IsBase64(obj.GSTFilePath) == true)
                        {
                            byte[] imageBytes = Convert.FromBase64String(obj.GSTFilePath);
                            fileUploadPath = DirectoryPathEnum.Upload.ToString() + "/" + emailAddress + "/" + DirectoryPathEnum.RestaurantDoc.ToString() + "/" + DirectoryPathEnum.GSTDOc.ToString() + "/";
                            directoryPath = CreatePathIfMissing(appPhysicalPath + "/" + fileUploadPath);
                            string[] files = Directory.GetFiles(directoryPath);
                            foreach (string file in files)
                            {
                                File.Delete(file);
                            }
                            fileName = CreateUniqueFileName(ExtensionEnum.PdfExtension.GetDescription().ToString());
                            fullPath = directoryPath + fileName;
                            File.WriteAllBytes(fullPath, imageBytes);
                            obj.GSTFilePath = "/" + fileUploadPath + fileName;
                        }


                        if (!string.IsNullOrEmpty(obj.LogoPath) && CommonManager.IsBase64(obj.LogoPath) == true)
                        {
                            byte[] imageBytes = Convert.FromBase64String(obj.LogoPath);
                            fileUploadPath = DirectoryPathEnum.Upload.ToString() + "/" + emailAddress + "/" + DirectoryPathEnum.Logo.ToString() + "/";
                            directoryPath = CreatePathIfMissing(appPhysicalPath + "/" + fileUploadPath);
                            string[] files = Directory.GetFiles(directoryPath);
                            foreach (string file in files)
                            {
                                File.Delete(file);
                            }
                            fileName = CreateUniqueFileName(ExtensionEnum.PNGExtension.GetDescription().ToString());
                            fullPath = directoryPath + fileName;
                            File.WriteAllBytes(fullPath, imageBytes);
                            obj.LogoPath = "/" + fileUploadPath + fileName;
                        }
                        #endregion

                        string connectionStr = CommonManager.getTenantConnection(string.Empty, emailAddress)[0].TenantConnection;
                        obj.Email = emailAddress;
                        _restaurantBusiness.AddUpdateRestaurant(obj, connectionStr);
                        return Request.CreateResponse(HttpStatusCode.OK, "success");
                    }
                    else
                    {
                        Errorresponse response = JsonConvert.DeserializeObject<Errorresponse>(result);
                        return Request.CreateResponse(HttpStatusCode.InternalServerError, response.error_description);
                    }
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message.ToString());
            }
        }

        [Route("GetRestaurant")]
        [HttpGet]
        public HttpResponseMessage GetRestaurant(string userId)
        {
            RestaurantModelVM obj = _restaurantBusiness.GetRestaurantDetail(CommonManager.getTenantConnection(userId, string.Empty)[0].TenantConnection);
            return Request.CreateResponse(HttpStatusCode.OK, obj);
        }

        [Route("SetOnlineOfflineRestaurant")]
        [HttpPost]
        public HttpResponseMessage SetOnlineOfflineRestaurant(bool IsOnline, string userId)
        {
            int response = _restaurantBusiness.SetOnlineOfflineRestaurant(IsOnline, CommonManager.getTenantConnection(userId, string.Empty)[0].TenantConnection);
            if (response == 1)
            {
                return Request.CreateResponse(HttpStatusCode.OK, response);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }

        }

        [Route("UpdateDeliverAreaSetting")]
        [HttpPost]
        public HttpResponseMessage UpdateDeliverAreaSetting(decimal minOrderAmt, decimal maxDeliveryAreaInMiles, string userId)
        {
            int response = _restaurantBusiness.UpdateDeliveryAreaSetting(minOrderAmt, maxDeliveryAreaInMiles, CommonManager.getTenantConnection(userId, string.Empty)[0].TenantConnection);
            if (response == 1)
            {
                return Request.CreateResponse(HttpStatusCode.OK, response);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }

        }

        [Route("UpdateWebsiteImage")]
        [HttpPost]
        public HttpResponseMessage UpdateWebsiteImage(WebSiteImages obj)
        {
            try
            {
                if (obj.BannerImg.Type == "banner")
                {
                    if (!string.IsNullOrEmpty(obj.BannerImg.ImagePath) && CommonManager.IsBase64(obj.BannerImg.ImagePath) == true)
                    {
                        obj.BannerImg.ImagePath = ConvertBase64ToImagePath(obj.BannerImg.ImagePath, obj.BannerImg.Ext, obj.UserId);
                    }
                }

                if (obj.PromotionalImg.Type == "promotional")
                {
                    if (!string.IsNullOrEmpty(obj.PromotionalImg.ImagePath) && CommonManager.IsBase64(obj.PromotionalImg.ImagePath) == true)
                    {
                        obj.PromotionalImg.ImagePath = ConvertBase64ToImagePath(obj.PromotionalImg.ImagePath, obj.PromotionalImg.Ext, obj.UserId);
                    }
                }

                if (obj.EmptyCartImg.Type == "emptycart")
                {
                    if (!string.IsNullOrEmpty(obj.EmptyCartImg.ImagePath) && CommonManager.IsBase64(obj.EmptyCartImg.ImagePath) == true)
                    {
                        obj.EmptyCartImg.ImagePath = ConvertBase64ToImagePath(obj.EmptyCartImg.ImagePath, obj.EmptyCartImg.Ext, obj.UserId);
                    }
                }
                if (obj.MinOrderImg.Type == "minOrder")
                {
                    if (!string.IsNullOrEmpty(obj.MinOrderImg.ImagePath) && CommonManager.IsBase64(obj.MinOrderImg.ImagePath) == true)
                    {
                        obj.MinOrderImg.ImagePath = ConvertBase64ToImagePath(obj.MinOrderImg.ImagePath, obj.MinOrderImg.Ext, obj.UserId);
                    }
                }
                if (obj.LocationFarAwayImg.Type == "locationFar")
                {
                    if (!string.IsNullOrEmpty(obj.LocationFarAwayImg.ImagePath) && CommonManager.IsBase64(obj.LocationFarAwayImg.ImagePath) == true)
                    {
                        obj.LocationFarAwayImg.ImagePath = ConvertBase64ToImagePath(obj.LocationFarAwayImg.ImagePath, obj.LocationFarAwayImg.Ext, obj.UserId);
                    }
                }
                if (obj.OfflineImg.Type == "offline")
                {
                    if (!string.IsNullOrEmpty(obj.OfflineImg.ImagePath) && CommonManager.IsBase64(obj.OfflineImg.ImagePath) == true)
                    {
                        obj.OfflineImg.ImagePath = ConvertBase64ToImagePath(obj.OfflineImg.ImagePath, obj.OfflineImg.Ext, obj.UserId);
                    }
                }

                _restaurantBusiness.UpdateImages(obj, CommonManager.getTenantConnection(obj.UserId, string.Empty)[0].TenantConnection);
                return Request.CreateResponse(HttpStatusCode.OK, "success");
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message.ToString());
            }
        }

        [Route("UpdateDeliveryCharges")]
        [HttpPost]
        public HttpResponseMessage UpdateDeliveryCharges(decimal tax, decimal deliveryCharges, bool IsCashOnDelivery, string userId)
        {
            int response = _restaurantBusiness.AddUpdateDeliveryCharges(tax, deliveryCharges, IsCashOnDelivery, CommonManager.getTenantConnection(userId, string.Empty)[0].TenantConnection);
            if (response == 2)
            {
                return Request.CreateResponse(HttpStatusCode.OK, response);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }

        }

        public static string CreatePathIfMissing(string path)
        {
            bool folderExists = Directory.Exists(path);
            if (!folderExists)
                System.IO.Directory.CreateDirectory(path);
            return path;
        }
        public static string CreateUniqueFileName(string type)
        {
            return string.Format(@"{0}", DateTime.Now.Ticks + type);
        }

        public string ConvertBase64ToImagePath(string base64Str, string ext, string userid)
        {
            string imagePath = string.Empty, fileName = string.Empty, fullPath = string.Empty, directoryPath = string.Empty, fileUploadPath = string.Empty;
            byte[] imageBytes = Convert.FromBase64String(base64Str);
            fileUploadPath = DirectoryPathEnum.Upload.ToString() + "/" + userid + "/" + DirectoryPathEnum.WebsiteImages.ToString() + "/";
            directoryPath = CreatePathIfMissing(appPhysicalPath + "/" + fileUploadPath);
            string[] files = Directory.GetFiles(directoryPath);
            //foreach (string file in files)
            //{
            //    File.Delete(file);
            //}
            fileName = CreateUniqueFileName(ext);
            fullPath = directoryPath + fileName;
            File.WriteAllBytes(fullPath, imageBytes);
            imagePath = "/" + fileUploadPath + fileName;
            return imagePath;

        }
    }
}
