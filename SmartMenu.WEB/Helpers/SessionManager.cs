using SmartMenu.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmartMenu.WEB.Helpers
{
    public class SessionManager
    {
        public static LoginResponse LoginResponse
        {
            get
            {
                if (HttpContext.Current.Session["LoginResponse"] != null)
                {
                    return (LoginResponse)HttpContext.Current.Session["LoginResponse"];
                }
                else
                {
                    return null;
                }
            }
            set
            {
                HttpContext.Current.Session["LoginResponse"] = value;
            }
        }

        public static List<MenuCartModel> AddTocartMenuItem
        {
            get
            {
                if (HttpContext.Current.Session["AddTocartMenuItem"] != null)
                {
                    return (List<MenuCartModel>)HttpContext.Current.Session["AddTocartMenuItem"];
                }
                else
                {
                    //MenuCartModel obj = new MenuCartModel();
                    //obj.MenuItemOptionalList = new List<MenuItemOptionalModel>();
                    //List<MenuCartModel> objList = new List<MenuCartModel>();
                    //objList.Add(obj);
                    return new List<MenuCartModel>();
                }
            }
            set
            {
                HttpContext.Current.Session["AddTocartMenuItem"] = value;
            }
        }

        public static string OTPNumber
        {
            get
            {
                if (HttpContext.Current.Session["OTPNumber"] != null)
                {
                    return HttpContext.Current.Session["OTPNumber"].ToString();
                }
                else
                {
                    return null;
                }
            }
            set
            {
                HttpContext.Current.Session["OTPNumber"] = value;
            }
        }
    }
}