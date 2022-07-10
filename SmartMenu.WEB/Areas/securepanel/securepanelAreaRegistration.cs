using System.Web.Mvc;

namespace SmartMenu.WEB.Areas.securepanel
{
    public class securepanelAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "securepanel";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "securepanel_default",
                "securepanel/{controller}/{action}/{id}",
                new { controller = "Account", action = "Login", id = UrlParameter.Optional }
            );
        }
    }
}