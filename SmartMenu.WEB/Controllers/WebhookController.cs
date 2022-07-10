using SmartMenu.BAL.Services;
using SmartMenu.DAL.Common;
using SmartMenu.DAL.Models;
using Stripe;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace SmartMenu.WEB.Controllers
{
    public class WebhookController : Controller
    {
        [HttpPost]
        public string Index()
        {
            StripeConfiguration.ApiKey = ConfigurationManager.AppSettings["SecretKeySuper"].ToString();
            int res = 0;
            var json = new StreamReader(HttpContext.Request.InputStream).ReadToEnd();
            try
            {
                var stripeEvent = EventUtility.ParseEvent(json);
                // Handle the event          
                System.Diagnostics.Debug.WriteLine(string.Format("{0} {1}", "stripe event", stripeEvent.Type));
                SmartMenu.WEB.Helpers.CommonManager.LogError(MethodBase.GetCurrentMethod(), null, stripeEvent.Type);
                if (stripeEvent.Type == Events.PaymentIntentSucceeded)
                {
                    var objPaymentIntent = stripeEvent.Data.Object as PaymentIntent;
                    var customerService = new CustomerService();
                    Customer customer = customerService.Get(objPaymentIntent.CustomerId);
                    var invoiceService = new InvoiceService();
                    Invoice invoice = invoiceService.Get(objPaymentIntent.InvoiceId);

                    var subscriptionService = new SubscriptionService();
                    Subscription subscription = subscriptionService.Get(invoice.SubscriptionId);

                    if (customer != null && !string.IsNullOrEmpty(customer.Email))
                    {
                        TenantsVM objTenants = CommonManager.getTenantConnection(string.Empty, customer.Email).FirstOrDefault();
                        if (objTenants != null)
                        {
                            Charge objCharge = objPaymentIntent.Charges.FirstOrDefault();
                            SubscriptionModel obj = new SubscriptionModel();
                            obj.UserId = objTenants.UserId;
                            obj.CustomerId = subscription.CustomerId;
                            obj.SubscriptionId = subscription.Id;
                            obj.SubscriptionStartDate = subscription.CurrentPeriodStart;
                            obj.SubscriptionEndDate = subscription.CurrentPeriodEnd;
                            obj.latest_invoice = objPaymentIntent.InvoiceId;
                            obj.Amount = objPaymentIntent.Amount;
                            obj.IsCaptured = objCharge.Captured;
                            obj.seller_message = objCharge.Outcome.SellerMessage;
                            obj.brand = objCharge.PaymentMethodDetails.Card.Brand;
                            obj.funding = objCharge.PaymentMethodDetails.Card.Funding;
                            obj.last4 = objCharge.PaymentMethodDetails.Card.Last4;
                            obj.receipt_url = objCharge.ReceiptUrl;
                            obj.status = objPaymentIntent.Status;
                            obj.livemode = objPaymentIntent.Livemode;
                            obj.payment_method = objPaymentIntent.PaymentMethodId;
                            obj.CancellationReason = objPaymentIntent.CancellationReason;
                            obj.CanceledAt = objPaymentIntent.CanceledAt;
                            obj.CreatedDate = objPaymentIntent.Created;
                            res = new SubscriptionBusiness().AddUpDateSubscription(obj, objTenants.TenantConnection);
                        }
                    }
                    System.Diagnostics.Debug.WriteLine(string.Format("{0} {1}", "obj PaymentIntent", objPaymentIntent));
                }
                SmartMenu.WEB.Helpers.CommonManager.LogError(MethodBase.GetCurrentMethod(), null, res);

                if (res > 0)
                {
                    return "1";
                }
                else
                {
                    return "DB return -1";
                }
            }
            catch (StripeException e)
            {
                System.Diagnostics.Debug.WriteLine("stripe exception: {0}", e.Message.ToString());
                SmartMenu.WEB.Helpers.CommonManager.LogError(MethodBase.GetCurrentMethod(), e, null);
                return e.Message.ToString();
            }
        }
    }
}