using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SmartMenu.DAL.Models
{
    public class DbErrorModel
    {
        [Key]
        public int ErrorID { get; set; }

        public string UserName { get; set; }

        public int? ErrorNumber { get; set; }

        public int? ErrorState { get; set; }

        public int? ErrorSeverity { get; set; }

        public int? ErrorLine { get; set; }

        public string ErrorProcedure { get; set; }

        public string ErrorMessage { get; set; }

        public DateTime? ErrorDateTime { get; set; }
    }
}
