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
    [RoutePrefix("api/Menu")]
    public class MenuController : ApiController
    {
        readonly string appPhysicalPath = ConfigurationManager.AppSettings["APPPhysicalPath"].ToString();
        private IMenuBusiness _menuBusiness;
        public MenuController(IMenuBusiness menuBusiness)
        {
            _menuBusiness = menuBusiness;
        }

        [Route("AddUpdateMenu")]
        [HttpPost]
        public HttpResponseMessage AddUpdateMenu(MenuItemViewModel obj)
        {

            #region "Saving uploaded document files"
            TenantsVM objTenant = CommonManager.getTenantConnection(obj.CreatedBy, string.Empty).FirstOrDefault();
            string fileName = string.Empty, fullPath = string.Empty, directoryPath = string.Empty, fileUploadPath = string.Empty;
            if (!string.IsNullOrEmpty(obj.ImagePath) && CommonManager.IsBase64(obj.ImagePath) == true)
            {
                byte[] imageBytes = Convert.FromBase64String(obj.ImagePath);
                fileUploadPath = DirectoryPathEnum.Upload.ToString() + "/" + objTenant.TenantName + "/" + DirectoryPathEnum.MenuImage.ToString() + "/";
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

            #endregion           
            _menuBusiness.AddUpdateMenu(obj, objTenant.TenantConnection);
            return Request.CreateResponse(HttpStatusCode.OK, "success");
        }

        [Route("GetMenuItemList")]
        [HttpGet]
        public HttpResponseMessage GetMenuItemList(string userId)
        {
            List<MenuItemViewModel> objList = _menuBusiness.GetAllMenu(0,CommonManager.getTenantConnection(userId, string.Empty)[0].TenantConnection);
            return Request.CreateResponse(HttpStatusCode.OK, objList.OrderBy(c=>c.Name).ToList());
        }

        [Route("GetMenuById")]
        [HttpGet]
        public HttpResponseMessage GetMenuById(int menuId, string userId)
        {
            MenuItemViewModel obj = _menuBusiness.GetAllMenu(menuId, CommonManager.getTenantConnection(userId, string.Empty)[0].TenantConnection).FirstOrDefault();
            return Request.CreateResponse(HttpStatusCode.OK, obj);
        }

        [Route("MarkeAsDelete")]
        [HttpPost]
        public HttpResponseMessage MarkeAsDelete(int menuId, string userId)
        {
            return Request.CreateResponse(HttpStatusCode.OK, _menuBusiness.MarkMenuAsDeleted(menuId, CommonManager.getTenantConnection(userId, string.Empty)[0].TenantConnection));
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
