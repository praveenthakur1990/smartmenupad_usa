using System;
using System.Collections.Generic;
using System.Text;

namespace SmartMenu.DAL.Models
{
    public class ContactUsModel
    {
        public int Id { get; set; }
        public string EmailAddress { get; set; }
        public DateTime? AddedDate { get; set; }
    }

    public class ContactUsVM
    {
        public string EmailAddress { get; set; }
        public DateTime? AddedDate { get; set; }
        public int TotalRows { get; set; }
    }
}
