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
    [RoutePrefix("api/ContactUs")]
    public class ContactUsController : ApiController
    {
        private IContactUsBusiness _contactUsBusiness;
        public ContactUsController(IContactUsBusiness contactUsBusiness)
        {
            _contactUsBusiness = contactUsBusiness;
        }
        [Route("GetContactUsList")]
        [HttpGet]
        public HttpResponseMessage GetContactUsList(string userId, int pageNumber, int pageSize, string searchStr)
        {
            PaginationModel obj = new PaginationModel();
            obj.PageNumber = pageNumber;
            obj.PageSize = pageSize;
            obj.SearchStr = searchStr;
            List<ContactUsVM> objList = _contactUsBusiness.GetContactUsList(obj, CommonManager.getTenantConnection(userId, string.Empty)[0].TenantConnection);
            return Request.CreateResponse(HttpStatusCode.OK, objList);
        }
    }
}
