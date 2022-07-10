using SmartMenu.BAL.Interfaces;
using SmartMenu.DAL.Models;
using SmartMenu.WEB.Helpers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace SmartMenu.WEB.Controllers
{
    //[CustomExceptionHandler]
    [Compress]
    public class HomeController : Controller
    {
        readonly IContactUsBusiness _contactUsBusiness;
        public HomeController(IContactUsBusiness contactUsBusiness)
        {
            _contactUsBusiness = contactUsBusiness;
        }
        public ActionResult Index()
        {
            //SmartMenu.DAL.Common.CommonManager.LogError(MethodBase.GetCurrentMethod(), new Exception(), null);
            //SMSManager.SendSMSNotification("+91", "9876693766", ConfigurationManager.AppSettings["OrderReceivedMessage"].ToString().Replace("{0}", "ORD-001").Replace("{1}", String.Format("{0}{1}", "$", "100")));
            //c = a / b;
            return View();
        }

        public ActionResult OnlineMenu()
        {
            return View();
        }

        public ActionResult QrMenu()
        {
            return View();
        }

        [HttpPost]
        public JsonResult ScheduleDemo(RequestDemoModel model)
        {
            return Json(new { res = _contactUsBusiness.SaveRequestForDemo(model) }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult TalkToExpert(TalkToExpertModel model)
        {
            return Json(new { res = _contactUsBusiness.SaveTalkToExpertRequest(model) }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult NewsLetterSubscriber(string emailAddress)
        {
            return Json(new { res = _contactUsBusiness.SaveNewsLetterSubscriber(emailAddress) }, JsonRequestBehavior.AllowGet);
        }
    }
}