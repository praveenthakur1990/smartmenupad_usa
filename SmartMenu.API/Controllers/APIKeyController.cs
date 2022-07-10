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
    [RoutePrefix("api/APIKey")]
    public class APIKeyController : ApiController
    {
        private IAPIKeyBusiness _apiKeyBusiness;
        public APIKeyController(IAPIKeyBusiness apiKeyBusiness)
        {
            _apiKeyBusiness = apiKeyBusiness;
        }

        [Route("AddUpdateAPIKey")]
        [HttpPost]
        public HttpResponseMessage AddUpdateAPIKey([FromBody] APIKeyModel obj)
        {
            try
            {
                int res = _apiKeyBusiness.AddUpdateAPIKey(obj, CommonManager.getTenantConnection(obj.CreatedBy, string.Empty)[0].TenantConnection);
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

        [Route("GetAPIKey")]
        [HttpGet]
        public HttpResponseMessage GetAPIKey(string userId)
        {
            APIKeyModel obj = _apiKeyBusiness.GetAPIKey(CommonManager.getTenantConnection(userId, string.Empty)[0].TenantConnection);
            return Request.CreateResponse(HttpStatusCode.OK, obj);
        }
    }
}
