using SmartMenu.BAL.Interfaces;
using SmartMenu.DAL.Common;
using SmartMenu.DAL.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SmartMenu.API.Controllers
{
    [Authorize]
    [RoutePrefix("api/Order")]
    public class OrderController : ApiController
    {
        private IOrderBusiness _orderBusiness;
        public OrderController(IOrderBusiness orderBusiness)
        {
            _orderBusiness = orderBusiness;
        }

        [Route("GetOrders")]
        [HttpGet]
        public HttpResponseMessage GetOrders(int orderId, string userId, int pageNumber, int pageSize, string orderNo = "", string status = "", bool IsCurrentDate = false, string searchStr = "")
        {
            PaginationModel obj = new PaginationModel();
            obj.PageNumber = pageNumber;
            obj.PageSize = pageSize;
            obj.SearchStr = searchStr;
            List<OrderViewModel> objList = _orderBusiness.GetOrderList(orderId, orderNo, status, IsCurrentDate, obj, CommonManager.getTenantConnection(userId, string.Empty)[0].TenantConnection);
            return Request.CreateResponse(HttpStatusCode.OK, objList);
        }

        [Route("UpdateOrderStatus")]
        [HttpPost]
        public HttpResponseMessage UpdateOrderStatus(OrderViewModel model)
        {
            try
            {

                int res = _orderBusiness.UpdateOrderStatus(model, CommonManager.getTenantConnection(model.StatusChangedBy, string.Empty)[0].TenantConnection);
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

        [Route("GetOrderGraphData")]
        [HttpPost]
        public HttpResponseMessage GetOrderGraphData(SalesOrderGraphRequestVM model)
        {
            List<SalesOrderGraphVM> objList = _orderBusiness.GetDashBoardGraphData(model, CommonManager.getTenantConnection(model.UserId, string.Empty)[0].TenantConnection);
            return Request.CreateResponse(HttpStatusCode.OK, objList);
        }

        [Route("GetDashBoardStatsData")]
        [HttpGet]
        public HttpResponseMessage GetDashBoardStatsData(string userId)
        {

            DashboardStatDataVM obj = _orderBusiness.GetDashBoardStatsData(CommonManager.getTenantConnection(userId, string.Empty)[0].TenantConnection);
            return Request.CreateResponse(HttpStatusCode.OK, obj);
        }
    }
}
