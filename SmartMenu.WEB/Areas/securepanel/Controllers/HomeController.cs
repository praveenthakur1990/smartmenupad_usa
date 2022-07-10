using SmartMenu.BAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SmartMenu.WEB.Areas.securepanel.Controllers
{
    //[CustomExceptionHandler]
    [Compress]
    [SuperAdminAuthorize]
    public class HomeController : Controller
    {
        readonly IContactUsBusiness _contactUsBusiness;
        public HomeController(IContactUsBusiness contactUsBusiness)
        {
            _contactUsBusiness = contactUsBusiness;
        }
        public ActionResult Index()
        {
            //int a = 1;
            //int b = 0;
            //int c = 0;
            //c = a / b;
            return View();
        }

        [SuperAdminOnlyAuthorize]
        public ActionResult RequestForDemoList()
        {
            return View(_contactUsBusiness.RequestForDemoList());
        }

        [SuperAdminOnlyAuthorize]
        public ActionResult TalkToExpertList()
        {
            return View(_contactUsBusiness.TalkToExpertRequestList());
        }

        [SuperAdminOnlyAuthorize]
        public ActionResult NewsLetterSubscriberList()
        {
            return View(_contactUsBusiness.NewsLetterSubscriberList());
        }
    }
}