using System;
using System.ComponentModel.DataAnnotations;

namespace SmartMenu.DAL.Models
{
    public class QuickPagesModel
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Link { get; set; }
        public string PageContent { get; set; }
        public bool IsActive { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }
    }

    public class QuickPagesVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Link { get; set; }
        public string PageContent { get; set; }
        public bool IsActive { get; set; }
        public bool IsRedirectToUrl { get; set; }
        public string CreatedBy { get; set; }

    }
}
