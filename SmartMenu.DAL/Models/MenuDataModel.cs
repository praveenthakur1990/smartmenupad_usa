using System;
using System.Collections.Generic;
using System.Text;

namespace SmartMenu.DAL.Models
{
    public class MenuDataModel
    {
        public RestaurantModelVM RestaurantModel { get; set; }
        public List<CategoryModel> CategoryModelList { get; set; }
        public List<MenuItemViewModel> MenuItemViewModelList { get; set; }
        public OrderViewModel OrderInfo { get; set; }
        public CustomerViewModel CustomerInfo { get; set; }
    }
}
