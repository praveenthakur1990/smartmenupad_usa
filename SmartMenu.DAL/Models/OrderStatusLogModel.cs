using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SmartMenu.DAL.Models
{
    public class OrderStatusLogModel
    {
        [Key]
        public int Id { get; set; }
        public int OrderId { get; set; }
        public string Status { get; set; }
        public string ChangedBy { get; set; }
        public DateTime? ChangedDateTime { get; set; }
    }
}
