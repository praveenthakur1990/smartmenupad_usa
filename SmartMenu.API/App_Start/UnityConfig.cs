using SmartMenu.BAL.Interfaces;
using SmartMenu.BAL.Services;
using System.Web.Http;
using Unity;
using Unity.WebApi;

namespace SmartMenu.API
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();
            container.RegisterType<IRestaurantBusiness, RestaurantBusiness>();
            container.RegisterType<ICategoryBusiness, CategoryBusiness>();
            container.RegisterType<IMenuBusiness, MenuBusiness>();
            container.RegisterType<IOrderBusiness, OrderBusiness>();
            container.RegisterType<ICustomerBusiness, CustomerBusiness>();
            container.RegisterType<IAPIKeyBusiness, APIKeyBusiness>();
            container.RegisterType<IBusinessHoursBusiness, BusinessHoursBusiness>();
            container.RegisterType<IQuickPagesBusiness, QuickPagesBusiness>();
            container.RegisterType<ISocialMedialinksBusiness, SocialMedialinksBusiness>();
            container.RegisterType<IContactUsBusiness, ContactUsBusiness>();
            container.RegisterType<IPartnerBusiness, PartnerBusiness>();
            container.RegisterType<IPlanBusiness, PlanBusiness>();
            GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container);
        }
    }
}