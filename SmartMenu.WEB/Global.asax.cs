using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace SmartMenu.WEB
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {            
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            Exception exception = Server.GetLastError();
            Response.Clear();
            var context = new HttpContextWrapper(Context);

            if (exception != null)
            {
                string action;

                switch (context.Response.StatusCode)
                {
                    case 404:
                        // page not found
                        action = "NotFound404";
                        break;
                    case 500:
                        // server error
                        action = "ServerError";
                        break;
                    default:
                        action = "ServerError";
                        break;
                }

                // clear error on server
                Server.ClearError();
                Response.Redirect(String.Format("~/SecurePanel/Error/{0}/?message={1}", action, exception.Message));
            }
        }

        //protected void Application_BeginRequest()
        //{
        //    string[] allowedOrigin = new string[] { "https://localhost:44339/" };
        //    var origin = HttpContext.Current.Request.Headers["Origin"];
        //    if (origin != null && allowedOrigin.Contains(origin))
        //    {
        //        HttpContext.Current.Response.AddHeader("Access-Control-Allow-Origin", origin);
        //        HttpContext.Current.Response.AddHeader("Access-Control-Allow-Methods", "GET,POST");
        //    }
        //}
    }
}
