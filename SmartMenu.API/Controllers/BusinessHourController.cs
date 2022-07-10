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
    [RoutePrefix("api/BusinessHour")]
    public class BusinessHourController : ApiController
    {
        private IBusinessHoursBusiness _businessHoursBusiness;
        public BusinessHourController(IBusinessHoursBusiness businessHoursBusiness)
        {
            _businessHoursBusiness = businessHoursBusiness;
        }
        [Route("GetBusinessHours")]
        [HttpGet]
        public HttpResponseMessage GetBusinessHours(string userId)
        {
            List<BusinessHoursModelVM> objList = _businessHoursBusiness.GetBusinessHours(CommonManager.getTenantConnection(userId, string.Empty)[0].TenantConnection);
            return Request.CreateResponse(HttpStatusCode.OK, objList);
        }

        [Route("AddUpdateBusinessHour")]
        [HttpPost]
        public HttpResponseMessage AddUpdateBusinessHour(string businessHourJsonStr, string userId)
        {
            try
            {
                int res = _businessHoursBusiness.AddUpdateBusinessHours(businessHourJsonStr, CommonManager.getTenantConnection(userId, string.Empty)[0].TenantConnection);
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


        [Route("AddUpdatePickUpAddress")]
        [HttpPost]
        public HttpResponseMessage AddUpdatePickUpAddress(string pickUpAddress, string userId)
        {
            try
            {
                int res = _businessHoursBusiness.AddUpdatePickupAddress(pickUpAddress, CommonManager.getTenantConnection(userId, string.Empty)[0].TenantConnection);
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

        [Route("AddCustomLabel")]
        [HttpPost]
        public HttpResponseMessage AddCustomLabel(string customLabelJsonStr, string userId)
        {
            try
            {
                int res = _businessHoursBusiness.AddCustomLabel(customLabelJsonStr, CommonManager.getTenantConnection(userId, string.Empty)[0].TenantConnection);
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
    }
}
