using System;
using System.Collections.Generic;
using System.Text;

namespace SmartMenu.DAL.Models
{
    public class PlanModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal? Price { get; set; }
        public bool? IsActive { get; set; }
        public string Interval { get; set; }
        public string PriceId { get; set; }
        public string ProductId { get; set; }
        public string NamePrice { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }
    }
}
