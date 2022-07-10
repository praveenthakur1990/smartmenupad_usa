using Newtonsoft.Json;
using SmartMenu.BAL.Interfaces;
using SmartMenu.DAL.Common;
using SmartMenu.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SmartMenu.API.Controllers
{
    [Authorize]
    [RoutePrefix("api/QuickPages")]
    public class QuickPagesController : ApiController
    {
        private IQuickPagesBusiness _quickPagesBusiness;
        public QuickPagesController(IQuickPagesBusiness quickPagesBusiness)
        {
            _quickPagesBusiness = quickPagesBusiness;
        }

        [Route("GetQuickPages")]
        [HttpGet]
        public HttpResponseMessage GetQuickPages(string userId)
        {
            List<QuickPagesVM> objList = _quickPagesBusiness.GetQuickPages(CommonManager.getTenantConnection(userId, string.Empty)[0].TenantConnection);
            return Request.CreateResponse(HttpStatusCode.OK, objList);
        }        

        [Route("AddUpdateQuickPages_v2")]
        [HttpPost]
        public HttpResponseMessage AddUpdateQuickPages_v2(List<QuickPagesVM> model)
        {
            try
            {
                foreach (var item in model)
                {
                    _quickPagesBusiness.AddUpdateQuickPages(item, CommonManager.getTenantConnection(item.CreatedBy, string.Empty)[0].TenantConnection);
                }
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message.ToString());
            }
        }
    }
}
