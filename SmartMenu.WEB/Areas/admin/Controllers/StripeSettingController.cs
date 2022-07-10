using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SmartMenu.WEB.Areas.admin.Controllers
{
    public class StripeSettingController : Controller
    {
        public ActionResult SubscriptionSuccess(string session_id)
        {
            var sessionId = TempData["sessionId"] != null ? TempData["sessionId"].ToString() : string.Empty;
            if (!string.IsNullOrEmpty(sessionId))
            {
                TempData["PaymentInprocess"] = true;
            }

            return RedirectToAction("Subscription", "Setting", new { area = "admin" });
        }

        [AllowAnonymous]
        public ActionResult BackToSubscription()
        {
            return RedirectToAction("Subscription", "Setting", new { area = "admin" });
        }
    }
}