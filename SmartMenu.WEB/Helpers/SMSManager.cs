using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace SmartMenu.WEB.Helpers
{
    public static class SMSManager
    {
        public static bool SendSMSNotification(string countryCode, string toNumber, string message)
        {
            try
            {
                if (Convert.ToBoolean(ConfigurationManager.AppSettings["IsSendOtp"].ToString()) == true)
                {
                    toNumber = SmartMenu.DAL.Common.CommonManager.RemoveSpecialCharacters(toNumber);
                    toNumber = countryCode + toNumber;
                    string accountSid = ConfigurationManager.AppSettings["TwilioAccountSid"].ToString();
                    string authToken = ConfigurationManager.AppSettings["TwilioAuthToken"].ToString();
                    TwilioClient.Init(accountSid, authToken);
                    var response = MessageResource.Create(
                        body: message,
                        from: new Twilio.Types.PhoneNumber(ConfigurationManager.AppSettings["TwilioPhoneNumber"].ToString()),
                        to: new Twilio.Types.PhoneNumber(toNumber)
                    );
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}