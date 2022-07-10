using Newtonsoft.Json;
using SmartMenu.BAL.Interfaces;
using SmartMenu.DAL.Common;
using SmartMenu.DAL.Models;
using SmartMenu.WEB.Helpers;
using Stripe;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Web.Mvc;
using static SmartMenu.DAL.Enums.EnumHelper;
using CommonManager = SmartMenu.DAL.Common.CommonManager;

namespace SmartMenu.WEB.Controllers
{
    //[CustomExceptionHandler]
    public class MenuController : MultitenantBaseController
    {
        #region "private members"
        private IRestaurantBusiness _restaurantBusiness;
        private ICategoryBusiness _categoryBusiness;
        private IMenuBusiness _menuBusiness;
        private ICustomerBusiness _customerBusiness;
        private IOrderBusiness _orderBusiness;
        private IPaymentBusiness _paymentBusiness;
        private IAPIKeyBusiness _apiKeyBusiness;
        private IContactUsBusiness _contactUsBusiness;
        readonly string apiBaseUrl = ConfigurationManager.AppSettings["ApiBaseUrl"].ToString();
        #endregion

        #region "constructor"
        public MenuController(IRestaurantBusiness restaurantBusiness, ICategoryBusiness categoryBusiness, IMenuBusiness menuBusiness, ICustomerBusiness customerBusiness, IOrderBusiness orderBusiness, IPaymentBusiness paymentBusiness, IAPIKeyBusiness apiKeyBusiness, IContactUsBusiness contactUsBusiness)
        {
            _restaurantBusiness = restaurantBusiness;
            _categoryBusiness = categoryBusiness;
            _menuBusiness = menuBusiness;
            _customerBusiness = customerBusiness;
            _orderBusiness = orderBusiness;
            _paymentBusiness = paymentBusiness;
            _apiKeyBusiness = apiKeyBusiness;
            ViewBag.quickLinks = quickLinks;
            _contactUsBusiness = contactUsBusiness;
        }
        #endregion

        #region "menu section"
        public ActionResult GetMenu(string mode = "")
        {
            MenuDataModel obj = new MenuDataModel();
            obj.RestaurantModel = restutantInfo;
            obj.CategoryModelList = GetCategories(tenantInfo.TenantConnection).Where(c => c.IsActive == true).ToList();
            obj.MenuItemViewModelList = GetMenuItem(tenantInfo.TenantConnection).ToList();
            ViewBag.mode = mode;
            ViewBag.title = obj.RestaurantModel.Name + "-Online Menu";
            return View(obj);
        }
        public List<CategoryModel> GetCategories(string connectionStr)
        {
            List<CategoryModel> objList = _categoryBusiness.GetCategories(string.Empty, connectionStr).ToList();
            return objList;
        }
        public List<MenuItemViewModel> GetMenuItem(string connectionStr)
        {
            List<MenuItemViewModel> objList = _menuBusiness.GetAllMenu(0, connectionStr).Where(c => c.FinalPrice > 0 && c.IsShowOnCurrentMonth == true && c.IsShowOnCurrentDay == true).ToList();
            return objList;
        }
        public PartialViewResult GetMenuDetail(int index, int menuId, string type)
        {
            MenuItemViewModel obj = null;
            if (type == "edit")
            {
                var data = SessionManager.AddTocartMenuItem.ElementAt(index);
                if (data != null)
                {
                    obj = new MenuItemViewModel();
                    obj.Id = data.Id;
                    obj.Name = data.Name;
                    obj.Price = data.Price;
                    obj.Qty = data.Qty;
                    obj.ImagePath = data.ImagePath;
                    obj.Description = data.Description;
                    obj.IsMultipleSize = data.IsMultipleSize;
                    obj.FinalPrice = data.TotalAmount;
                    obj.CategoryImagePath = data.CategoryImagePath;
                    obj.AddedMenuMultipleSizesList = new List<MenuMultipleSizesViewModel>();
                    if (data.ItemSizeList != null && data.ItemSizeList.Count() > 0)
                    {
                        foreach (var item in data.ItemSizeList)
                        {
                            obj.AddedMenuMultipleSizesList.Add(item);
                        }
                    }
                    obj.AddedMenuAddOnsChoicesList = new List<MenuAddOnsChoicesViewModel>();
                    if (data.AddOnsItemList != null && data.AddOnsItemList.Count() > 0)
                    {
                        foreach (var item in data.AddOnsItemList)
                        {
                            obj.AddedMenuAddOnsChoicesList.Add(item);
                        }
                    }
                    //var res = CommonManager.getTenantConnection(string.Empty, ConfigurationManager.AppSettings["TenantName"].ToString()).FirstOrDefault();
                    var result = _menuBusiness.GetAllMenu(menuId, tenantInfo.TenantConnection).ToList().FirstOrDefault();
                    obj.MenuMultipleSizesList = result.MenuMultipleSizesList;
                    obj.MenuAddOnsChoicesList = result.MenuAddOnsChoicesList;
                }
            }
            else
            {
                //var res = CommonManager.getTenantConnection(string.Empty, ConfigurationManager.AppSettings["TenantName"].ToString()).FirstOrDefault();
                obj = _menuBusiness.GetAllMenu(menuId, tenantInfo.TenantConnection).ToList().FirstOrDefault();
                obj.Qty = 1;
            }
            obj.ActionType = type;
            obj.RowIndex = index;
            return PartialView("_GetMenuDetail", obj);
        }
        #endregion

        #region "add/remove to cart"
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddToCart(MenuCartModel obj)
        {
            if (SessionManager.AddTocartMenuItem.Count() == 0)
            {
                SessionManager.AddTocartMenuItem = new List<MenuCartModel>();
            }

            int index = SessionManager.AddTocartMenuItem.FindIndex(c => c.RowIndex == obj.RowIndex);
            if (obj.ActionType == "edit")
            {
                var item = SessionManager.AddTocartMenuItem.ElementAt(obj.RowIndex);
                if (item != null)
                {
                    SessionManager.AddTocartMenuItem.Remove(item);
                }
            }

            decimal totalAmtPerItem = 0, totalAmt = 0;

            #region "calculating per item total amount"
            if (obj.IsMultipleSize == true)
            {
                if (obj.ItemSizeList != null && obj.ItemSizeList.Count() > 0)
                {
                    for (int j = 0; j < obj.ItemSizeList.Count(); j++)
                    {
                        totalAmtPerItem += obj.ItemSizeList[j].Price.HasValue ? obj.ItemSizeList[j].Price.Value : 0;
                        obj.Price = obj.ItemSizeList[j].Price.HasValue ? obj.ItemSizeList[j].Price.Value : 0;
                    }
                }
            }
            else
            {
                totalAmtPerItem += obj.Price;
            }

            if (obj.AddOnsItemList != null && obj.AddOnsItemList.Count() > 0)
            {
                for (int j = 0; j < obj.AddOnsItemList.Count(); j++)
                {
                    if (obj.AddOnsItemList[j].AddOnChoiceItems != null && obj.AddOnsItemList[j].AddOnChoiceItems.Count() > 0)
                    {
                        totalAmtPerItem += obj.AddOnsItemList[j].AddOnChoiceItems.Select(c => c.Price).Sum();
                    }

                }
            }
            obj.TotalAmount = totalAmtPerItem * obj.Qty;

            #endregion
            if (obj.ActionType == "")
            {
                SessionManager.AddTocartMenuItem.Insert(obj.RowIndex, obj);
            }
            else
            {
                SessionManager.AddTocartMenuItem.Add(obj);
            }

            if (SessionManager.AddTocartMenuItem.Count() > 0)
            {
                totalAmt = Convert.ToDecimal(SessionManager.AddTocartMenuItem.Select(c => c.TotalAmount).Sum());
            }

            string viewStr = RenderPartialViewToString("_CartDetails", null);
            return Json(new { htmlStr = viewStr, totalAmt = totalAmt }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult RemoveCartItem(int index, int id)
        {
            string viewStr = string.Empty;
            MenuCartModel obj = SessionManager.AddTocartMenuItem.ElementAt(index);
            if (obj != null)
            {
                SessionManager.AddTocartMenuItem.RemoveAt(index);
            }
            decimal totalAmt = 0;
            if (SessionManager.AddTocartMenuItem.Count() > 0)
            {
                totalAmt = SessionManager.AddTocartMenuItem.Select(c => c.TotalAmount).Sum();
            }
            viewStr = RenderPartialViewToString("_CartDetails", null);
            return Json(new { htmlStr = viewStr, totalAmt = totalAmt }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult EditCartQty(int index, int newQty)
        {
            string viewStr = string.Empty;
            var obj = SessionManager.AddTocartMenuItem.ElementAt(index);
            SessionManager.AddTocartMenuItem.Remove(obj);
            obj.Qty = newQty;
            decimal totalAmtPerItem = 0, totalAmt = 0;

            #region "calculating per item total amount"
            if (obj.IsMultipleSize == true)
            {
                if (obj.ItemSizeList != null && obj.ItemSizeList.Count() > 0)
                {
                    for (int j = 0; j < obj.ItemSizeList.Count(); j++)
                    {
                        totalAmtPerItem += obj.ItemSizeList[j].Price.HasValue ? obj.ItemSizeList[j].Price.Value : 0;
                        obj.Price = obj.ItemSizeList[j].Price.HasValue ? obj.ItemSizeList[j].Price.Value : 0;
                    }
                }
            }
            else
            {
                totalAmtPerItem += obj.Price;
            }

            if (obj.AddOnsItemList != null && obj.AddOnsItemList.Count() > 0)
            {
                for (int j = 0; j < obj.AddOnsItemList.Count(); j++)
                {
                    totalAmtPerItem += obj.AddOnsItemList[j].AddOnChoiceItems.Select(c => c.Price).Sum();
                }
            }
            obj.TotalAmount = totalAmtPerItem * obj.Qty;

            #endregion
            SessionManager.AddTocartMenuItem.Insert(index, obj);
            if (SessionManager.AddTocartMenuItem.Count() > 0)
            {
                totalAmt = SessionManager.AddTocartMenuItem.Select(c => c.TotalAmount).Sum();
            }
            viewStr = RenderPartialViewToString("_CartDetails", null);
            return Json(new { htmlStr = viewStr, totalAmt = totalAmt }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region "checkOut"
        [CheckIsCartEmpty]
        public ActionResult ProceedForCheckout()
        {
            ViewBag.States = new SelectList(Helpers.CommonManager.getStateList(), "value", "Text");
            MenuDataModel obj = new MenuDataModel();
            ViewBag.Publishkey = _apiKeyBusiness.GetAPIKey(tenantInfo.TenantConnection).Publishablekey;
            obj.RestaurantModel = restutantInfo;

            //decimal _totalCartAmt = SessionManager.AddTocartMenuItem != null && SessionManager.AddTocartMenuItem.Count() > 0 ? SessionManager.AddTocartMenuItem.Select(c => c.TotalAmount).Sum() : 0;
            return View(obj);
        }
        [HttpPost]
        public JsonResult SaveUserInfo(CustomerAddressesViewModel model)
        {
            string resStr = string.Empty, encrptId = string.Empty;
            if (SessionManager.AddTocartMenuItem != null && SessionManager.AddTocartMenuItem.Count() > 0)
            {
                if (model.CustomerAddressId > 0)
                {
                    PaginationModel objPagination = new PaginationModel();
                    objPagination.PageSize = _pageSize;
                    CustomerViewModel objCustomerInfo = _customerBusiness.GetCustomerList(model.CustomerId, model.PhoneNumber == null ? string.Empty : model.PhoneNumber, objPagination, tenantInfo.TenantConnection).FirstOrDefault();
                    CustomerAddressesModel objCustomerAddress = objCustomerInfo.CustomerAddresses.Where(c => c.CustomerAddressId == model.CustomerAddressId).FirstOrDefault();

                    decimal totalDistanceInMiles = _restaurantBusiness.CalculateDistanceBetweenRestaurantAndCustomer(Convert.ToDecimal(restutantInfo.Latitude), Convert.ToDecimal(restutantInfo.Longitude), Convert.ToDecimal(objCustomerAddress.Latitude == null ? "0" : objCustomerAddress.Latitude), Convert.ToDecimal(objCustomerAddress.Longitude == null ? "0" : objCustomerAddress.Longitude), tenantInfo.TenantConnection);
                    decimal totalCartAmt = 0;
                    if (SessionManager.AddTocartMenuItem.Count() > 0)
                    {
                        totalCartAmt = Convert.ToDecimal(SessionManager.AddTocartMenuItem.Select(c => c.TotalAmount).Sum());
                    }
                    decimal MaxDeliveryAreaInMiles = restutantInfo.MaxDeliveryAreaInMiles;
                    decimal MinDeliveryOrdered = restutantInfo.MinOrderAmt;
                    bool IsFarAway = false, IsMinOrderAmtvalid = true;
                    //if (totalDistanceInMiles > MaxDeliveryAreaInMiles)
                    //{
                    //    IsFarAway = true;
                    //}
                    //if (totalCartAmt < MinDeliveryOrdered)
                    //{
                    //    IsMinOrderAmtvalid = false;
                    //}

                    if (IsFarAway == true || IsMinOrderAmtvalid == false)
                    {
                        return Json(new { totalDistance = totalDistanceInMiles, totalCartAmt = totalCartAmt, IsFarAway = IsFarAway, IsMinOrderAmtvalid = IsMinOrderAmtvalid }, JsonRequestBehavior.AllowGet);
                    }
                }
                model.CreatedBy = tenantInfo.UserId;
                int res = _customerBusiness.AddUpdateCustomerAddress(model, tenantInfo.TenantConnection);
                if (res != -1)
                {
                    encrptId = CryptoEngine.EncryptNew(res.ToString());
                    if (model.CustomerAddressId > 0)
                    {
                        resStr = RenderPartialViewToString("~/Views/Menu/_PaymentSection.cshtml", null);
                    }
                    else
                    {
                        ViewBag.States = new SelectList(Helpers.CommonManager.getStateList(), "value", "Text");
                        PaginationModel objPagination = new PaginationModel();
                        objPagination.PageSize = _pageSize;
                        List<CustomerViewModel> objCustomerList = _customerBusiness.GetCustomerList(res, model.PhoneNumber, objPagination, tenantInfo.TenantConnection);
                        if (objCustomerList.Count() > 0)
                        {
                            resStr = RenderPartialViewToString("~/Views/Menu/_DeliveryAddress.cshtml", objCustomerList.FirstOrDefault());
                        }
                        else
                        {
                            CustomerViewModel ObjCustVM = new CustomerViewModel();
                            ObjCustVM.CustomerAddresses = new List<CustomerAddressesModel>();
                            resStr = RenderPartialViewToString("~/Views/Menu/_DeliveryAddress.cshtml", ObjCustVM);
                        }
                    }
                    return Json(new { result = "1", CustomerId = encrptId, htmlStr = resStr, type = model.CustomerAddressId > 0 ? "payment" : "address" }, JsonRequestBehavior.AllowGet);
                }
                return Json(new { result = "-1", CustomerId = encrptId, htmlStr = "" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { result = "-2", CustomerId = encrptId, htmlStr = "" }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult loadPaymentOption(string type, int customerAddressId, string mobileNumber)
        {
            if (type == "Delivery")
            {
                PaginationModel objPagination = new PaginationModel();
                objPagination.PageSize = _pageSize;
                CustomerViewModel objCustomerInfo = _customerBusiness.GetCustomerList(0, mobileNumber, objPagination, tenantInfo.TenantConnection).FirstOrDefault();
                CustomerAddressesModel objCustomerAddress = objCustomerInfo.CustomerAddresses.Where(c => c.CustomerAddressId == customerAddressId).FirstOrDefault();

                decimal totalDistanceInMiles = _restaurantBusiness.CalculateDistanceBetweenRestaurantAndCustomer(Convert.ToDecimal(restutantInfo.Latitude), Convert.ToDecimal(restutantInfo.Longitude), Convert.ToDecimal(objCustomerAddress.Latitude == null ? "0" : objCustomerAddress.Latitude), Convert.ToDecimal(objCustomerAddress.Longitude == null ? "0" : objCustomerAddress.Longitude), tenantInfo.TenantConnection);
                decimal totalCartAmt = 0;
                if (SessionManager.AddTocartMenuItem.Count() > 0)
                {
                    totalCartAmt = Convert.ToDecimal(SessionManager.AddTocartMenuItem.Select(c => c.TotalAmount).Sum());
                }
                decimal MaxDeliveryAreaInMiles = restutantInfo.MaxDeliveryAreaInMiles;
                decimal MinDeliveryOrdered = restutantInfo.MinOrderAmt;
                bool IsFarAway = false, IsMinOrderAmtvalid = true;
                if (totalDistanceInMiles > MaxDeliveryAreaInMiles)
                {
                    IsFarAway = true;
                }
                if (totalCartAmt < MinDeliveryOrdered)
                {
                    IsMinOrderAmtvalid = false;
                }

                if (IsFarAway == true || IsMinOrderAmtvalid == false)
                {
                    return Json(new { result = "-1", totalDistance = totalDistanceInMiles, totalCartAmt = totalCartAmt, IsFarAway = IsFarAway, IsMinOrderAmtvalid = IsMinOrderAmtvalid }, JsonRequestBehavior.AllowGet);
                }
            }

            string resStr = string.Empty;
            ViewBag.OrderType = type;
            resStr = RenderPartialViewToString("~/Views/Menu/_LoadPaymentMethod.cshtml", null);
            return Json(new { result = "1", htmlStr = resStr }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ValidatePickupDateTime(string pickupDateTime)
        {
            string resStr = string.Empty;
            ViewBag.OrderType = "PickUp";
            resStr = RenderPartialViewToString("~/Views/Menu/_LoadPaymentMethod.cshtml", null);
            return Json(new { result = "1", htmlStr = resStr }, JsonRequestBehavior.AllowGet);
            //List<PickupAvailabilityModel> objPickupAvailabilty = _orderBusiness.CheckPickupDateTimeAvailability(pickupDateTime, tenantInfo.TenantConnection);
            //if (objPickupAvailabilty.Count() > 0)
            //{
            //    ViewBag.OrderType = "PickUp";
            //    resStr = RenderPartialViewToString("~/Views/Menu/_LoadPaymentMethod.cshtml", null);
            //    return Json(new { result = "1", htmlStr = resStr }, JsonRequestBehavior.AllowGet);
            //}
            //else
            //{
            //    return Json(new { result = "0", htmlStr = resStr }, JsonRequestBehavior.AllowGet);
            //}
        }

        [HttpPost]
        public JsonResult ValidateOrderDistance(string destinationLatitude, string destinationLongitude)
        {
            decimal totalDistanceInMiles = _restaurantBusiness.CalculateDistanceBetweenRestaurantAndCustomer(Convert.ToDecimal(restutantInfo.Latitude), Convert.ToDecimal(restutantInfo.Longitude), Convert.ToDecimal(destinationLatitude), Convert.ToDecimal(destinationLongitude), tenantInfo.TenantConnection);
            decimal totalCartAmt = 0;
            if (SessionManager.AddTocartMenuItem.Count() > 0)
            {
                totalCartAmt = Convert.ToDecimal(SessionManager.AddTocartMenuItem.Select(c => c.TotalAmount).Sum());
            }
            decimal MaxDeliveryAreaInMiles = restutantInfo.MaxDeliveryAreaInMiles;
            decimal MinDeliveryOrdered = restutantInfo.MinOrderAmt;
            bool IsFarAway = false, IsMinOrderAmtvalid = true;
            if (totalDistanceInMiles > MaxDeliveryAreaInMiles)
            {
                IsFarAway = true;
            }
            if (totalCartAmt < MinDeliveryOrdered)
            {
                IsMinOrderAmtvalid = false;
            }

            return Json(new { totalDistance = totalDistanceInMiles, totalCartAmt = totalCartAmt, IsFarAway = IsFarAway, IsMinOrderAmtvalid = IsMinOrderAmtvalid, MinDeliveryOrderedAmt = MinDeliveryOrdered }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ValidateOrderAmount()
        {
            decimal totalCartAmt = 0;
            if (SessionManager.AddTocartMenuItem.Count() > 0)
            {
                totalCartAmt = Convert.ToDecimal(SessionManager.AddTocartMenuItem.Select(c => c.TotalAmount).Sum());
            }
            decimal MinDeliveryOrdered = restutantInfo.MinOrderAmt;
            bool IsMinOrderAmtvalid = true;
            if (totalCartAmt < MinDeliveryOrdered)
            {
                IsMinOrderAmtvalid = false;
            }
            return Json(new { IsMinOrderAmtvalid = IsMinOrderAmtvalid, MinDeliveryOrderedAmt = MinDeliveryOrdered }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetPickUpTime(int WeekDayId)
        {
            List<PickUpTimeVM> objList = (List<PickUpTimeVM>)ViewBag.PickUpTimeArr;
            objList = objList.Where(c => c.WeekDayId == WeekDayId).ToList();
            return Json(new { result = objList }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region "send/verify otp"
        [HttpPost]
        public ActionResult SendOtp(string mobileNumber)
        {
            SessionManager.OTPNumber = CommonManager.GenerateRandomNo().ToString();
            if (SMSManager.SendSMSNotification(ViewBag.CountryMobileCode, mobileNumber, ConfigurationManager.AppSettings["OTPSendMessage"].ToString().Replace("{0}", SessionManager.OTPNumber)))
            {
                return Content("1");
            }
            else
            {
                return Content("-1");
            }
        }

        [HttpPost]
        public JsonResult VerifyOtp_v1(string mobileNumber, string otp)
        {
            string jsonStr = string.Empty;
            if (Convert.ToBoolean(ConfigurationManager.AppSettings["IsSendOtp"].ToString()) == true)
            {
                if (otp == SessionManager.OTPNumber)
                {
                    PaginationModel objPagination = new PaginationModel();
                    objPagination.PageSize = _pageSize;
                    List<CustomerViewModel> objCustomerList = _customerBusiness.GetCustomerList(0, mobileNumber, objPagination, tenantInfo.TenantConnection);
                    if (objCustomerList.Count() > 0)
                    {
                        jsonStr = RenderPartialViewToString("~/Views/Menu/_DeliveryAddress.cshtml", objCustomerList.Where(c => c.MobileNumber == mobileNumber).FirstOrDefault());
                    }
                    else
                    {
                        CustomerViewModel ObjCustVM = new CustomerViewModel();
                        ObjCustVM.CustomerAddresses = new List<CustomerAddressesModel>();
                        jsonStr = RenderPartialViewToString("~/Views/Menu/_DeliveryAddress.cshtml", ObjCustVM);
                    }

                    return Json(new { result = "1", htmlStr = jsonStr }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { result = "-1", htmlStr = string.Empty }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                PaginationModel objPagination = new PaginationModel();
                objPagination.PageSize = _pageSize;
                List<CustomerViewModel> objCustomerList = _customerBusiness.GetCustomerList(0, mobileNumber, objPagination, tenantInfo.TenantConnection);
                if (objCustomerList.Count() > 0)
                {
                    jsonStr = RenderPartialViewToString("~/Views/Menu/_DeliveryAddress.cshtml", objCustomerList.Where(c => c.MobileNumber == mobileNumber).FirstOrDefault());
                }
                else
                {
                    CustomerViewModel ObjCustVM = new CustomerViewModel();
                    ObjCustVM.CustomerAddresses = new List<CustomerAddressesModel>();
                    jsonStr = RenderPartialViewToString("~/Views/Menu/_DeliveryAddress.cshtml", ObjCustVM);
                }
                return Json(new { result = "1", htmlStr = jsonStr }, JsonRequestBehavior.AllowGet);
            }

        }
        #endregion

        #region "place Order"
        [CheckIsCartEmpty]
        [HttpPost]
        public ActionResult PlaceOrder(string stripeToken, string stripeEmail, string customerIdStr, string specialInstruction)
        {
            try
            {
                string decryptCustomerId = string.Empty;
                int orderId = 0;
                if (Request.IsAjaxRequest())
                {
                    if (SessionManager.AddTocartMenuItem != null && SessionManager.AddTocartMenuItem.Count() > 0)
                    {
                        PaginationModel objPagination = new PaginationModel();
                        objPagination.PageSize = _pageSize;
                        decryptCustomerId = CryptoEngine.DecryptNew(customerIdStr);
                        var objCustomer = _customerBusiness.GetCustomerList(Convert.ToInt32(decryptCustomerId), string.Empty, objPagination, tenantInfo.TenantConnection).FirstOrDefault();

                        var jsonStr = JsonConvert.SerializeObject(SessionManager.AddTocartMenuItem);
                        OrderModel obj = new OrderModel();
                        obj.CustomerAddressId = objCustomer.CustomerAddresses.Where(c => c.IsDefault).Select(c => c.CustomerAddressId).FirstOrDefault();
                        obj.OrderNo = _orderBusiness.GenerateOrderNumber(tenantInfo.TenantConnection);
                        obj.OrderedDetails = jsonStr;
                        obj.Status = OrderStatusEnum.Pending.GetDescription().ToString();
                        obj.Mode = PaymentModeEnum.Cash.ToString();
                        obj.SpecialInstruction = specialInstruction == null ? string.Empty : specialInstruction;
                        obj.TaxRate = ViewBag.Tax;
                        obj.TaxAmt = ViewBag.TaxAmt;
                        obj.DeliveryCharges = ViewBag.DeliveryCharges;
                        orderId = _orderBusiness.AddUpdateOrder(obj, tenantInfo.TenantConnection);
                        if (orderId > 0)
                        {
                            RestaurantModelVM objRestaurant = restutantInfo;
                            //List<MenuCartModel> objCartItems = JsonConvert.DeserializeObject<List<MenuCartModel>>(jsonStr);
                            decimal totalAmount = ViewBag.TotalAmt;
                            //if (objCartItems.Count() > 0)
                            //{
                            //    totalAmount = objCartItems.Select(c => c.TotalAmount).Sum();
                            //}
                            //Sending message to admin on order receiving
                            SMSManager.SendSMSNotification(ViewBag.CountryMobileCode, objRestaurant.Mobile, ConfigurationManager.AppSettings["OrderReceivedMessage"].ToString().Replace("{0}", obj.OrderNo).Replace("{1}", String.Format("{0}{1}", restutantInfo.CurrencySymbol, totalAmount.ToString())));

                            //Sending message to cutomer on successfully recived
                            SMSManager.SendSMSNotification(ViewBag.CountryMobileCode, objCustomer.MobileNumber, ConfigurationManager.AppSettings["OrderPlacedMessage"].ToString());
                            SessionManager.AddTocartMenuItem = new List<MenuCartModel>();
                            return Json(new { result = CryptoEngine.EncryptNew(orderId.ToString()) }, JsonRequestBehavior.AllowGet);
                        }
                    }
                    return Json(new { result = -1 }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    string _pickUpAddress = Request.Form["hdnPickUpAddress"].ToString();
                    int hdnCustomerId = Request.Form["hdnCustomerId"] != string.Empty ? Convert.ToInt32(CryptoEngine.DecryptNew(Request.Form["hdnCustomerId"])) : 0;
                    PaginationModel objPagination = new PaginationModel();
                    objPagination.PageSize = _pageSize;
                    var objCustomer = _customerBusiness.GetCustomerList(hdnCustomerId, string.Empty, objPagination, tenantInfo.TenantConnection).FirstOrDefault();

                    //var totalAmount = Convert.ToDecimal(Request.Form["hdnTotalAmount"]);
                    var totalAmount = ViewBag.TotalAmt;
                    CustomerAddressesModel objCustomerAddress = objCustomer.CustomerAddresses.Where(c => c.IsDefault).FirstOrDefault();

                    RestaurantModelVM objRestaurant = restutantInfo;
                    PaymentModel objPayments = new PaymentModel();
                    try
                    {
                        if (SessionManager.AddTocartMenuItem != null && SessionManager.AddTocartMenuItem.Count() > 0)
                        {
                            APIKeyModel objApiKeyInfo = _apiKeyBusiness.GetAPIKey(tenantInfo.TenantConnection);
                            StripeConfiguration.SetApiKey(objApiKeyInfo.Publishablekey);
                            StripeConfiguration.ApiKey = objApiKeyInfo.Secretkey;
                            var customers = new CustomerService();
                            var charges = new ChargeService();

                            var customer = customers.Create(new CustomerCreateOptions()
                            {
                                Name = string.Format("{0} {1}", objCustomerAddress.FirstName, objCustomerAddress.LastName),
                                Address = new AddressOptions()
                                {
                                    Line1 = objCustomerAddress.Address1,
                                    Line2 = objCustomerAddress.Address2 == null ? string.Empty : objCustomerAddress.Address2,
                                    City = objCustomerAddress.City,
                                    State = objCustomerAddress.State,
                                    Country = restutantInfo.CountryCode,
                                    PostalCode = objCustomerAddress.ZipCode
                                },
                                Phone = objCustomer.MobileNumber,
                                Email = stripeEmail,
                                Source = stripeToken
                            });

                            //Int64 am = Convert.ToInt64(Convert.ToDecimal(totalAmount * 100));
                            var charge = charges.Create(new ChargeCreateOptions()
                            {
                                Amount = Convert.ToInt64(Convert.ToDecimal(totalAmount * 100)),//charge in cents
                                Description = Request.Form["Description"],
                                Currency = ViewBag.CurrencyCode,
                                Customer = customer.Id
                            });
                            if (charge.Captured && !string.IsNullOrEmpty(charge.Id))
                            {
                                var jsonStr = JsonConvert.SerializeObject(SessionManager.AddTocartMenuItem);
                                OrderModel obj = new OrderModel();
                                obj.CustomerAddressId = objCustomerAddress.CustomerAddressId;
                                obj.OrderNo = _orderBusiness.GenerateOrderNumber(tenantInfo.TenantConnection);
                                obj.OrderedDetails = jsonStr;
                                obj.Mode = PaymentModeEnum.Card.ToString();
                                obj.Status = OrderStatusEnum.Pending.GetDescription().ToString();
                                obj.OrderedType = Request.Form["hdnOrderType"].ToString();
                                if (!string.IsNullOrEmpty(Request.Form["hdnPickUpDateTime"]) && obj.OrderedType == "P")
                                {
                                    obj.PickUpDateTime = Convert.ToDateTime(Request.Form["hdnPickUpDateTime"].ToString());
                                }
                                obj.SpecialInstruction = Request.Form["hdnSpecialInstruction"].ToString();
                                obj.PickUpAddress = _pickUpAddress;
                                obj.TaxRate = ViewBag.Tax;
                                obj.TaxAmt = ViewBag.TaxAmt;
                                obj.DeliveryCharges = ViewBag.DeliveryCharges;
                                orderId = _orderBusiness.AddUpdateOrder(obj, tenantInfo.TenantConnection);

                                objPayments.OrderId = orderId;
                                objPayments.CapturedId = charge.Id;
                                objPayments.CapturedAmt = charge.AmountCaptured;
                                objPayments.Currency = charge.Currency;
                                objPayments.Email_name = charge.BillingDetails.Name;
                                objPayments.NetWorkStatus = charge.Outcome.NetworkStatus;
                                objPayments.SellerMessage = charge.Outcome.SellerMessage;
                                objPayments.Paid = charge.Paid;
                                objPayments.PaymentMethod = charge.PaymentMethod;
                                objPayments.Card_brand = charge.PaymentMethodDetails.Card.Brand;
                                objPayments.Funding = charge.PaymentMethodDetails.Card.Funding;
                                objPayments.Country = charge.PaymentMethodDetails.Card.Country;
                                objPayments.Network = charge.PaymentMethodDetails.Card.Network;
                                objPayments.Last4 = charge.PaymentMethodDetails.Card.Last4;
                                objPayments.Exp_Month = Convert.ToInt32(charge.PaymentMethodDetails.Card.ExpMonth);
                                objPayments.Exp_Year = Convert.ToInt32(charge.PaymentMethodDetails.Card.ExpYear);
                                objPayments.Status = charge.Status;
                                objPayments.Receipt_url = charge.ReceiptUrl;
                                objPayments.FailureCode = charge.FailureCode;
                                objPayments.FailureMessage = charge.FailureMessage;
                                objPayments.TransactionDate = charge.Created;

                                //Sending message to admin on order receiving
                                SMSManager.SendSMSNotification(ViewBag.CountryMobileCode, objRestaurant.Mobile, ConfigurationManager.AppSettings["OrderReceivedMessage"].ToString().Replace("{0}", obj.OrderNo).Replace("{1}", totalAmount.ToString()));

                                _paymentBusiness.AddPaymentInfo(objPayments, tenantInfo.TenantConnection);
                                SMSManager.SendSMSNotification(ViewBag.CountryMobileCode, objCustomer.MobileNumber, ConfigurationManager.AppSettings["OrderPlacedMessage"].ToString());
                                SessionManager.AddTocartMenuItem = new List<MenuCartModel>();
                            }
                            else
                            {

                            }
                        }
                        return RedirectToAction("PaymentResponse", new { orderId = CryptoEngine.EncryptNew(orderId.ToString()) });
                    }
                    catch (StripeException e)
                    {
                        objPayments.FailureMessage = e.Message.ToString();
                        _paymentBusiness.AddPaymentInfo(objPayments, tenantInfo.TenantConnection);
                        TempData["Error"] = true;
                        return RedirectToAction("GetMenu");
                    }
                    catch (Exception ex)
                    {
                        objPayments.FailureMessage = ex.Message.ToString();
                        _paymentBusiness.AddPaymentInfo(objPayments, tenantInfo.TenantConnection);
                        TempData["Error"] = true;
                        return RedirectToAction("GetMenu");
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return Json(new { result = "-1" }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region "payment response/error"
        public ActionResult PaymentResponse(string orderId = "")
        {
            int orderIdInt = 0;
            try
            {
                if (!string.IsNullOrEmpty(orderId))
                {
                    orderIdInt = Convert.ToInt32(CryptoEngine.DecryptNew(orderId));
                }
            }
            catch (Exception ex)
            {
                orderIdInt = 0;
            }
            if (orderIdInt > 0)
            {
                MenuDataModel obj = new MenuDataModel();
                obj.RestaurantModel = restutantInfo;
                PaginationModel objPagination = new PaginationModel();
                objPagination.PageSize = _pageSize;
                obj.OrderInfo = _orderBusiness.GetOrderList(orderIdInt, string.Empty, string.Empty, false, objPagination, tenantInfo.TenantConnection).FirstOrDefault();
                string template = string.Empty;
                template = System.IO.File.ReadAllText(HttpContext.Server.MapPath("~/Templates/ordered-email-template.html"));
                template = template.Replace("[#LOGO]", obj.RestaurantModel.LogoPath);
                template = template.Replace("[#ORDERNO]", obj.OrderInfo.OrderNo);
                template = template.Replace("[#ORDERDATE]", obj.OrderInfo.OrderedDate.ToString());

                template = template.Replace("[#ORDERSTATUS]", obj.OrderInfo.Status);
                template = template.Replace("[#ORDERTYPE]", obj.OrderInfo.OrderedType == "P" ? "PickUp Order" : "Delivery Order");

                if (obj.OrderInfo.OrderedType == "P")
                {
                    template = template.Replace("[#IsShowPickDateTime]", "block");
                    template = template.Replace("[#PICKUPDATETIME]", obj.OrderInfo.PickUpDateTime.ToString());
                }
                else
                {
                    template = template.Replace("[#IsShowPickDateTime]", "none");
                }
                template = template.Replace("[#PAYMENTMODE]", obj.OrderInfo.Mode);
                template = template.Replace("[#PAYMENTSTATUS]", (!string.IsNullOrEmpty(obj.OrderInfo.PaymentStatus) ? obj.OrderInfo.PaymentStatus : "---"));


                template = template.Replace("[#CUSTOMERNAME]", obj.OrderInfo.CustomerName);
                template = template.Replace("[#CUSTOMERADDRESS]", obj.OrderInfo.CustomerAddress);
                template = template.Replace("[#CUSTOMERMOBILE]", obj.OrderInfo.MobileNumber);

                string orderDetailsStr = RenderPartialViewToString("_OrderedEmailTemplateData", obj.OrderInfo.OrderedDetails);
                template = template.Replace("[#ORDERDETAILS]", orderDetailsStr);

                template = template.Replace("[#SUBTOTAL]", string.Format("{0}{1}", restutantInfo.CurrencySymbol, obj.OrderInfo.OrderedDetails.Select(c => c.TotalAmount).Sum().ToString()));
                string _taxAmt = String.Format("{0:0.00}", obj.OrderInfo.TaxAmt.ToString());
                string _deliveryCharges = String.Format("{0:0.00}", obj.OrderInfo.DeliveryCharges.ToString());
                template = template.Replace("[#TaxRate]", obj.OrderInfo.TaxRate.ToString());
                template = template.Replace("[#TaxAmt]", string.Format("{0}{1}", restutantInfo.CurrencySymbol, _taxAmt));
                template = template.Replace("[#DeliveryCharges]", string.Format("{0}{1}", restutantInfo.CurrencySymbol, _deliveryCharges));

                template = template.Replace("[#TOTALAMOUNT]", string.Format("{0}{1}", restutantInfo.CurrencySymbol, (obj.OrderInfo.OrderedDetails.Select(c => c.TotalAmount).Sum() + obj.OrderInfo.TaxAmt + obj.OrderInfo.DeliveryCharges)));
                template = template.Replace("[#RESTURANTMOBILE]", obj.RestaurantModel.Mobile);
                template = template.Replace("[#RESTURANTADDRESS]", String.Format("{0} {1}", obj.RestaurantModel.Address, obj.RestaurantModel.ZipCode));

                EmailManager.SendOrderNotification(obj.OrderInfo.CustomerName, obj.OrderInfo.CustomerEmail, obj, template);
                return View(obj);
            }
            else
            {
                MenuDataModel obj = new MenuDataModel();
                return View(obj);
            }
        }
        public ActionResult ErrorResponse()
        {
            return View();
        }
        #endregion

        #region "contactUs"
        [HttpPost]
        public JsonResult SendContactUs(string emailAddress)
        {
            int response = 0;
            if (_contactUsBusiness.SaveContactUs(emailAddress, tenantInfo.TenantConnection) == 1)
            {
                response = 1;
            }
            return Json(new { response }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region "track order"
        [AllowAnonymous]
        public ActionResult TrackOrder()
        {
            MenuDataModel obj = new MenuDataModel();
            obj.RestaurantModel = restutantInfo;
            return View(obj);
        }

        [HttpPost]
        public JsonResult GetOrderInfo(string orderNo = "")
        {
            try
            {
                string jsonStr = string.Empty;
                PaginationModel objPagination = new PaginationModel();
                objPagination.PageSize = _pageSize;
                OrderViewModel obj = _orderBusiness.GetOrderList(0, orderNo, string.Empty, false, objPagination, tenantInfo.TenantConnection).FirstOrDefault();
                jsonStr = RenderPartialViewToString("~/Views/Menu/_trackOrderInfo.cshtml", obj);
                return Json(new { result = "1", htmlStr = jsonStr }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { result = "-1", htmlStr = string.Empty }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region "helper"
        public virtual string RenderPartialViewToString(string viewName, object viewmodel)
        {
            if (string.IsNullOrEmpty(viewName))
            {
                viewName = this.ControllerContext.RouteData.GetRequiredString("action");
            }

            ViewData.Model = viewmodel;

            using (var sw = new StringWriter())
            {
                ViewEngineResult viewResult = System.Web.Mvc.ViewEngines.Engines.FindPartialView(this.ControllerContext, viewName);
                var viewContext = new ViewContext(this.ControllerContext, viewResult.View, this.ViewData, this.TempData, sw);
                viewResult.View.Render(viewContext, sw);
                viewResult.ViewEngine.ReleaseView(ControllerContext, viewResult.View);

                return sw.GetStringBuilder().ToString();
            }
        }
        #endregion
    }
}