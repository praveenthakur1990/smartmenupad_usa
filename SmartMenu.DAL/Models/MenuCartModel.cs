using System;
using System.Collections.Generic;
using System.Text;

namespace SmartMenu.DAL.Models
{
    public class MenuCartModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public string ImagePath { get; set; }
        public string CategoryImagePath { get; set; }
        public int Qty { get; set; }
        public decimal TotalAmount { get; set; }
        public string SpecialInstruction { get; set; }
        public bool? IsMultipleSize { get; set; }
        public List<MenuMultipleSizesViewModel> ItemSizeList { get; set; }
        public List<MenuAddOnsChoicesViewModel> AddOnsItemList { get; set; }
        public string ActionType { get; set; }
        public int RowIndex { get; set; }
    }
}
