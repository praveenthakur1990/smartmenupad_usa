using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace SmartMenu.WEB.Configuration
{
    public class SubdomainRoute : RouteBase
    {
        public override RouteData GetRouteData(HttpContextBase httpContext)
        {
            string hostName = string.Empty, subdomain = string.Empty, controller = string.Empty, action = string.Empty, id = string.Empty; int index = 0;
            var routeData = new RouteData(this, new MvcRouteHandler());
            var ns = new string[] { "SmartMenu.WEB.Controllers" };
            routeData.DataTokens.Add("Namespaces", ns);
            if (httpContext.Request == null || httpContext.Request.Url == null)
            {
                return null;
            }
            hostName = httpContext.Request.Url.Host;
            // hostName = "https://mexicocitynj.smartmenupad.com/";

            if (hostName.Contains("localhost"))
            {
                string[] segments = httpContext.Request.Url.PathAndQuery.TrimStart('/').Split('/');
                if (hostName.Split('.').Length > 0)
                {
                    index = hostName.IndexOf(".");
                    if (index > 0 && String.Join(",", segments) == string.Empty)
                    {
                        subdomain = hostName.Substring(0, index);
                        string[] blacklist = { "www", "dfsd", "mail" };
                        if (blacklist.Contains(subdomain))
                        {
                            return null;
                        }
                        controller = "Menu"; action = "GetMenu"; id = "";
                    }
                    else
                    {
                        if (index > 0)
                        {
                            subdomain = hostName.Substring(0, index);
                        }

                        if(String.Join(",", segments) == string.Empty)
                        {
                            return null;
                        }
                        else
                        {
                            controller = (segments.Length > 0) ? segments[0] : "Menu";
                            action = (segments.Length > 1) ? segments[1].Split('?')[0] : "GetMenu";
                            id = (segments.Length > 2) ? segments[2] : segments.Length > 1 ? (segments[1].Split('?').Length > 1 ? segments[1].Split('?')[1] : "") : "";
                        }
                        
                    }
                    routeData.Values.Add("controller", controller); //Goes to the relevant Controller  class
                    routeData.Values.Add("action", action); //Goes to the relevant action method on the specified Controller
                    routeData.Values.Add("id", id);
                    routeData.Values.Add("subdomain", subdomain); //pass subdomain as argument to action method
                    return routeData;
                }
                return null;
            }
            else
            {
                if (hostName.Split('.').Length > 2)
                {
                    index = hostName.IndexOf(".");
                    subdomain = hostName.Substring(0, index);
                    string[] blacklist = { "www", "yourdomain", "mail" };
                    if (blacklist.Contains(subdomain))
                    {
                        return null;
                    }

                    //index = hostName.IndexOf(".");
                    string[] segments = httpContext.Request.Url.PathAndQuery.TrimStart('/').Split('/');
                    if (index < 0)
                    {
                        return null;
                    }
                    //subdomain = hostName.Substring(0, index);
                    //string[] blacklist = { "www", "yourdomain", "mail" };
                    //if (blacklist.Contains(subdomain))
                    //{
                    //    return null;
                    //}
                    if (index > 0 && String.Join(",", segments) == string.Empty)
                    {
                        //subdomain = hostName.Substring(0, index);
                        controller = "Menu"; action = "GetMenu"; id = "";
                    }
                    else
                    {
                        controller = (segments.Length > 0) ? segments[0] : "Menu";
                        action = (segments.Length > 1) ? segments[1].Split('?')[0] : "GetMenu";
                        id = (segments.Length > 2) ? segments[2] : (segments[1].Split('?').Length > 1 ? segments[1].Split('?')[1] : "");
                    }

                    routeData.Values.Add("controller", controller); //Goes to the relevant Controller  class
                    routeData.Values.Add("action", action); //Goes to the relevant action method on the specified Controller
                    routeData.Values.Add("id", id);
                    routeData.Values.Add("subdomain", subdomain); //pass subdomain as argument to action method
                    return routeData;
                }
                return null;
            }

        }

        public override VirtualPathData GetVirtualPath(RequestContext requestContext, RouteValueDictionary values)
        {
            //Implement your formating Url formating here
            return null;
        }
        //private readonly string _subDomain;
        //private readonly RouteValueDictionary _routeData;
        //private readonly string[] _namespaces;
        //public SubdomainRoute(string subDomain, object routeData, string[] namespaces) :
        //    this(subDomain, new RouteValueDictionary(routeData), namespaces)
        //{ }

        //public SubdomainRoute(string subDomain, RouteValueDictionary routeData, string[] namespaces)
        //{
        //    _subDomain = subDomain;
        //    _routeData = routeData;
        //    _namespaces = namespaces;
        //}

        //public override RouteData GetRouteData(HttpContextBase httpContext)
        //{
        //    var url = httpContext.Request.Headers["HOST"];
        //    string[] test = _namespaces;

        //    var index = url.IndexOf(".", StringComparison.Ordinal);
        //    if (index < 0) return null;

        //    var firstDomain = url.Split('.')[0];
        //    if ((firstDomain.Equals("www") || firstDomain.Equals("localhost"))
        //        && !firstDomain.Equals(_subDomain))
        //        return null;
        //    var dataTokens = new RouteValueDictionary();
        //    var ns = new string[] { "SmartMenu.WEB.Controllers" };
        //    dataTokens["Namespaces"] = ns;

        //    var handler = new MvcRouteHandler();
        //    var result = new RouteData { RouteHandler = handler };
        //    result.DataTokens.Add("Namespaces", _namespaces);
        //    foreach (var route in _routeData)
        //    {
        //        result.Values.Add(route.Key, route.Value);
        //    }
        //    result.Values.Add("subdomain", firstDomain);

        //    return result;
        //}

        //public override VirtualPathData GetVirtualPath(RequestContext requestContext, RouteValueDictionary values)
        //{
        //    return null;
        //}
    }
}