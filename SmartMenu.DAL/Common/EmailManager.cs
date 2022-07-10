using SmartMenu.DAL.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Net.Mail;
using System.Text;

namespace SmartMenu.DAL.Common
{
    public static class EmailManager
    {
        public static string SendOrderNotification(string customerName, string emailAddress, MenuDataModel obj, string template)
        {
            try
            {
                MailMessage mail = new MailMessage();
                mail.From = new MailAddress(obj.RestaurantModel.Email, obj.RestaurantModel.Name);
                mail.IsBodyHtml = true;
                mail.To.Add(emailAddress);
                if (Convert.ToBoolean(ConfigurationManager.AppSettings["IsSendToBccEmails"].ToString())==true)
                {
                    string[] bccEmiils = ConfigurationManager.AppSettings["bccEmails"].Split(',');
                    foreach (var item in bccEmiils)
                    {
                        mail.Bcc.Add(item);
                    }
                }
                
                mail.Subject = obj.OrderInfo.OrderNo;               
                mail.Priority = MailPriority.High;                
                mail.IsBodyHtml = true;
                mail.Body = template;
                SmtpClient SmtpServer = new SmtpClient();
                SmtpServer.Send(mail);
                return "1";
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }
        }

    }
}
