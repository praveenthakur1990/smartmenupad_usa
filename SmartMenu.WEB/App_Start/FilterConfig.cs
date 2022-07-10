using SmartMenu.WEB.Helpers;
using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SmartMenu.WEB
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
    public class CompressAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {

            var _encodingsAccepted = filterContext.HttpContext.Request.Headers["Accept-Encoding"];
            if (string.IsNullOrEmpty(_encodingsAccepted)) return;

            _encodingsAccepted = _encodingsAccepted.ToLowerInvariant();
            var _response = filterContext.HttpContext.Response;

            if (_encodingsAccepted.Contains("deflate"))
            {
                _response.AppendHeader("Content-encoding", "deflate");
                _response.Filter = new System.IO.Compression.DeflateStream(_response.Filter, System.IO.Compression.CompressionMode.Compress);
            }
            else if (_encodingsAccepted.Contains("gzip"))
            {
                _response.AppendHeader("Content-encoding", "gzip");
                _response.Filter = new System.IO.Compression.GZipStream(_response.Filter, System.IO.Compression.CompressionMode.Compress);
            }
        }
    }
    public class SuperAdminAuthorizeAttribute : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            bool authorize = false;
            if (Helpers.SessionManager.LoginResponse != null && (Helpers.SessionManager.LoginResponse.RoleName.ToLower() == DAL.Enums.EnumHelper.RolesEnum.SuperAdmin.ToString().ToLower() || Helpers.SessionManager.LoginResponse.RoleName.ToLower() == DAL.Enums.EnumHelper.RolesEnum.Partner.ToString().ToLower()))
            {
                authorize = true;
            }
            return authorize;
        }
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new HttpUnauthorizedResult();
            //filterContext.HttpContext.Response.Redirect("/Account/Login");
            filterContext.Result = new RedirectResult("/Securepanel/Account/Login");

        }
    }
    public class AdminAuthorizeAttribute : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            bool authorize = false;
            if (Helpers.SessionManager.LoginResponse != null && Helpers.SessionManager.LoginResponse.RoleName.ToLower() == DAL.Enums.EnumHelper.RolesEnum.Admin.ToString().ToLower())
            {
                authorize = true;
            }
            return authorize;
        }
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new HttpUnauthorizedResult();
            //filterContext.HttpContext.Response.Redirect("/Account/Login");
            filterContext.Result = new RedirectResult("/Securepanel/Account/Login");

        }
    }
    public class SuperAdminOnlyAuthorizeAttribute : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            bool authorize = false;
            if (Helpers.SessionManager.LoginResponse != null && (Helpers.SessionManager.LoginResponse.RoleName.ToLower() == DAL.Enums.EnumHelper.RolesEnum.SuperAdmin.ToString().ToLower()))
            {
                authorize = true;
            }
            return authorize;
        }
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new HttpUnauthorizedResult();
            //filterContext.HttpContext.Response.Redirect("/Account/Login");
            filterContext.Result = new RedirectResult("/Securepanel/Account/Login");

        }
    }
    public class CheckIsCartEmptyAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (SessionManager.AddTocartMenuItem == null || SessionManager.AddTocartMenuItem.Count() == 0)
            {
                filterContext.Result = new RedirectResult("~/Menu/GetMenu");
            }
            else
            {
                decimal totalAmt = 0;
                decimal subTotal = SessionManager.AddTocartMenuItem.Select(c => c.TotalAmount).Sum();
                decimal taxRate = filterContext.Controller.ViewBag.Tax;
                decimal taxAmt = Convert.ToDecimal(String.Format("{0:0.00}", (subTotal * taxRate / 100)));
                decimal deliveryCharges = filterContext.Controller.ViewBag.DeliveryCharges;
                totalAmt = subTotal + taxAmt + deliveryCharges;
                filterContext.Controller.ViewBag.TaxAmt = taxAmt;
                filterContext.Controller.ViewBag.TotalAmt = Convert.ToDecimal(String.Format("{0:0.00}", totalAmt));
            }
        }
    }
    public class CustomExceptionHandler : HandleErrorAttribute
    {
        public override void OnException(ExceptionContext filterContext)
        {
            string controller = filterContext.RouteData.Values["controller"].ToString();
            var areas = filterContext.RouteData.DataTokens["area"] ?? string.Empty;
            string action = filterContext.RouteData.Values["action"].ToString();
            if (filterContext.ExceptionHandled || filterContext.HttpContext.IsCustomErrorEnabled)
            {
                return;
            }
            if (filterContext.HttpContext.Request.IsAjaxRequest())
            {
                filterContext.Result = new JsonResult
                {
                    Data = new
                    {
                        errorMessage = filterContext.Exception.Message.ToString()
                    },
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
            else
            {
                Exception e = filterContext.Exception;
                filterContext.ExceptionHandled = true;
                filterContext.Result = new RedirectResult("/Securepanel/Error/ServerError?message=" + e.Message.ToString() + "&type=" + (!string.IsNullOrEmpty(areas.ToString()) ? areas.ToString() : string.Empty));
            }

            //filterContext.Result = new ViewResult()
            //{
            //    ViewName = "Error"
            //};
        }
    }

    //public class TenantActionFilter : ActionFilterAttribute, IActionFilter
    //{
    //    public override void OnActionExecuting(ActionExecutingContext filterContext)
    //    {
    //        string subDomainStr = (string)filterContext.RequestContext.RouteData.Values["subdomain"];
    //        if (subDomainStr == null)
    //        {
    //            filterContext.Result = new HttpUnauthorizedResult();
    //            filterContext.Result = new RedirectResult("/Securepanel/Account/Login");
    //        }
    //        base.OnActionExecuting(filterContext);
    //    }
    //}
}

