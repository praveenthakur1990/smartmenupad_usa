using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SmartMenu.WEB.Areas.securepanel.Controllers
{
    public class ErrorController : Controller
    {
        public ActionResult NotFound404(string message)
        {
            ViewBag.message = message;
            return View();
        }
        public ActionResult ServerError(string message, string type = "")
        {
            ViewBag.message = message;
            return View();
        }

        public ActionResult UnauthorizedRequest(string message)
        {
            ViewBag.message = message;
            return View();
        }

    }
}