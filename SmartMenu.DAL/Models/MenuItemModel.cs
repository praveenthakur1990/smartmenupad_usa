using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SmartMenu.DAL.Models
{
    public class MenuItemModel
    {
        [Key]
        public int Id { get; set; }
        public int? CategoryId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImagePath { get; set; }
        public string VideoPath { get; set; }
        public bool? IsMultipleSize { get; set; }
        public string MultipleSizesJsonStr { get; set; }
        public bool? IsAddOnsChoices { get; set; }
        public string AddOnsJsonStr { get; set; }
        public decimal Price { get; set; }
        public string VegNonVeg { get; set; }
        public string RecommendedItems { get; set; }
        public bool? IsSeasonal { get; set; }
        public string SeasonalMonth { get; set; }
        public bool? IsOnSelectedDays { get; set; }
        public string SelectedDays { get; set; }
        public string ItemAs { get; set; }
        public bool? IsPublish { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public bool? IsDeleted { get; set; }
        public DateTime? DeletedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }
    }

    public class MenuItemViewModel
    {
        public int Id { get; set; }
        public int? CategoryId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImagePath { get; set; }
        public string VideoPath { get; set; }
        public bool? IsMultipleSize { get; set; }
        public string MultipleSizesJsonStr { get; set; }
        public bool? IsAddOnsChoices { get; set; }
        public string AddOnsJsonStr { get; set; }
        public decimal Price { get; set; }
        public string VegNonVeg { get; set; }
        public string RecommendedItems { get; set; }
        public bool? IsSeasonal { get; set; }
        public string SeasonalMonth { get; set; }
        public bool? IsOnSelectedDays { get; set; }
        public string SelectedDays { get; set; }
        public string ItemAs { get; set; }
        public bool? IsPublish { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public bool? IsDeleted { get; set; }
        public DateTime? DeletedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public List<MenuMultipleSizesViewModel> MenuMultipleSizesList { get; set; }
        public List<MenuAddOnsChoicesViewModel> MenuAddOnsChoicesList { get; set; }
        public string CategoryName { get; set; }
        public int Qty { get; set; }
        public decimal FinalPrice { get; set; }

        public List<MenuMultipleSizesViewModel> AddedMenuMultipleSizesList { get; set; }
        public List<MenuAddOnsChoicesViewModel> AddedMenuAddOnsChoicesList { get; set; }
        public string ActionType { get; set; }
        public int RowIndex { get; set; }
        public string CategoryImagePath { get; set; }

        public bool IsShowOnCurrentMonth { get; set; }
        public bool IsShowOnCurrentDay { get; set; }

    }


    public class MenuMultipleSizesViewModel
    {
        public string Name { get; set; }
        public decimal? Price { get; set; }
    }
    public class MenuAddOnsChoicesViewModel
    {
        public string Title { get; set; }
        public bool IsRequired { get; set; }
        public int? Min { get; set; }
        public int? Max { get; set; }
        public List<MenuAddOnsChoicesItemViewModel> AddOnChoiceItems { get; set; }
    }

    public class MenuAddOnsChoicesItemViewModel
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
    }
}
