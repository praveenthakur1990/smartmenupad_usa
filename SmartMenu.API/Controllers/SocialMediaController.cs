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
    [RoutePrefix("api/SocialMedia")]
    public class SocialMediaController : ApiController
    {
        private ISocialMedialinksBusiness _socialMedialinksBusiness;
        public SocialMediaController(ISocialMedialinksBusiness socialMedialinksBusiness)
        {
            _socialMedialinksBusiness = socialMedialinksBusiness;
        }

        [Route("GetSocialMedia")]
        [HttpGet]
        public HttpResponseMessage GetSocialMedia(string userId)
        {
            List<SocialMediaModel> objList = _socialMedialinksBusiness.GetSocialMediaLinks(CommonManager.getTenantConnection(userId, string.Empty)[0].TenantConnection);
            return Request.CreateResponse(HttpStatusCode.OK, objList);
        }

        [Route("AddUpdateSocialMedia")]
        [HttpPost]
        public HttpResponseMessage AddUpdateSocialMedia(string socialMediaLinkJsonStr, string userId)
        {
            try
            {
                int res = _socialMedialinksBusiness.AddUpdateSocialMediaLinks(socialMediaLinkJsonStr, userId, CommonManager.getTenantConnection(userId, string.Empty)[0].TenantConnection);
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
