using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SmartMenu.DAL.Models
{
    public class APIKeyModel
    {
        [Key]
        public int Id { get; set; }
        public string Publishablekey { get; set; }
        public string Secretkey { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
