using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace SmartMenu.DAL.Enums
{
    public static class EnumHelper
    {
        public enum RolesEnum
        {
            SuperAdmin,
            Admin,
            Partner,
            PluginAccess
        }

        public enum MethodEnum
        {
            [Description("/token")]
            Login,
            [Description("/api/Account/Logout")]
            Logout,
            [Description("/api/Account/Register")]
            Register,

            [Description("/api/Restaurant/AddUpdateRestaurant")]
            AddUpdateRestaurant,
            [Description("/api/Restaurant/GetRestaurant")]
            GetRestaurant,
            [Description("/api/Restaurant/SetOnlineOfflineRestaurant")]
            SetOnlineOfflineRestaurant,
            [Description("/api/Restaurant/UpdateDeliverAreaSetting")]
            UpdateDeliverAreaSetting,
            [Description("/api/Restaurant/UpdateWebsiteImage")]
            UpdateWebsiteImage,
            [Description("/api/Restaurant/UpdateDeliveryCharges")]
            UpdateDeliveryCharges,

            [Description("/api/Category/AddUpdateCategory")]
            AddUpdateCategory,
            [Description("/api/Category/GetCategories")]
            GetCategories,
            [Description("/api/Category/GetCategoryById")]
            GetCategoryById,
            [Description("/api/Category/MarkeAsDelete")]
            MarkAsDeletedCategory,

            [Description("/api/Menu/AddUpdateMenu")]
            AddUpdateMenu,
            [Description("/api/Menu/GetMenuItemList")]
            GetMenuItemList,
            [Description("/api/Menu/GetMenuById")]
            GetMenuById,
            [Description("/api/Menu/MarkeAsDelete")]
            MarkAsDeletedMenu,

            [Description("/api/Order/GetOrders")]
            GetOrders,
            [Description("/api/Order/UpdateOrderStatus")]
            UpdateOrderStatus,
            [Description("/api/Order/GetOrderGraphData")]
            GetOrderGraphData,
            [Description("/api/Order/GetDashBoardStatsData")]
            GetDashBoardStatsData,


            [Description("/api/Customer/GetCustomers")]
            GetCustomers,

            [Description("/api/APIKey/AddUpdateAPIKey")]
            AddUpdateAPIKey,
            [Description("/api/APIKey/GetAPIKey")]
            GetAPIKey,

            [Description("/api/BusinessHour/GetBusinessHours")]
            GetBusinessHours,
            [Description("/api/BusinessHour/AddUpdateBusinessHour")]
            AddUpdateBusinessHour,
            [Description("/api/BusinessHour/AddUpdatePickUpAddress")]
            AddUpdatePickUpAddress,
            [Description("/api/BusinessHour/AddCustomLabel")]
            AddCustomLabel,

            [Description("/api/QuickPages/GetQuickPages")]
            GetQuickPages,
            [Description("/api/QuickPages/AddUpdateQuickPages")]
            AddUpdateQuickPages,
            [Description("/api/QuickPages/AddUpdateQuickPages_v2")]
            AddUpdateQuickPages_v2,

            [Description("/api/SocialMedia/GetSocialMedia")]
            GetSocialMedia,
            [Description("/api/SocialMedia/AddUpdateSocialMedia")]
            AddUpdateSocialMedia,

            [Description("/api/ContactUs/GetContactUsList")]
            GetContactUsList,

            [Description("/api/Partner/AddUpdatePartner")]
            AddUpdatePartner,
            [Description("/api/Partner/GetPartnerList")]
            GetPartnerList,
            [Description("/api/Partner/GetPartnerById")]
            GetPartnerById,

            [Description("/api/Plan/AddUpdatePlan")]
            AddUpdatePlan,
            [Description("/api/Plan/GetPlanList")]
            GetPlanList,
            [Description("/api/Plan/GetPlanById")]
            GetPlanById


        }

        public enum DirectoryPathEnum
        {
            RestaurantDoc,
            FSSIDoc,
            GSTDOc,
            Upload,
            CategoryImage,
            MenuImage,
            Logo,
            WebsiteImages,
            ErrorLogs
        }

        public enum ExtensionEnum
        {
            [Description(".pdf")]
            PdfExtension,
            [Description(".png")]
            PNGExtension,
            [Description(".cshtml")]
            csHtmlExtension
        }

        public enum PaymentModeEnum
        {
            Cash,
            Card
        }

        public enum OrderStatusEnum
        {
            [Description("Pending")]
            Pending,
            [Description("Accepted")]
            Accepted,
            [Description("Rejected")]
            Rejected,
            [Description("Preparing Food")]
            Preparing_Food,
            [Description("Out for delivery")]
            Out_for_delivery,
            [Description("Delivered")]
            Delivered
        }

        public enum PlanTypeEnum
        {
            Free,
            Paid
        }

        public enum PlanFrequency
        {
            Montly,
            Annually
        }
        public static string GetDescription<T>(this T enumValue)
           where T : struct, IConvertible
        {
            if (!typeof(T).IsEnum)
                return null;

            var description = enumValue.ToString();
            var fieldInfo = enumValue.GetType().GetField(enumValue.ToString());

            if (fieldInfo != null)
            {
                var attrs = fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), true);
                if (attrs != null && attrs.Length > 0)
                {
                    description = ((DescriptionAttribute)attrs[0]).Description;
                }
            }
            return description;
        }
    }
}
