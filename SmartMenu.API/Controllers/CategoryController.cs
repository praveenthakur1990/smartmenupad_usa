using SmartMenu.BAL.Interfaces;
using SmartMenu.DAL.Common;
using SmartMenu.DAL.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using static SmartMenu.DAL.Enums.EnumHelper;

namespace SmartMenu.API.Controllers
{
    [Authorize]
    [RoutePrefix("api/Category")]
    public class CategoryController : ApiController
    {
        readonly string appPhysicalPath = ConfigurationManager.AppSettings["APPPhysicalPath"].ToString();
        private ICategoryBusiness _categoryBusiness;
        public CategoryController(ICategoryBusiness categoryBusiness)
        {
            this._categoryBusiness = categoryBusiness;
        }

        [Route("AddUpdateCategory")]
        [HttpPost]
        public HttpResponseMessage AddUpdateCategory([FromBody] CategoryModel obj)
        {
            try
            {
                //return Request.CreateResponse(HttpStatusCode.InternalServerError);
                string fileName = string.Empty, fullPath = string.Empty, directoryPath = string.Empty, fileUploadPath = string.Empty;
                if (CommonManager.IsBase64(obj.ImagePath))
                {
                    byte[] imageBytes = Convert.FromBase64String(obj.ImagePath);
                    fileUploadPath = DirectoryPathEnum.Upload.ToString() + "/" + obj.CreatedBy + "/" + DirectoryPathEnum.CategoryImage.ToString() + "/";
                    directoryPath = CreatePathIfMissing(appPhysicalPath + "/" + fileUploadPath);
                    //string[] files = Directory.GetFiles(directoryPath);
                    //foreach (string file in files)
                    //{
                    //    File.Delete(file);
                    //}
                    fileName = CreateUniqueFileName(ExtensionEnum.PNGExtension.GetDescription().ToString());
                    fullPath = directoryPath + fileName;
                    File.WriteAllBytes(fullPath, imageBytes);
                    obj.ImagePath = "/" + fileUploadPath + fileName;
                }
                else if (!string.IsNullOrEmpty(obj.ImagePath))
                {
                    obj.ImagePath = obj.ImagePath.Substring(obj.ImagePath.IndexOf("/Upload"));
                }
                int res = _categoryBusiness.AddUpdateCategory(obj, CommonManager.getTenantConnection(obj.CreatedBy, string.Empty)[0].TenantConnection);
                if (res > 0)
                {
                    return Request.CreateResponse(HttpStatusCode.OK);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.InternalServerError);
                }

            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message.ToString());
            }
        }

        [Route("GetCategories")]
        [HttpGet]
        public HttpResponseMessage GetCategories(string type, string userId)
        {
            List<CategoryModel> objList = _categoryBusiness.GetCategories(type, CommonManager.getTenantConnection(userId, string.Empty)[0].TenantConnection);
            return Request.CreateResponse(HttpStatusCode.OK, objList);
        }


        [Route("GetCategoryById")]
        [HttpGet]
        public HttpResponseMessage GetCategoryById(int categoryId, string userId)
        {
            CategoryModel obj = _categoryBusiness.GetCategories("All", CommonManager.getTenantConnection(userId, string.Empty)[0].TenantConnection).Where(c => c.Id == categoryId).FirstOrDefault();
            return Request.CreateResponse(HttpStatusCode.OK, obj);
        }


        [Route("MarkeAsDelete")]
        [HttpPost]
        public HttpResponseMessage MarkeAsDelete(int categoryId, string userId)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _categoryBusiness.MarkCategoryAsDeleted(categoryId, CommonManager.getTenantConnection(userId, string.Empty)[0].TenantConnection));
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
    }
}
